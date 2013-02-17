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
        /// The super ambiguous function designed to be super ambiguous.
        /// 
        /// Basically, call this function for some random object to do some
        /// random thing.
        /// 
        /// See? Super ambiguous.
        /// </summary>
        public virtual void activate()
        {
        }
    }
}
