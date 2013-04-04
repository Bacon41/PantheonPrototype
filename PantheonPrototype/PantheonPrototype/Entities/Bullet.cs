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
        private static int nextId = 0;

        public static int NextId
        {
            get
            {
                return nextId++;
            }
            set { nextId = value; }
        }

        /// <summary>
        /// How much the bullet hurts when it makes contact.
        /// </summary>
        private int damage;

        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        /// <summary>
        /// The constructor assumes that you are generating the bullet from a given location at a given velocity.
        /// </summary>
        /// <param name="location">The initial position for the bullet.</param>
        /// <param name="velocity">The initial velocity of the bullet.</param>
        public Bullet(Vector2 location, int speed, float angle, int range, int damage, Pantheon gameReference)
            : base(location,
                new Rectangle(0,0,20,20),
                new Rectangle(1,1,18,18))
        {
            // If scoping, then use perfect accuracy.
            // otherwies use a random deviation.
            if (gameReference.controlManager.actions.Aim)
            {
                this.Velocity = new Vector2(speed * (float)Math.Cos(angle), speed * (float)Math.Sin(angle));
                sprite.Rotation = angle;
            }
            else
            {
                double randomDeviation = new Random().NextDouble();
                float randomAngle = (float)(angle + (randomDeviation * .1) - .05);
                // Max deviaion of .01 radians
                // -.05 to center the deviation around the laser

                this.Velocity = new Vector2(speed * (float)Math.Cos(randomAngle), speed * (float)Math.Sin(randomAngle));
                sprite.Rotation = randomAngle;
            }
            timeToLive = range;
            this.damage = damage;
        }

        /// <summary>
        /// Loads the bullet.
        /// </summary>
        /// <param name="contentManager">Using this content manager.</param>
        public override void Load(ContentManager contentManager)
        {
            base.Load(contentManager);

            Texture2D bulletimage = contentManager.Load<Texture2D>("BulletSprite");

            if (bulletimage != null)
            {
                // Need to make it just one image, not many frames.
                this.sprite.loadSprite(bulletimage, 2, 4, 0);
                this.sprite.addState("Forward", 7, 7, false, false);

                //setDirection();
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
        /// Sets the direction of the sprite based on the current velocity.
        /// </summary>
        private void setDirection()
        {
            Direction facing = HamburgerHelper.reduceAngle(velocity);

            //Find the angle range of the bullet and set the direction
            if (facing == Direction.Right)
            {
                this.CurrentState = "Right";
            }
            else if (facing == Direction.forwardRight)
            {
                this.CurrentState = "Forward Right";
            }
            else if (facing == Direction.forward)
            {
                this.CurrentState = "Forward";
            }
            else if (facing == Direction.forwardLeft)
            {
                this.CurrentState = "Forward Left";
            }
            else if (facing == Direction.Left)
            {
                this.CurrentState = "Left";
            }
            else if (facing == Direction.backLeft)
            {
                this.CurrentState = "Back Left";
            }
            else if (facing == Direction.back)
            {
                this.CurrentState = "Back";
            }
            else
            {
                this.CurrentState = "Back Right";
            }
        }

        /// <summary>
        /// Draws the bullet... yes it does.
        /// </summary>
        /// <param name="canvas">The thingy onto which the bullet is drawn.</param>
        public override void Draw(SpriteBatch canvas)
        {
            base.Draw(canvas);
        }
    }
}
