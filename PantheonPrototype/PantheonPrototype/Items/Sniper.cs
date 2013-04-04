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
    /// An upgrade, A sniper rifle.
    /// </summary>
    class Sniper : Weapon
    {
        

        /// <summary>
        /// Initializes key values of a weapon.
        /// </summary>
        public Sniper(ContentManager Content)
            : base(Content.Load<Texture2D>("Sniper"))
        {
            //LastShot = TimeSpan.Zero;
            fireRate = 1;
            totalAmmo = 5;
            currentAmmo = totalAmmo;
            range = 2000;
            damage = 15;
            reloadDelay = TimeSpan.FromSeconds(2);
            reloading = false;
            type = Type.WEAPON;
            ItemTag = "weapon";
            Info = "This is the sniper Rifle\n" +
                   "   It has MUCH better range and damage but\n" +
                   "   it's reload time is not great.\n";
        }

    }
}
