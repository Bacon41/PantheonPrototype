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
    /// This is the class that will contain all of the information
    /// for drawing and maintaining a component of the HUD.
    /// </summary>
    class HUDItem
    {
        protected Texture2D image;
        protected Rectangle coordinates;
        protected Color opacity;
        protected int defaultWidth;

        public HUDItem(ContentManager Content, String img, int x, int y)
        {
            image = Content.Load<Texture2D>(img);
            defaultWidth = image.Width;
            coordinates = new Rectangle(x, y, image.Width, image.Height);
            opacity = new Color(256,256,256,256);
        }

        /// <summary>
        /// The current image of this HUD item
        /// </summary>
        public Texture2D Image
        {
            get { return image; }
        }

        /// <summary>
        /// The Coordinates of this HUD item
        /// </summary>
        public Rectangle Coordinates
        {
            get { return coordinates; }
            set { coordinates = value; }
        }

        /// <summary>
        /// The current opacity of this HUD item
        /// </summary>
        public Color Opacity
        {
            get { return opacity; }
        }
        
        /// <summary>
        /// Sets the opacity using an integer so you don't have to create a whole new Color().
        /// Takes an integer from 0 to 255
        /// </summary>
        /// <param name="n"></param>
        public void SetOpacity(int n) 
        {
            if (n < 0)
            {
                opacity.A = 0;
                opacity.B = 0;
                opacity.G = 0;
                opacity.R = 0;
            }
            else if (n > 255)
            {
                opacity.A = 255;
                opacity.B = 255;
                opacity.G = 255;
                opacity.R = 255;
            }
            else
            {
                opacity.A = (byte)n;
                opacity.B = (byte)n;
                opacity.G = (byte)n;
                opacity.R = (byte)n;
            }
        }

        /// <summary>
        /// Returns the original (default) width of the image
        /// </summary>
        public int DefaultWidth
        {
            get { return defaultWidth; }
        }

        /// <summary>
        /// The method to update the component.
        /// </summary>
        /// <param name="gameTime">Contains the time since [wheneverYouWant].</param>
        public void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// The method to draw the component.
        /// </summary>
        /// <param name="spriteBatch">A shared SpriteBatch for the HUD.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Coordinates, Opacity);
        }
    }
}
