﻿using System;
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
    class Level
    {
        // Member Variable Declaration
        protected Camera camera;
        protected Dictionary<string, Entity> entities;
        protected Map levelMap;
        protected Player player;
        protected Rectangle screenRect;
        protected Texture2D hideTexture;
        protected Rectangle hideRect;
        protected int hideRectDimen;
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

        // Object Function Declaration
        /// <summary>
        /// The constructor for the Level class. Basically doesn't do anything
        /// important at this point.
        /// </summary>
        public Level(GraphicsDevice graphicsDevice)
        {
            this.entities = new Dictionary<string, Entity>();
            this.camera = new Camera(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            this.screenRect = Rectangle.Empty;
            this.hideTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            this.hideTexture.SetData(new[] { Color.Black });
            this.hideRect = Rectangle.Empty;
            this.hideRectDimen = graphicsDevice.Viewport.Height;
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
            
            this.entities.Add("character", new PlayerCharacter(gameReference));
            this.entities["character"].Load(gameReference.Content);

            //this.entities.Add("theOldMan", new OldManNPC(Vector2.Zero));
            //this.entities["theOldMan"].Load(gameReference.Content);

            //this.entities.Add("theOldMan", new OldManNPC(Vector2.Zero));
            //this.entities["theOldMan"].Load(gameReference.Content);

            //this.entities.Add("theOldMan", new OldManNPC(Vector2.Zero));
            //this.entities["theOldMan"].Load(gameReference.Content);

            //this.entities.Add("theOldMan", new OldManNPC(Vector2.Zero));
            //this.entities["theOldMan"].Load(gameReference.Content);

            //this.entities.Add("theOldMan", new OldManNPC(Vector2.Zero));
            //this.entities["theOldMan"].Load(gameReference.Content);

            // This spawns the character in the right place in the map.
            foreach (MapObject obj in levelMap.ObjectLayers["Spawn"].MapObjects)
            {
                if (obj.Name.Substring(0, 5) == "start" && obj.Name.Substring(5) == oldLevel)
                {
                    this.entities["character"].Location = new Vector2(obj.Bounds.X, obj.Bounds.Y);
                }
            }

            camera.Pos = new Vector2(this.entities["character"].DrawingBox.X + entities["character"].DrawingBox.Width / 2,
                this.entities["character"].DrawingBox.Y + entities["character"].DrawingBox.Height / 2);

            // This is the level-load-in-from-black feature.
            hideRect.X = (int)camera.Pos.X - gameReference.GraphicsDevice.Viewport.Width / 2;
            hideRect.Y = (int)camera.Pos.Y - gameReference.GraphicsDevice.Viewport.Height / 2;
            hideRect.Width = gameReference.GraphicsDevice.Viewport.Width;
            hideRect.Height = hideRectDimen;
        }

        public Dictionary<string, Entity> Entities
        {
            get { return entities; }
        }

        /// <summary>
        /// The Update function will run through the level and perform any
        /// necessary operations for processing the frame. This includes
        /// going through the list of active entities in the level and
        /// updating them as well.
        /// </summary>
        public void Update(GameTime gameTime, Pantheon gameReference)
        {
            foreach (string entityName in this.entities.Keys)
            {
                this.entities[entityName].Update(gameTime, gameReference);
            }
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

            if (!gameReference.controlManager.actions.Aim)
            {
                MoveCamera(gameReference, Vector2.Zero);
            }

            // This is a fairly ugly way of making the tiles draw in the right locations.
            screenRect.X = (int)camera.Pos.X - gameReference.GraphicsDevice.Viewport.Width / 2;
            if (screenRect.X < 0) screenRect.X = 0;
            screenRect.Y = (int)camera.Pos.Y - gameReference.GraphicsDevice.Viewport.Height / 2;
            if (screenRect.Y < 0) screenRect.Y = 0;
            screenRect.Width = (int)camera.Pos.X + gameReference.GraphicsDevice.Viewport.Width / 2;
            screenRect.Height = (int)camera.Pos.Y + gameReference.GraphicsDevice.Viewport.Height / 2;

            if (levelStart)
            {
                gameReference.controlManager.disableControls();
                hideRect.X = (int)camera.Pos.X - gameReference.GraphicsDevice.Viewport.Width / 2;
                hideRect.Y = (int)camera.Pos.Y - gameReference.GraphicsDevice.Viewport.Height / 2;
                hideRect.Width = gameReference.GraphicsDevice.Viewport.Width;
                hideRect.Height = hideRectDimen;

                hideRectDimen -= 20;
                if (hideRectDimen < 0)
                {
                    levelStart = false;
                    gameReference.controlManager.enableControls();
                }
            }

            foreach (MapObject obj in levelMap.ObjectLayers["Spawn"].MapObjects)
            {
                if (obj.Name.Substring(0, 3) == "end" && obj.Bounds.Intersects(this.entities["character"].DrawingBox))
                {
                    gameReference.controlManager.disableControls();
                    hideRect.X = (int)camera.Pos.X - gameReference.GraphicsDevice.Viewport.Width / 2;
                    hideRect.Y = (int)camera.Pos.Y - gameReference.GraphicsDevice.Viewport.Height / 2;
                    hideRect.Width = gameReference.GraphicsDevice.Viewport.Width;
                    hideRect.Height = hideRectDimen;

                    hideRectDimen += 20;
                    if (hideRectDimen > gameReference.GraphicsDevice.Viewport.Height)
                    {
                        levelPlaying = false;
                        nextLevel = obj.Name.Substring(3);
                    }
                }
            }
        }

        public void MoveCamera(Pantheon gameReference, Vector2 offset)
        {
            camera.Pos = new Vector2(this.entities["character"].DrawingBox.X + entities["character"].DrawingBox.Width / 2,
                this.entities["character"].DrawingBox.Y + entities["character"].DrawingBox.Height / 2);
            camera.Pos += offset;
            ((PlayerCharacter)entities["character"]).UpdateLaser(gameReference, offset);
        }

        /// <summary>
        /// The Draw function will draw the level itself, as well as any
        /// subentities that are a part of the level. Each entity will
        /// be in charge of drawing itself, and the level will merely
        /// draw the physical level. (Tiles, Sprites, Etc)
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, this.camera.getTransformation());
            
            levelMap.Draw(spriteBatch, screenRect);

            foreach (string entityName in this.entities.Keys)
            {
                this.entities[entityName].Draw(spriteBatch);
            }
            
            spriteBatch.Draw(hideTexture, hideRect, Color.White);

            spriteBatch.End();
        }
    }
}
