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
        /// <summary>
        /// The rate of fire of the gun as the number of shots per second.
        /// </summary>
        protected float fireRate;

        public float FireRate
        {
            get { return fireRate; }
            set { fireRate = value; }
        }

        /// <summary>
        /// The current amount of ammunition available to the player.
        /// </summary>
        protected int currentAmmo;

        public int CurrentAmmo
        {
            get { return currentAmmo; }
            set { currentAmmo = value; }
        }

        /// <summary>
        /// The total amount of ammunition available to the player.
        /// </summary>
        protected int totalAmmo;

        public int TotalAmmo
        {
            get { return totalAmmo; }
            set { totalAmmo = value; }
        }

        protected int range;
        protected int damage;
        protected TimeSpan reloadDelay;
        protected bool reloading;

        public bool Reloading
        {
            get { return reloading; }
        }

        /// <summary>
        /// The amount of time since the last shot was fired
        /// </summary>
        private TimeSpan lastShot;

        /// <summary>
        /// Initializes key values of a weapon.
        /// </summary>
        public Weapon(ContentManager Content)
            : base(Content.Load<Texture2D>("Rifle"))
        {
            lastShot = TimeSpan.Zero;
            fireRate = 5;
            totalAmmo = 10;
            currentAmmo = totalAmmo;
            range = 500;
            damage = 5;
            reloadDelay = TimeSpan.FromSeconds(2);
            reloading = false;
            Info = "This is the basic weapon\n" +
                   "   It has so/so range and\n" +
                   "   reload time. Also, watch\n" +
                   "   out for Butterflies carring\n" +
                   "   this weapon!";
        }

        /// <summary>
        /// Shoot the weapon. That's what this game is really about, right?
        /// 
        /// Also, we need to rethink the way that shooting works right now. Eventually, I think that the
        /// Character Entity class needs an aiming point. This makes it so both Enemies and Players can use
        /// weapons.
        /// </summary>
        /// <param name="gameReference">A reference so we can see where everything is.</param>
        /// <param name="holder">A reference to the character holding the weapon.</param>
        public override void activate(Pantheon gameReference, CharacterEntity holder)
        {
            base.activate(gameReference, holder);

            //Shoot when the cool down has lasted long enough.
            if(lastShot.CompareTo(TimeSpan.Zero) <= 0 && currentAmmo > 0)
            {
                shootABullet(gameReference, holder);
                lastShot = TimeSpan.FromMilliseconds(1000/ fireRate);
            }
        }

        /// <summary>
        /// Updates the weapon, taking care for cooldown and other time sensitive functions.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        /// <param name="gameReference">A reference to the entire game.</param>
        public override void Update(GameTime gameTime, Pantheon gameReference)
        {
            if (lastShot.CompareTo(TimeSpan.Zero) > 0)
            {
                lastShot = lastShot.Subtract(gameTime.ElapsedGameTime);
            }
            if (reloading)
            {
                Reload(gameTime);
            }
        }

        /// <summary>
        /// Shoots a bullet.
        /// </summary>
        /// <param name="gameReference">A reference to the entire game thiny.</param>
        /// <param name="holder">A reference to the holder character.</param>
        private void shootABullet(Pantheon gameReference, CharacterEntity holder)
        {
            Bullet bullet = new Bullet(holder.Location, 25, holder.AngleFacing, range, damage, gameReference);
            bullet.Load(gameReference.Content);

            gameReference.currentLevel.addList.Add("bullet_" + Bullet.NextId, bullet);

            //Drain a bullet from the current ammo
            currentAmmo--;
        }

        /// <summary>
        /// The method to start the reloading procudure.
        /// </summary>
        /// <param name="gameTime">Time since last call.</param>
        public void Reload(GameTime gameTime)
        {
            reloading = true;
            reloadDelay = reloadDelay.Subtract(gameTime.ElapsedGameTime);
            if (reloadDelay.CompareTo(TimeSpan.Zero) <= 0)
            {
                reloadDelay = TimeSpan.FromSeconds(2);
                currentAmmo = totalAmmo;
                reloading = false;
            }
        }

        public TimeSpan ReloadDelay
        {
            get { return reloadDelay; }
        }

        public float PercentToEndReload()
        {
            if (reloadDelay.Seconds == 2)
            {
                return 0;
            }
            else
            {
                return (float)((reloadDelay.Seconds * 1000 + reloadDelay.Milliseconds) / (2000.0));
            }
        }
    }
}
