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
    /// Contains all characteristics common to all types of characters.
    /// </summary>
    public class CharacterLoader
    {
        /// BEGIN: XML Parsing Section

        /// <summary>
        /// The total armor of the character.
        /// </summary>
        public int TotalArmor;

        /// <summary>
        /// The starting state of the character.
        /// </summary>
        public string InitialState;

        /// <summary>
        /// The initial facing direction of the character.
        /// </summary>
        public string InitialDirection;

        /// <summary>
        /// A list of strings which indicate items that should be in the
        /// character's inventory.
        /// </summary>
        public List<string> Inventory;

        /// <summary>
        /// A list of characteristics that define what type of behavior the
        /// character responds to.
        /// </summary>
        public List<string> Characteristics;

        /// <summary>
        /// The sprite used for the character.
        /// </summary>
        public SpriteLoader Sprite;

        /// END: XML Parsing Section
    }
}
