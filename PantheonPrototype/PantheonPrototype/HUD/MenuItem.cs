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
        protected Boolean isDisabled;
        protected Vector2 textSize;
        private Color color;

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

        /// <summary>
        /// Creats a new menu item with the text "text" at the location of "percentDrawBox."
        /// percentDrawBox is given as the "percent points" of the screen.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="percentDrawBox"></param>
        /// <param name="screenCoordinates"></param>
        public MenuItem(string text, Rectangle percentDrawBox, Vector2 screenCoordinates)
        {
            this.text = text;
            
            int xCoord = (int)(percentDrawBox.X/100.0 * screenCoordinates.X);
            int yCoord = (int)(percentDrawBox.Y/100.0 * screenCoordinates.Y);
            int width = (int)(percentDrawBox.Width/100.0 * screenCoordinates.X);
            int height = (int)(percentDrawBox.Height/100.0 * screenCoordinates.Y);

            this.drawBox = new Rectangle(xCoord, yCoord, width, height);
            this.isSelected = false;
            this.isDisabled = false;
        }

        public Boolean IsSelected
        {
            set { isSelected = value; }
        }

        public Boolean IsDisabled
        {
            get { return isDisabled; }
            set { isDisabled = value; }
        }

        public void Load(Pantheon gameReference)
        {
            background = gameReference.Content.Load<Texture2D>("Button");
            selcted = gameReference.Content.Load<Texture2D>("ButtonSelect");

            font = gameReference.Content.Load<SpriteFont>("DebugFont");
        }

        /// The method for updating the actual item.
        /// </summary>
        /// <param name="gameTime">How much time has elapsed.</param>
        /// <param name="gameReference">The reference to everything.</param>
        public void Update(GameTime gameTime, Pantheon gameReference)
        {
            textSize = font.MeasureString(text);
            if (isDisabled)
            {
                color = Color.Gray;
            }
            else
            {
                color = Color.White;
            }
        }

        /// <summary>
        /// The method for drawing the item.
        /// </summary>
        /// <param name="spriteBatch">What is used to draw.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, drawBox, Color.White);

            if (isSelected && !isDisabled)
            {
                spriteBatch.Draw(selcted, drawBox, Color.White);
            }

            spriteBatch.DrawString(font, text, new Vector2((drawBox.Width - textSize.X) / 2 + drawBox.X,
                (drawBox.Height - 25) / 2 + drawBox.Y), color);
            
        }
    }
}
