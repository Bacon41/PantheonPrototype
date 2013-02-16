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
            int length = (int)velocity.Length();

            List<Vector2> cardinalDirections = new List<Vector2>();

            Vector2 velocityNormalized = velocity;
            velocityNormalized.Normalize();

            //The current cardinal direction to be added to the list
            Vector2 direction;

            //The distance between velocity and the direction
            Vector2 distance;

            //The minimum distance between the velocity and the cardinal direction
            //Should be initialized to greater than any possible value.
            double minDistance = 10;

            //The current distance being measured
            double currentDistance;

            //The index of the minimum distance
            int indexOfMin = 0;
            
            //Load all the cardinal directions
            direction = Vector2.UnitX;
            direction *= length;
            cardinalDirections.Add(direction);

            direction = Vector2.UnitX + Vector2.UnitY;
            direction.Normalize();
            direction *= length;
            cardinalDirections.Add(direction);

            direction = Vector2.UnitY;
            direction *= length;
            cardinalDirections.Add(direction);

            direction = -Vector2.UnitX + Vector2.UnitY;
            direction.Normalize();
            direction *= length;
            cardinalDirections.Add(direction);

            direction = -Vector2.UnitX;
            direction *= length;
            cardinalDirections.Add(direction);

            direction = -Vector2.UnitX - Vector2.UnitY;
            direction.Normalize();
            direction *= length;
            cardinalDirections.Add(direction);

            direction = -Vector2.UnitY;
            direction *= length;
            cardinalDirections.Add(direction);

            direction = Vector2.UnitX - Vector2.UnitY;
            direction.Normalize();
            direction *= length;
            cardinalDirections.Add(direction);

            //Find the minimum distance
            for(int i = 0; i < cardinalDirections.Count; i++)
            {
                distance = velocityNormalized - cardinalDirections[i];
                currentDistance = distance.Length();
                Console.WriteLine(currentDistance + " at " + i);
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    indexOfMin = i;
                }
            }

            switch (indexOfMin)
            {
                case 0:
                    this.CurrentState = "Right";
                    break;
                case 1:
                    this.CurrentState = "Forward Right";
                    break;
                case 2:
                    this.CurrentState = "Forward";
                    break;
                case 3:
                    this.CurrentState = "Forward Left";
                    break;
                case 4:
                    this.CurrentState = "Left";
                    break;
                case 5:
                    this.CurrentState = "Back Left";
                    break;
                case 6:
                    this.CurrentState = "Back";
                    break;
                case 7:
                    this.CurrentState = "Back Right";
                    break;
                default:
                    break;
            }
        }
    }
}
