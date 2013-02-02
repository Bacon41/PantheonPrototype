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
    /// This is the wrapper class for all information that will go
    /// into the HUD. It will have HUDItems in it, thad can be added
    /// at will and drawn all at once.
    /// </summary>
    class HUD
    {
        /// <summary>
        /// All of the different HUDItems that will need to be drawn.
        /// More or less can be registered.
        /// </summary>
        protected List<HUDItem> hudItems;

        public HUD()
        {
        }

        /// <summary>
        /// This is the method to add more items to the HUD. It will probably have
        /// parameters later, but I'm not sure what yet.
        /// </summary>
        public void AddItem()
        {
        }

        /// <summary>
        /// The method to update all of the HUDItems' information.
        /// </summary>
        /// <param name="gameTime">The object that holds all the time information.</param>
        public void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// The method to draw all of the different HUDItems.
        /// </summary>
        /// <param name="spriteBatch">The object that we will use to draw to the screen.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
