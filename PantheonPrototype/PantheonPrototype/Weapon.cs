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
    ///  A basic implementation of a weapon inheriting from Item.
    ///  
    /// As more weapons are added, the specific logic of the weapon should be
    /// moved to a subclass and this should be a parent of all weapons.
    /// </summary>
    class Weapon : Item
    {
    }
}
