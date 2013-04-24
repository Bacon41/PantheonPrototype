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
    public class SpriteLoader
    {
        /// <summary>
        /// Defines the necessary components for a range of frames for animation.
        /// </summary>
        public struct FrameRange
        {
            public int Begin;
            public int End;
            public bool Looping;
            public bool Sweeping;
        }

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

        /// <summary>
        /// The animation states of the sprite. Note that the key is a string
        /// representing the state name.
        /// </summary>
        public Dictionary<string, FrameRange> AnimationStates;

        /// END: XML Parsing Section

        /// <summary>
        /// Holds the image for the sprite.
        /// </summary>
        private Texture2D image;

        /// <summary>
        /// Loads the resources whose paths were specified in the XML.
        /// </summary>
        /// <param name="contentManager"></param>
        public void Load(ContentManager contentManager)
        {
            image = contentManager.Load<Texture2D>(ImagePath);
        }

        public Texture2D getImage()
        {
            return image;
        }
    }
}
