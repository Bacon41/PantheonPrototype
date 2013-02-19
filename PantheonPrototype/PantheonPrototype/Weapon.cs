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

            shootABullet(gameReference, holder);
        }

        /// <summary>
        /// Shoots a bullet.
        /// </summary>
        /// <param name="gameReference">A reference to the entire game thiny.</param>
        /// <param name="holder">A reference to the holder character.</param>
        private void shootABullet(Pantheon gameReference, CharacterEntity holder)
        {
            Vector2 cursorLocation = gameReference.controlManager.actions.CursorPosition;
            cursorLocation.X += holder.Location.X - gameReference.GraphicsDevice.Viewport.Width / 2; // + offset.X // Whenever we figure out how to do this...
            cursorLocation.Y += holder.Location.Y - gameReference.GraphicsDevice.Viewport.Height / 2; // + offset.Y // Whenever we figure out how to do this...

            float angle = (float)Math.Atan2(cursorLocation.Y - holder.Location.Y, cursorLocation.X - holder.Location.X);
            double randomDeviation = new Random().NextDouble();
            float randomAngle = (float)(angle + (randomDeviation * .1) - .05);
            // Max deviaion of 1 * .01
            // -.05 to center the deviation around the laser

            //Vector2 velocity = new Vector2(25 * (float)Math.Cos(randomAngle), 25 * (float)Math.Sin(randomAngle));
            //Bullet bullet = new Bullet(holder.Location, velocity);
            Bullet bullet = new Bullet(holder.Location, 25, angle, gameReference);
            bullet.Load(gameReference.Content);

            gameReference.currentLevel.addList.Add("bullet_" + Bullet.NextId, bullet);
        }
    }
}
