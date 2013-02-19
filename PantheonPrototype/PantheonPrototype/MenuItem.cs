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
    /// This is the class that contains a component of the menu screen.
    /// </summary>
    class MenuItem
    {
        protected Texture2D background;
        protected Texture2D selcted;
        protected SpriteFont font;
        protected Boolean isSelected;

        /// <summary>
        /// The text to display on the menu item.
        /// </summary>
        protected string text;

        public string Text
        {
            get { return text; }
        }

        /// <summary>
        /// The draw box for the item.
        /// </summary>
        protected Rectangle drawBox;

        public Rectangle DrawBox
        {
            get { return drawBox; }
            set { drawBox = value; }
        }

        public MenuItem(string text, Rectangle drawBox)
        {
            this.text = text;
            this.drawBox = drawBox;
            this.isSelected = false;
        }

        public Boolean IsSelected
        {
            set { isSelected = value; }
        }

        public void Load(Pantheon gameReference)
        {
            background = gameReference.Content.Load<Texture2D>("Button");
            selcted = gameReference.Content.Load<Texture2D>("ButtonSelect");
            //background = new Texture2D(gameReference.GraphicsDevice, 1, 1);
            //background.SetData(new[] { Color.Gray });

            font = gameReference.Content.Load<SpriteFont>("DebugFont");
        }

        /// The method for updating the actual item.
        /// </summary>
        /// <param name="gameTime">How much time has elapsed.</param>
        /// <param name="gameReference">The reference to everything.</param>
        public void Update(GameTime gameTime, Pantheon gameReference)
        {
        }

        /// <summary>
        /// The method for drawing the item.
        /// </summary>
        /// <param name="spriteBatch">What is used to draw.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, drawBox, Color.White);
            spriteBatch.DrawString(font, text, new Vector2((drawBox.Width - text.Length * 15) / 2 + drawBox.X,
                (drawBox.Height - 25) / 2 + drawBox.Y), Color.White);
            if (isSelected)
            {
                spriteBatch.Draw(selcted, drawBox, Color.White);
            }
        }
    }
}
