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
    /// An Item is an object that may be equipped and stored in an Inventory.
    /// 
    /// The Item will have an action that may be called without knowledge of
    /// the actual function of the Item. This provides encapsulation and abstraction
    /// for the inventory.
    /// </summary>
    class Item
    {
        public bool isNull;
        protected String info;
        protected String itemTag;
        public int type;
        public string soundCueName;

        /// <summary>
        /// The text that shows in the inventory when selected
        /// </summary>
        public String Info
        {
            get { return info; }
            set { info = value; }
        }

        /// <summary>
        /// This tag gets used in EquippedItems
        /// </summary>
        public String ItemTag
        {
            get { return itemTag; }
            set { itemTag = value; }
        }

        /// <summary>
        /// Defines the types of items
        /// </summary>
        public static class Type
        {
            public const int WEAPON = 0x8;
            public const int HOLDABLE = 0x4;
            public const int SHIELD = 0x2;
            public const int USEABLE = 0x1;
            public const int ALL = 0xF;
        }

        /// <summary>
        /// The representation of the item on the HUD.
        /// 
        /// For the moment, this is the same as the representation in the inventory.
        /// That could change.
        /// </summary>
        public Texture2D HUDRepresentation;

        /// <summary>
        /// One of those constructor thingies. Defined so that you can initialize
        /// all the thingies in the item class
        /// </summary>
        public Item(Texture2D HUDrep)
        {
            HUDRepresentation = HUDrep;
            isNull = false;
        }

        public Item()
        {
            HUDRepresentation = null;
            isNull = true;
            info = "";
            itemTag = "";
        }

        /// <summary>
        /// An update function to be called only on equipped items. Takes care
        /// of time sensitive functionality.
        /// </summary>
        /// <param name="gameTime">The elapsed game time for each update.</param>
        /// <param name="gameReference">A reference to the entire game.</param>
        public virtual void Update(GameTime gameTime, Pantheon gameReference)
        {
        }

        /// <summary>
        /// The super ambiguous function designed to be super ambiguous.
        /// 
        /// Basically, call this function for some random object to do some
        /// random thing.
        /// 
        /// See? Super ambiguous.
        /// </summary>
        /// <param name="gameReference">A supremely useful reference to everything, just in case you need it.</param>
        /// <param name="holder">A reference to the character holding the weapon.</param>
        public virtual void activate(Pantheon gameReference, CharacterEntity holder)
        {
        }
    }
}
