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
    /// Contains all characteristics specific to the player.
    /// </summary>
    public class PlayerLoader : CharacterLoader
    {
        /// BEGIN: XML Parsing Section

        /// <summary>
        /// Path to the laser texture.
        /// </summary>
        public string LaserTexturePath;

        /// END: XML Parsing Section

        /// <summary>
        /// Holds the loaded laser texture.
        /// </summary>
        private Texture2D laserTexture;

        /// <summary>
        /// Loads paths in the character loader into appropriate members.
        /// </summary>
        /// <param name="contentManager">Content Manager</param>
        public void Load(ContentManager contentManager)
        {
            laserTexture = contentManager.Load<Texture2D>(LaserTexturePath);
        }

        /// <summary>
        /// Gets the laser texture for the player
        /// </summary>
        /// <returns></returns>
        public Texture2D GetLaserTexture()
        {
            return laserTexture;
        }
    }
}
