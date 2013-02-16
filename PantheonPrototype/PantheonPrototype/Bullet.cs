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
    /// A bullet object meant to hurt people, porcupines, or pinapple.
    /// 
    /// Bullets inherit from projectile and add the capacity to deal damage.
    /// In this initial implementation, they will be a leaf class with its
    /// own image etc...
    /// </summary>
    class Bullet : Projectile
    {
        /// <summary>
        /// The constructor assumes that you are generating the bullet from a given location at a given velocity.
        /// </summary>
        /// <param name="location">The initial position for the bullet.</param>
        /// <param name="velocity">The initial velocity of the bullet.</param>
        public Bullet(Vector2 location, Vector2 velocity)
            : base(location,
                new Rectangle(0,0,20,20),
                new Rectangle(1,1,18,18))
        {
            this.Velocity = velocity;
        }

        /// <summary>
        /// Loads the bullet.
        /// </summary>
        /// <param name="contentManager">Using this content manager.</param>
        public override void  Load(ContentManager contentManager)
        {
            base.Load(contentManager);

            Texture2D bulletimage = contentManager.Load<Texture2D>("BulletSprite");

            if (bulletimage != null)
            {
                this.sprite.loadSprite(bulletimage, 2, 4, 0);

                this.sprite.addState("Forward Right", 0, 0);
                this.sprite.addState("Forward", 1, 1);
                this.sprite.addState("Forward Left", 2, 2);
                this.sprite.addState("Left", 3, 3);
                this.sprite.addState("Back Left", 4, 4);
                this.sprite.addState("Back", 5, 5);
                this.sprite.addState("Back Right", 6, 6);
                this.sprite.addState("Right", 7, 7);

                setDirection();
            }
        }

        /// <summary>
        /// May the bullet update evermore... or at least until it reaches the edge of the screen...
        /// </summary>
        /// <param name="gameTime">The amount of time since last calling the Update function.</param>
        /// <param name="gameReference">The reference to life the universe and everything.</param>
        public override void Update(GameTime gameTime, Pantheon gameReference)
        {
            base.Update(gameTime, gameReference);
        }

        /// <summary>
        /// Draws the bullet... yes it does.
        /// </summary>
        /// <param name="canvas">The thingy onto which the bullet is drawn.</param>
        public override void Draw(SpriteBatch canvas)
        {
            base.Draw(canvas);
        }

        /// <summary>
        /// Sets the direction of the sprite based on the current velocity.
        /// </summary>
        private void setDirection()
        {
            //Unit vector in the direction of the velocity
            Vector2 unitVelocity = velocity;

            //The angle of the unitVelocity with the positive x axis
            double angle;

            unitVelocity.Normalize();

            //Check for the case where arcsin will not work
            /*if (unitVelocity.Y == 0)
            {
                if (unitVelocity.X < 0)
                {
                    this.CurrentState = "Left";
                }
                else
                {
                    this.CurrentState = "Right";
                }
            }*/

            //Find the angle
            angle = Math.Asin(unitVelocity.Y);

            //Make sure that the direction is correct for right and left
            //(sine doesn't handle right and left)
            if (unitVelocity.X < 0)
            {
                angle += Math.PI;
            }

            //Make sure that 0 < angle < 2*pi
            while (angle < 0)
            {
                angle += 2 * Math.PI;
            }

            while (angle > 2 * Math.PI)
            {
                angle -= 2 * Math.PI;
            }

            Console.WriteLine(angle / (2 * Math.PI) * 360 + " or " + angle + " in radians");

            //Find the angle range of the bullet and set the direction
            if (Math.PI * 15 / 8 < angle || angle < Math.PI / 8)
            {
                this.CurrentState = "Right";
                Console.WriteLine("Choose between 15pi/8 (" + (Math.PI * 15 / 8) + ") and pi/8 (" + (Math.PI / 8) + ")");
            }
            else if (Math.PI / 8 < angle && angle < Math.PI * 3 / 8)
            {
                this.CurrentState = "Forward Right";
                Console.WriteLine("Choose between pi/8 (" + (Math.PI / 8) + ") and 3pi/8 (" + (Math.PI * 3 / 8) + ")");
            }
            else if (Math.PI * 3 / 8 < angle && angle < Math.PI * 5 / 8)
            {
                if (unitVelocity.Y < 0)
                {
                    this.CurrentState = "Back";
                }
                else
                {
                    this.CurrentState = "Forward";
                }
                Console.WriteLine("Choose between 3pi/8 (" + (Math.PI * 3 / 8) + ") and 5pi/8 (" + (Math.PI * 5 / 8) + ")");
            }
            else if (Math.PI * 5 / 8 < angle && angle < Math.PI * 7 / 8)
            {
                this.CurrentState = "Back Left";
                Console.WriteLine("Choose between 5pi/8 (" + (Math.PI * 5 / 8) + ") and 7pi/8 (" + (Math.PI * 7 / 8) + ")");
            }
            else if (Math.PI * 7 / 8 < angle && angle < Math.PI * 9 / 8)
            {
                this.CurrentState = "Left";
                Console.WriteLine("Choose between 7pi/8 (" + (Math.PI * 7 / 8) + ") and 9pi/8 (" + (Math.PI * 9 / 8) + ")");
            }
            else if (Math.PI * 9 / 8 < angle && angle < Math.PI * 11 / 8)
            {
                this.CurrentState = "Forward Left";
                Console.WriteLine("Choose between 9pi/8 (" + (Math.PI * 9 / 8) + ") and 11pi/8 (" + (Math.PI * 11 / 8) + ")");
            }
            else if (Math.PI * 11 / 8 < angle && angle < Math.PI * 13 / 8)
            {
                if (unitVelocity.Y < 0)
                {
                    this.CurrentState = "Back";
                }
                else
                {
                    this.CurrentState = "Forward";
                }
                Console.WriteLine("Choose between 11pi/8 (" + (Math.PI * 11 / 8) + ") and 13pi/8 (" + (Math.PI * 13 / 8) + ")");
            }
            else
            {
                this.CurrentState = "Back Right";
                Console.WriteLine("Choose between 13pi/8 (" + (Math.PI * 13 / 8) + ") and 15pi/8 (" + (Math.PI * 15 / 8) + ")");
            }
        }
    }
}
