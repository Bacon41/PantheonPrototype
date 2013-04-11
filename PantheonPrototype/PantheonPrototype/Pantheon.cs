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
using Test;

namespace PantheonPrototype
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Pantheon : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public Random rand;

        public ControlManager ControlManager;

        public CutsceneManager CutsceneManager;

        public QuestManager QuestManager;

        public EventManager EventManager;

        public Entity player;

        public Level currentLevel;

        Menu menu;

        HUD hud;

        SpriteFont debugFont;

        public Pantheon()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // This sets the screen to full based on the device and removes the border.
            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
            var form = System.Windows.Forms.Control.FromHandle(this.Window.Handle).FindForm();
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1/30f);
        }

        /// <summary>
        /// This is the code to set the window size to the display size.
        /// </summary>
        /// <param name="sender">I'm not really sure.</param>
        /// <param name="e">This stores the information about the actual screen hardware.</param>
        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            DisplayMode displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferFormat = displayMode.Format;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = 800; // displayMode.Width;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = 600; // displayMode.Height;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            rand = new Random();
            ControlManager = new ControlManager();

            int SCREEN_WIDTH = GraphicsDevice.Viewport.Width;
            int SCREEN_HEIGHT = GraphicsDevice.Viewport.Height;

            menu = new Menu(SCREEN_WIDTH, SCREEN_HEIGHT);

            player = new PlayerCharacter(this);

            debugFont = Content.Load<SpriteFont>("DebugFont");

            hud = new HUD(GraphicsDevice, Content, SCREEN_WIDTH, SCREEN_HEIGHT, debugFont);

            currentLevel = new Level(GraphicsDevice);

            CutsceneManager = new CutsceneManager(GraphicsDevice);

            EventManager = new EventManager(this);

            QuestManager = new QuestManager(this);

            XMLTest testingStuffs = this.Content.Load<XMLTest>("TestXML");
            Console.WriteLine(testingStuffs.Name);
            foreach(string line in testingStuffs.List)
            {
                Console.WriteLine(line);
            }

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

            ControlManager.actions.Pause = true;
        }

        internal void StartGame()
        {
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
            CutsceneManager.Update(gameTime, this);

            ControlManager.Update();

            // REMOVE LATER
            if (Keyboard.GetState().IsKeyDown(Keys.Back)) { this.Exit(); }

            if (ControlManager.actions.Pause)
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

            if (ControlManager.actions.Pause)
            {
                menu.Draw(spriteBatch, debugFont);
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
