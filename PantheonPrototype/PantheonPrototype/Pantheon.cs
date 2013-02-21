using System;
using System.Collections.Generic;
using System.Linq;
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
    /// This is the main type for your game
    /// </summary>
    public class Pantheon : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public ControlManager controlManager;

        Menu menu;

        HUD hud;

        public Level currentLevel;

        public CutsceneManager CutsceneManager;

        SpriteFont debugFont;

        public Pantheon()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1/30f);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            controlManager = new ControlManager();

            int SCREEN_WIDTH = GraphicsDevice.Viewport.Width;
            int SCREEN_HEIGHT = GraphicsDevice.Viewport.Height;

            menu = new Menu();

            debugFont = Content.Load<SpriteFont>("DebugFont");

            hud = new HUD(GraphicsDevice, Content, SCREEN_WIDTH, SCREEN_HEIGHT, debugFont);

            currentLevel = new Level(GraphicsDevice);

            CutsceneManager = new CutsceneManager();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Map.InitObjectDrawing(graphics.GraphicsDevice);

            menu.Load(this);

            currentLevel.Load("map1", "map0", this);
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content heressssss
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            CutsceneManager.Update(gameTime);

            controlManager.Update();

            // REMOVE LATER
            if (Keyboard.GetState().IsKeyDown(Keys.Back)) { this.Exit(); }

            if (controlManager.actions.Pause)
            {
                this.IsMouseVisible = true;
                menu.Update(gameTime, this);
            }
            else
            {
                menu.MenuState = "main";

                this.IsMouseVisible = false;

                if (currentLevel.LevelPlaying)
                {
                    currentLevel.Update(gameTime, this);
                }
                else
                {
                    string nextLevel = currentLevel.NextLevel;
                    string prevLevel = currentLevel.LevelNum;
                    currentLevel = new Level(GraphicsDevice);
                    currentLevel.Load(nextLevel, prevLevel, this);
                }

                hud.Update(gameTime, this, this.currentLevel);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Public method to allow access to the level's Camera object.
        /// </summary>
        public Camera GetCamera()
        {
            return currentLevel.Camera;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (controlManager.actions.Pause)
            {
                menu.Draw(spriteBatch);
            }
            else
            {
                if (currentLevel.LevelPlaying)
                {
                    currentLevel.Draw(spriteBatch);
                    CutsceneManager.Draw(spriteBatch);
                }
                hud.Draw(spriteBatch, debugFont);
            }

            base.Draw(gameTime);
        }
    }
}
