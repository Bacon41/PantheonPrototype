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
        /// <summary>
        /// The representation of the item on the HUD.
        /// 
        /// For the moment, this is the same as the representation in the inventory.
        /// That could change.
        /// </summary>
        public Sprite HUDRepresentation;

        /// <summary>
        /// One of those constructor thingies. Defined so that you can initialize
        /// all the thingies in the item class
        /// </summary>
        public Item()
        {
            //Right now just an empty declaration
            HUDRepresentation = new Sprite();
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
