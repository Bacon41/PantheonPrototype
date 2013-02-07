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

        public HUDItem(ContentManager Content, String img, int x, int y)
        {
            image = Content.Load<Texture2D>(img);
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
        /// Sets the opacity using an integer so you don't have to create a whole new Color()
        /// </summary>
        /// <param name="n"></param>
        public void SetOpacity(int n) 
        {
            opacity.A = (byte)n;
            opacity.B = (byte)n;
            opacity.G = (byte)n;
            opacity.R = (byte)n;
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
        }
    }
}
