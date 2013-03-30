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
        protected DialogueManager dialogueManager;
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
            this.dialogueManager = null;
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
                    this.entities.Add(obj.Name, new ButterflyEnemy(new Vector2(obj.Bounds.Center.X, obj.Bounds.Center.Y), gameReference.Content));
                    this.entities[obj.Name].Load(gameReference.Content);
                }
                if (obj.Name.Contains("Trigger"))
                {
                    this.entities.Add(obj.Name, new Trigger(new Rectangle(obj.Bounds.Left, obj.Bounds.Top, obj.Bounds.Width, obj.Bounds.Height), gameReference));
                    this.entities[obj.Name].Load(gameReference.Content);
                    ((Trigger)this.entities[obj.Name]).ReactivateTime = (int)obj.Properties["ReactivateTime"].AsInt32;
                }
            }
            Camera.Pos = new Vector2(this.entities["character"].Location.X, this.entities["character"].Location.Y);

            gameReference.CutsceneManager.PlayLevelLoad(gameReference);

            // Load the dialogue manager...
            this.dialogueManager = new DialogueManager(gameReference.Content.Load<SpriteFont>("DialogueFont"));
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

            // Black magicks to select all the bullets (SQL in C# with XNA and LINQ!!! Look at all the acronyms! Also, I feel nerdy.)
            // +1 this --^
            var bulletQuery = from entity in this.entities where entity.Key.Contains("bullet") select entity.Key;
            var friendEntityQuery = from entity in this.entities where entity.Key.Contains("Friend") select entity.Key;
            var enemyEntityQuery = from entity in this.entities where entity.Key.Contains("Enemy") select entity.Key;

            // Checking all bullets for end of life.
            foreach (String entityKey in entities.Keys)
            {
                if ((this.entities[entityKey]).ToDestroy)
                {
                    this.removeList.Add(entityKey);
                }
            }

            // Update AI roaming (might consider putting this in an AI manager class)
            foreach (String friendKey in friendEntityQuery)
            {
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

            foreach (String enemyKey in enemyEntityQuery)
            {
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

            // Take care of all collisions
            detectCollisions(gameReference);

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

            // Update the DialogueManager.
            this.dialogueManager.Update(gameTime, gameReference);

            // Updating the camera when the character isn't scoping.
            if (!gameReference.ControlManager.actions.Aim)
            {
                Camera.Pos = new Vector2(this.entities["character"].DrawingBox.X + entities["character"].DrawingBox.Width / 2,
                    this.entities["character"].DrawingBox.Y + entities["character"].DrawingBox.Height / 2);
            }

            // This is a fairly ugly way of making the tiles draw in the right locations.
            screenRect.X = (int)Camera.Pos.X - gameReference.GraphicsDevice.Viewport.Width / 2;
            if (screenRect.X < 0) screenRect.X = 0;
            screenRect.Y = (int)Camera.Pos.Y - gameReference.GraphicsDevice.Viewport.Height / 2;
            if (screenRect.Y < 0) screenRect.Y = 0;
            screenRect.Width = (int)Camera.Pos.X + gameReference.GraphicsDevice.Viewport.Width / 2;
            screenRect.Height = (int)Camera.Pos.Y + gameReference.GraphicsDevice.Viewport.Height / 2;

            // Update the Quest Manager
            gameReference.QuestManager.Update(gameTime);
        }

        /// <summary>
        /// A central function for handling all collisions.
        /// </summary>
        /// <param name="gameReference">One of those universal reference thingies.</param>
        private void detectCollisions(Pantheon gameReference)
        {
            //Create a list of entities
            List<string> entityNameList = Entities.Keys.ToList<string>();
            List<Entity> entityList = Entities.Values.ToList<Entity>();

            // Go through all the entities
            for (int i = 0; i < entityList.Count; i++)
            {
                // Go through the bounds
                if (entityList[i].BoundingBox.X <= 0
                    || entityList[i].BoundingBox.Right >= levelMap.Width * levelMap.TileWidth
                    || entityList[i].BoundingBox.Y <= 0
                    || entityList[i].BoundingBox.Bottom >= levelMap.Height * levelMap.TileHeight)
                {
                    // The entity is outside the bounds, so delete it
                    this.removeList.Add(entityNameList[i]);

                    // Done updating the entity
                    continue;
                }

                // Go through all the tiles
                foreach (TileData tile in levelMap.GetTilesInRegion(entityList[i].BoundingBox))
                {
                    checkTiles(entityNameList[i], entityList[i], tile);
                }

                // Go through all the map objects
                foreach (MapObject obj in levelMap.GetObjectsInRegion(entityList[i].BoundingBox))
                {
                    checkObjects(entityNameList[i], entityList[i], obj, gameReference);
                }

                // Check against all other entities
                for (int j = i + 1; j < entityList.Count; j++)
                {
                    if (entityList[i].BoundingBox.Intersects(entityList[j].BoundingBox))
                    {

                        List<string> collidedNames = new List<string>();
                        List<Entity> collidedEntities = new List<Entity>();

                        collidedNames.Add(entityNameList[i]);
                        collidedEntities.Add(entityList[i]);
                        collidedNames.Add(entityNameList[j]);
                        collidedEntities.Add(entityList[j]);

                        checkEntities(collidedNames, collidedEntities, gameReference);
                    }
                }
            }
        }

        /// <summary>
        /// Checks the appropriate characteristics for the given tile collision.
        /// </summary>
        /// <param name="entityName">The name of the entity colliding with the tile.</param>
        /// <param name="entity">The entity that collides with the tile.</param>
        /// <param name="tile">The tile to be checked.</param>
        private void checkTiles(string entityName, Entity entity, TileData tile)
        {
            // The actual location and dimensions of the tile in map coordinates
            Rectangle tileRect = new Rectangle(
                tile.Target.X - tile.Target.Width / 2,
                tile.Target.Y - tile.Target.Height / 2,
                tile.Target.Width,
                tile.Target.Height);

            // Check against shootable tiles if appropriate
            if (entity.Characteristics.Contains("Projectile"))
            {
                if (levelMap.SourceTiles[tile.SourceID].Properties["isShootable"].AsBoolean == false)
                {
                    if (tileRect.Intersects(entity.BoundingBox))
                    {
                        this.removeList.Add(entityName);
                    }
                }
            }

            // Check against walkable tiles if appropriate
            if (entity.Characteristics.Contains("Walking"))
            {
                if (levelMap.SourceTiles[tile.SourceID].Properties["isWalkable"].AsBoolean == false)
                {
                    if (tileRect.Intersects(entity.BoundingBox))
                    {
                        entity.Location = entity.PrevLocation;
                    }
                }
            }
        }

        /// <summary>
        /// Checks the appropriate characteristics for the given object collision.
        /// </summary>
        /// <param name="entityName">The name of the entity that has collided with an object.</param>
        /// <param name="entity">The entity that has colided with an object.</param>
        /// <param name="obj">The object which the entity has collided with.</param>
        /// <param name="gameReference">An inconvenience, necessary for level loading etc...</param>
        private void checkObjects(string entityName, Entity entity, MapObject obj, Pantheon gameReference)
        {
            if (entity.Characteristics.Contains("Friendly"))
            {
                if (obj.Name == entityName)
                {
                    if (!obj.Bounds.Contains(entity.BoundingBox))
                    {
                        entity.Location = entity.PrevLocation;
                    }
                }
            }

            if (entity.Characteristics.Contains("Enemy"))
            {
                if (obj.Name == entityName)
                {
                    if (!obj.Bounds.Contains(entity.BoundingBox))
                    {
                        entity.Location = entity.PrevLocation;
                    }
                }
            }

            // Check for level markers if appropriate
            if (entity.Characteristics.Contains("Player"))
            {
                if (obj.Name.Substring(0, 3).Equals("end") && obj.Bounds.Intersects(entity.BoundingBox))
                {
                    levelPlaying = !gameReference.CutsceneManager.CutsceneEnded;
                    nextLevel = obj.Name.Substring(3);
                    if (!gameReference.CutsceneManager.CutscenePlaying)
                    {
                        //if (!levelPlaying) { break; }
                        gameReference.CutsceneManager.PlayLevelEnd(gameReference);
                    }
                }
            }
        }

        /// <summary>
        /// Checks the appropriate characteristics for the given entity collision.
        /// </summary>
        /// <param name="entityOneName">The name of the first entity in the collision.</param>
        /// <param name="entityOne">The first entity in the collision.</param>
        /// <param name="entityTwoName">The name of the second entity in the collision.</param>
        /// <param name="entityTwo">The second entity in the collision.</param>
        private void checkEntities(List<string> entityNames, List<Entity> entityList, Pantheon gameReference)
        {





            // Check for collisions from the first entity to the second and from the second to the first
            for (int i = 0; i < entityList.Count; i++)
            {















                // Hackses to select the other entity
                int j = (i + 1) % 2;




                // Projectile collisions
                if (entityList[i].Characteristics.Contains("Projectile"))
                {
                    if (entityList[j].Characteristics.Contains("Friendly"))
                    {
                        this.removeList.Add(entityNames[i]);
                    }
                    else if (entityList[j].Characteristics.Contains("Enemy"))
                    {
                        this.removeList.Add(entityNames[i]);
                        ((EnemyNPC)entityList[j]).Damage(((Bullet)entityList[i]).Damage);
                    }
                    else if (entityList[j].Characteristics.Contains("Player"))
                    {
                        this.removeList.Add(entityNames[i]);
                        ((PlayerCharacter)entityList[j]).Damage(((Bullet)entityList[i]).Damage);
                    }

                    if (entityNames[j].Contains("Bunny"))
                    {
                        Console.WriteLine(entityNames[j] + " collided with " + entityNames[i]);
                    }
                }

                // Inter-walker collisions
                if (entityList[i].Characteristics.Contains("Walking") && entityList[j].Characteristics.Contains("Walking"))
                {


                    entityList[i].Location = entityList[i].PrevLocation;
                    entityList[j].Location = entityList[j].PrevLocation;
                }

                // Trigger collisions
                if (entityList[i].Characteristics.Contains("Triggerable"))
                {


                    if (entityList[j].Characteristics.Contains("Player"))
                    {
                        Dictionary<string, string> bunnyList = new Dictionary<string, string>();
                        bunnyList.Add("Trigger", entityNames[j]);
                        bunnyList.Add("Payload", "Bunnies");
                        gameReference.EventManager.notify(new Event("TriggerEventWithBunnies!!!", bunnyList));
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
            this.dialogueManager.Draw(spriteBatch);

            foreach (string entityName in this.entities.Keys)
            {
                this.entities[entityName].Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}