using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FuncWorks.XNA.XTiled;

namespace PantheonPrototype
{
    /// <summary>
    /// This is the preliminary level class.
    /// The level class handles rendering the map
    /// and handling all entities that happen to
    /// populate said map. These entities will be used
    /// to build the world and create event triggering,
    /// enemies, NPCs, etc. 
    ///
    /// This should be where the majority of the game logic
    /// goes, via the interaction between entities.
    /// 
    /// Level content will be loaded dynamically through
    /// some sort of WAD-type file.
    ///</summary>
    public class Level
    {
        // Member Variable Declaration
        public Camera Camera;
        protected Dictionary<string, Entity> entities;
        protected Map levelMap;
        //protected Player player;
        protected Rectangle screenRect;
        
        protected bool levelStart;
        protected bool levelPlaying;

        public bool LevelPlaying
        {
            get { return levelPlaying; }
        }

        protected string levelNum;

        public string LevelNum
        {
            get { return levelNum; }
        }

        protected string nextLevel;

        public string NextLevel
        {
            get { return nextLevel; }
        }
		
		public Dictionary<string, Entity> Entities
        {
            get { return entities; }
        }
		
		/// <summary>
        /// A list of entities to add to the level entity list.
        /// </summary>
        public Dictionary<string, Entity> addList;

        /// <summary>
        /// A list of entities to remove from the level entity list.
        /// </summary>
        public List<string> removeList;

        // Object Function Declaration
        /// <summary>
        /// The constructor for the Level class. Basically doesn't do anything
        /// important at this point.
        /// </summary>
        public Level(GraphicsDevice graphicsDevice)
        {
            this.entities = new Dictionary<string, Entity>();
			this.addList = new Dictionary<string, Entity>();
            this.removeList = new List<string>();
            this.Camera = new Camera(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            this.screenRect = Rectangle.Empty;
            this.levelStart = true;
            this.levelPlaying = true;
        }

        /// <summary>
        /// Loads the level from a descriptive script file on the harddrive.
        /// </summary>
        public void Load(string newLevel, string oldLevel, Pantheon gameReference)
        {
            levelMap = gameReference.Content.Load<Map>(newLevel);
            levelNum = newLevel;
            
            this.entities.Add("character", gameReference.player);
            this.entities["character"].Load(gameReference.Content);

            // This spawns the character in the right place in the map.
            foreach (MapObject obj in levelMap.ObjectLayers["Spawn"].MapObjects)
            {
                if (obj.Name.Substring(0, 5) == "start" && obj.Name.Substring(5) == oldLevel)
                {
                    this.entities["character"].Location = new Vector2(obj.Bounds.Center.X, obj.Bounds.Center.Y);
                }
                if (obj.Name.Contains("Friend"))
                {
                    this.entities.Add(obj.Name, new OldManFriend(new Vector2(obj.Bounds.Center.X, obj.Bounds.Center.Y)));
                    this.entities[obj.Name].Load(gameReference.Content);
                }
                if (obj.Name.Contains("Enemy"))
                {
                    this.entities.Add(obj.Name, new ButterflyEnemy(new Vector2(obj.Bounds.Center.X, obj.Bounds.Center.Y)));
                    this.entities[obj.Name].Load(gameReference.Content);
                }
            }
            Camera.Pos = new Vector2(this.entities["character"].Location.X, this.entities["character"].Location.Y);

            gameReference.CutsceneManager.PlayLevelLoad(gameReference);
        }

        /// <summary>
        /// The Update function will run through the level and perform any
        /// necessary operations for processing the frame. This includes
        /// going through the list of active entities in the level and
        /// updating them as well.
        /// </summary>
        public void Update(GameTime gameTime, Pantheon gameReference)
        {
            // Updating all entities
            foreach (string entityName in this.entities.Keys)
            {
                this.entities[entityName].Update(gameTime, gameReference);
            }

            // Checking the character entity for collision with nonwalkable tiles.
            foreach (TileData tile in levelMap.GetTilesInRegion(this.entities["character"].BoundingBox))
            {
                if (levelMap.SourceTiles[tile.SourceID].Properties["isWalkable"].AsBoolean == false)
                {
                    Rectangle test = new Rectangle(tile.Target.X - tile.Target.Width / 2, tile.Target.Y - tile.Target.Height / 2,
                        tile.Target.Width, tile.Target.Height);
                    if (test.Intersects(this.entities["character"].BoundingBox))
                    {
                        this.entities["character"].Location = this.entities["character"].PrevLocation;
                    }
                }
            }

            // Black magicks to select all the bullets (SQL in C# with XNA and LINQ!!! Look at all the acronyms! Also, I feel nerdy.)
            var bulletQuery = from entity in this.entities where entity.Key.Contains("bullet") select entity.Key;
            var friendBoundsQuery = from obj in levelMap.ObjectLayers["Spawn"].MapObjects where obj.Name.Contains("Friend") select obj;
            var friendEntityQuery = from entity in this.entities where entity.Key.Contains("Friend") select entity.Key;
            var enemyBoundsQuery = from obj in levelMap.ObjectLayers["Spawn"].MapObjects where obj.Name.Contains("Enemy") select obj;
            var enemyEntityQuery = from entity in this.entities where entity.Key.Contains("Enemy") select entity.Key;
            
            // Checking all bullets for collision with nonshootable tiles, NPCs, enemys, and the player.
            foreach (String bulletKey in bulletQuery)
            {
                if (((Projectile)this.entities[bulletKey]).ToDestroy)
                {
                    this.removeList.Add(bulletKey);
                }
                else if (this.entities[bulletKey].BoundingBox.X > 0 && this.entities[bulletKey].BoundingBox.Right < levelMap.Width * levelMap.TileWidth
                    && this.entities[bulletKey].BoundingBox.Y > 0 && this.entities[bulletKey].BoundingBox.Bottom < levelMap.Height * levelMap.TileHeight)
                {
                    foreach (TileData tile in levelMap.GetTilesInRegion(this.entities[bulletKey].BoundingBox))
                    {
                        if (levelMap.SourceTiles[tile.SourceID].Properties["isShootable"].AsBoolean == false)
                        {
                            Rectangle test = new Rectangle(tile.Target.X - tile.Target.Width / 2, tile.Target.Y - tile.Target.Height / 2,
                                tile.Target.Width, tile.Target.Height);

                            if (test.Intersects(this.entities[bulletKey].BoundingBox))
                            {
                                this.removeList.Add(bulletKey);
                                break;
                            }
                        }
                    }
                    foreach (String friendKey in friendEntityQuery)
                    {
                        if (this.entities[bulletKey].BoundingBox.Intersects(this.entities[friendKey].BoundingBox))
                        {
                            this.removeList.Add(bulletKey);
                        }
                    }
                    foreach (String enemyKey in enemyEntityQuery)
                    {
                        if (this.entities[bulletKey].BoundingBox.Intersects(this.entities[enemyKey].BoundingBox))
                        {
                            this.removeList.Add(bulletKey);
                            this.removeList.Add(enemyKey);
                        }
                    }
                    if (this.entities[bulletKey].BoundingBox.Intersects(this.entities["character"].BoundingBox))
                    {
                        this.removeList.Add(bulletKey);
                        ((PlayerCharacter)this.entities["character"]).Damage(((Bullet)this.entities[bulletKey]).Damage);
                    }
                }
                else
                {
                    this.removeList.Add(bulletKey);
                }
            }

            // Checking each npc for collisions with their bounds and with the player.
            foreach (MapObject boundObj in friendBoundsQuery)
            {
                foreach (String friendKey in friendEntityQuery)
                {
                    if (boundObj.Name == friendKey)
                    {
                        if (!boundObj.Bounds.Contains(this.entities[friendKey].BoundingBox))
                        {
                            this.entities[friendKey].Location = this.entities[friendKey].PrevLocation;
                        }
                    }
                    if (this.entities["character"].BoundingBox.Intersects(this.entities[friendKey].BoundingBox))
                    {
                        this.entities[friendKey].Location = this.entities[friendKey].PrevLocation;
                        this.entities["character"].Location = this.entities["character"].PrevLocation;
                    }
                    if (this.entities["character"].BoundingBox.Intersects(((NPCCharacter)this.entities[friendKey]).ComfortZone))
                    {
                        ((NPCCharacter)this.entities[friendKey]).IsRoaming = false;
                        float angle = (float)Math.Atan2(entities["character"].Location.Y - entities[friendKey].Location.Y,
                            entities["character"].Location.X - entities[friendKey].Location.X);
                        ((CharacterEntity)this.entities[friendKey]).AngleFacing = angle;
                        ((CharacterEntity)this.entities[friendKey]).Facing =
                            HamburgerHelper.reduceAngle(entities["character"].Location - entities[friendKey].Location);
                    }
                    else
                    {
                        ((NPCCharacter)this.entities[friendKey]).IsRoaming = true;
                    }
                }
            }

            // Checking each enemy for collisions with their bounds and with the player.
            foreach (MapObject boundObj in enemyBoundsQuery)
            {
                foreach (String enemyKey in enemyEntityQuery)
                {
                    if (boundObj.Name == enemyKey)
                    {
                        if (!boundObj.Bounds.Contains(this.entities[enemyKey].BoundingBox))
                        {
                            this.entities[enemyKey].Location = this.entities[enemyKey].PrevLocation;
                        }
                    }
                    if (this.entities["character"].BoundingBox.Intersects(this.entities[enemyKey].BoundingBox))
                    {
                        this.entities[enemyKey].Location = this.entities[enemyKey].PrevLocation;
                        this.entities["character"].Location = this.entities["character"].PrevLocation;
                    }
                    if (this.entities["character"].BoundingBox.Intersects(((EnemyNPC)this.entities[enemyKey]).ComfortZone))
                    {
                        ((NPCCharacter)this.entities[enemyKey]).IsRoaming = false;
                        float angle = (float)Math.Atan2(entities["character"].Location.Y - entities[enemyKey].Location.Y,
                            entities["character"].Location.X - entities[enemyKey].Location.X);
                        ((CharacterEntity)this.entities[enemyKey]).AngleFacing = angle;
                        ((CharacterEntity)this.entities[enemyKey]).Facing =
                            HamburgerHelper.reduceAngle(entities["character"].Location - entities[enemyKey].Location);
                    }
                    else
                    {
                        ((NPCCharacter)this.entities[enemyKey]).IsRoaming = true;
                    }
                }
            }

            // Update the entity list
            foreach (string entityName in this.removeList)
            {
                this.entities.Remove(entityName);
            }
            this.removeList.RemoveRange(0, this.removeList.Count);

            foreach (string entityName in this.addList.Keys)
            {
                this.entities.Add(entityName, addList[entityName]);
            }
            this.addList = new Dictionary<string, Entity>();

            // Updating the camera when the character isn't scoping.
            if (!gameReference.controlManager.actions.Aim)
            {
                Camera.Pos = new Vector2(this.entities["character"].Location.X, this.entities["character"].Location.Y);
            }

            // This is a fairly ugly way of making the tiles draw in the right locations.
            screenRect.X = (int)Camera.Pos.X - gameReference.GraphicsDevice.Viewport.Width / 2;
            if (screenRect.X < 0) screenRect.X = 0;
            screenRect.Y = (int)Camera.Pos.Y - gameReference.GraphicsDevice.Viewport.Height / 2;
            if (screenRect.Y < 0) screenRect.Y = 0;
            screenRect.Width = (int)Camera.Pos.X + gameReference.GraphicsDevice.Viewport.Width / 2;
            screenRect.Height = (int)Camera.Pos.Y + gameReference.GraphicsDevice.Viewport.Height / 2;

            // This checks each spawn oblect for collision with the character, and tells it to end the level if necessary.
            foreach (MapObject obj in levelMap.ObjectLayers["Spawn"].MapObjects)
            {
                if (obj.Name.Substring(0, 3) == "end" && obj.Bounds.Intersects(this.entities["character"].DrawingBox))
                {
                    levelPlaying = !gameReference.CutsceneManager.CutsceneEnded;
                    nextLevel = obj.Name.Substring(3);
                    if (!gameReference.CutsceneManager.CutscenePlaying)
                    {
                        if (!levelPlaying) break;
                        gameReference.CutsceneManager.PlayLevelEnd(gameReference);
                    }
                }
            }
        }

        /// <summary>
        /// The Draw function will draw the level itself, as well as any
        /// subentities that are a part of the level. Each entity will
        /// be in charge of drawing itself, and the level will merely
        /// draw the physical level. (Tiles, Sprites, Etc)
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, this.Camera.getTransformation());
            
            levelMap.Draw(spriteBatch, screenRect);

            foreach (string entityName in this.entities.Keys)
            {
                this.entities[entityName].Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
﻿
