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

namespace PantheonPrototype
{
    /// <summary>
    /// This is the class for drawing our pause menu.
    /// </summary>
    class Menu
    {
        protected Dictionary<string, MenuItem> items;
        protected Rectangle backgroundRect;
        protected Texture2D backgroundTex;

        public Menu()
        {
            items = new Dictionary<string, MenuItem>();
        }

        public void Load(Pantheon gameReference)
        {
            backgroundRect = new Rectangle((gameReference.GraphicsDevice.Viewport.Width - 300) / 2,
                (gameReference.GraphicsDevice.Viewport.Height - 400) / 2, 300, 400);
            
            backgroundTex = new Texture2D(gameReference.GraphicsDevice, 1, 1);
            backgroundTex.SetData(new[] { Color.White });
            
            loadDefaultMenu(gameReference);
        }

        /// <summary>
        /// This is the method to set up a basic menu with items in it (resume / exit).
        /// </summary>
        /// <param name="gameReference">The whole game.</param>
        private void loadDefaultMenu(Pantheon gameReference)
        {
            MenuItem resumeItem = new MenuItem("Resume", new Rectangle((backgroundRect.Width - 250) / 2 + backgroundRect.X,
                backgroundRect.Y + 25, 250, 50));
            resumeItem.Load(gameReference);
            items.Add("resume", resumeItem);

            MenuItem exitItem = new MenuItem("Exit", new Rectangle((backgroundRect.Width - 250) / 2 + backgroundRect.X,
                backgroundRect.Y + 100, 250, 50));
            exitItem.Load(gameReference);
            items.Add("exit", exitItem);
        }

        /// <summary>
        /// The method for updating the menu items.
        /// </summary>
        /// <param name="gameTime">How much time has elapsed.</param>
        /// <param name="gameReference">The reference to everything.</param>
        public void Update(GameTime gameTime, Pantheon gameReference)
        {
            foreach (string itemName in this.items.Keys)
            {
                this.items[itemName].Update(gameTime, gameReference);
            }

            if (gameReference.controlManager.actions.Attack)
            {
                if (items["resume"].DrawBox.Contains((int)gameReference.controlManager.actions.CursorPosition.X,
                    (int)gameReference.controlManager.actions.CursorPosition.Y))
                {
                    gameReference.controlManager.actions.Pause = false;
                }
                if (items["exit"].DrawBox.Contains((int)gameReference.controlManager.actions.CursorPosition.X,
                    (int)gameReference.controlManager.actions.CursorPosition.Y))
                {
                    gameReference.Exit();
                }
            }
        }

        /// <summary>
        /// The method for drawing the whole menu.
        /// </summary>
        /// <param name="spriteBatch">What is used to draw.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTex, backgroundRect, Color.White);
            foreach (string itemName in this.items.Keys)
            {
                this.items[itemName].Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
