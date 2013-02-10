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
        protected Texture2D blank;

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
            this.blank = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
        }

        /// <summary>
        /// Loads the level from a descriptive script file on the harddrive.
        /// </summary>
        public void Load(string fileName, ContentManager contentManager)
        {
            // Open file
            // Read file
            // initialize tile set
            //  -- read in tiles
            //  -- associate with ids
            // initialize map
            //  -- create tile map
            //  -- build collision map (?)
            //  -- build portal map (?)
            // initialize entities
            //player = new Player();
            //  -- build entities using factory
            //  -- string together into an entity map
            //  -- initialize each entitiy as it is stored
            // DONE

            levelMap = contentManager.Load<Map>(fileName);
            
            // HACK HACK HACK
            this.entities.Add("character", new PlayerEntity());
            this.entities["character"].Load(contentManager);

            // This spawns the character in the right place in the map.
            for (int i = 0; i < levelMap.TileLayers["Spawn"].Tiles.Length; i++)
            {
                for (int j = 0; j < levelMap.TileLayers["Spawn"].Tiles[i].Length; j++)
                {
                    if (levelMap.TileLayers["Spawn"].Tiles[i][j] != null && levelMap.TileLayers["Spawn"].Tiles[i][j].SourceID == 2)
                    {
                        Rectangle source = levelMap.TileLayers["Spawn"].Tiles[i][j].Target;
                        this.entities["character"].Location = new Rectangle(source.X - source.Width / 2, source.Y - source.Height / 2,
                            entities["character"].Location.Width, entities["character"].Location.Height);
                    }
                }
            }
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
            foreach (TileData tile in levelMap.GetTilesInRegion(this.entities["character"].Location))
            {
                if (tile.SourceID == 0)
                {
                    Rectangle source = tile.Target;
                    Rectangle test = new Rectangle(source.X - source.Width / 2, source.Y - source.Height / 2, source.Width, source.Height);
                    if (test.Intersects(this.entities["character"].Location))
                    {
                        this.entities["character"].Location = this.entities["character"].PrevLocation;
                    }
                }
            }

            // Makes the camera follow the character
            // This works, but I'm not sure if this is where we want to put this. ~Tumbler
            camera.Pos = new Vector2(this.entities["character"].Location.X, this.entities["character"].Location.Y);

            // This is a fairly ugly way of making the tiles draw in the right locations.
            screenRect.X = this.entities["character"].Location.X - gameReference.GraphicsDevice.Viewport.Width / 2;
            if (screenRect.X < 0) screenRect.X = 0;
            screenRect.Y = this.entities["character"].Location.Y - gameReference.GraphicsDevice.Viewport.Height / 2;
            if (screenRect.Y < 0) screenRect.Y = 0;
            screenRect.Width = this.entities["character"].Location.X + gameReference.GraphicsDevice.Viewport.Width / 2;
            screenRect.Height = this.entities["character"].Location.Y + gameReference.GraphicsDevice.Viewport.Height / 2;
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

            spriteBatch.End();
        }

        /// <summary>
        /// The method to draw the lines for the level edging.
        /// </summary>
        /// <param name="batch">The object that allows the drawing.</param>
        /// <param name="width">The thickness of the line.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="point1">The start point for the line.</param>
        /// <param name="point2">The end point for the line.</param>
        void DrawLine(SpriteBatch batch, float width, Color color, Vector2 point1, Vector2 point2)
        {
            // The angle between the two points (to get the rotation of the texture) and the distance between them
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);
            blank.SetData(new[] { color });

            // Drawing the empty texture from point1 to point2
            batch.Draw(blank, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
        }
    }
}
