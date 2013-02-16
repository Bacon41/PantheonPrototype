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

        public Menu()
        {
            items = new Dictionary<string, MenuItem>();
        }

        public void Load(Pantheon gameReference)
        {
            MenuItem item = new MenuItem("Resume", new Rectangle(0, 0, 100, 20));
            item.Load(gameReference);
            items.Add("resume", item);
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
        }

        /// <summary>
        /// The method for drawing the whole menu.
        /// </summary>
        /// <param name="spriteBatch">What is used to draw.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (string itemName in this.items.Keys)
            {
                this.items[itemName].Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
