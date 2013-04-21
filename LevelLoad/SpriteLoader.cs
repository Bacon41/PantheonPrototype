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

namespace LevelLoad
{
    /// <summary>
    /// Loads a sprite from an XML file.
    /// </summary>
    class SpriteLoader
    {
        /// BEGIN: XML Parsing Section
        
        /// <summary>
        /// Path to the sprite image.
        /// </summary>
        public string ImagePath;

        /// <summary>
        /// The number of rows in the image.
        /// </summary>
        public int Rows;

        /// <summary>
        /// The number of columns in the image.
        /// </summary>
        public int Columns;

        /// END: XML Parsing Section
    }
}
