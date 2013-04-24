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
    class NPCCharacter : CharacterEntity
    {
        /// <summary>
        /// The time until the NPC needs to change directions for their random movement.
        /// </summary>
        protected TimeSpan changeDirection;

        /// <summary>
        /// This is the area where interactions will occur (either conversations or attacking).
        /// </summary>
        protected Rectangle comfortZone;

        public Rectangle ComfortZone
        {
            get { return comfortZone; }
            set { comfortZone = value; }
        }

        /// <summary>
        /// This tells the NPC if it is randomly moving around or not.
        /// </summary>
        protected bool isRoaming;

        public bool IsRoaming
        {
            get { return isRoaming; }
            set { isRoaming = value; }
        }

        /// <summary>
        /// This is the speed in which the character will walk.
        /// </summary>
        protected int speed;

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public NPCCharacter(Vector2 location, Rectangle drawBox, Rectangle boundingBox)
            : base(location, drawBox, boundingBox)
        {
            isRoaming = true;
        }

        public override void Update(GameTime gameTime, Pantheon gameReference)
        {
            base.Update(gameTime, gameReference);

            if (!isRoaming)
            {
                switch (Facing)
                {
                    case Direction.forward:
                        sprite.changeState(currentState + " Forward");
                        break;
                    case Direction.forwardLeft:
                        sprite.changeState(currentState + " Forward Left");
                         break;
                    case Direction.Left:
                        sprite.changeState(currentState + " Left");
                        break;
                    case Direction.backLeft:
                        sprite.changeState(currentState + " Back Left");
                        break;
                    case Direction.back:
                        sprite.changeState(currentState + " Back");
                        break;
                    case Direction.backRight:
                        sprite.changeState(currentState + " Back Right");
                        break;
                    case Direction.Right:
                        sprite.changeState(currentState + " Right");
                        break;
                    case Direction.forwardRight:
                        sprite.changeState(currentState + " Forward Right");
                        break;
                    default:
                        sprite.changeState(currentState + " Forward");
                        break;
                }
            }

            changeDirection = changeDirection.Subtract(gameTime.ElapsedGameTime);
            if (changeDirection.CompareTo(TimeSpan.Zero) <= 0)
            {
                if (isRoaming)
                {
                    switchDirection(gameReference);
                }
                changeDirection = TimeSpan.FromSeconds(gameReference.rand.Next(3) + 1);
            }

            ComfortZone = new Rectangle(BoundingBox.X - ComfortZone.Width / 2, BoundingBox.Y - ComfortZone.Height / 2,
                ComfortZone.Width, ComfortZone.Height);
        }

        /// <summary>
        /// Randomly switch directions.
        /// </summary>
        private void switchDirection(Pantheon gameReference)
        {
            int dir = gameReference.rand.Next(8);
            switch (dir)
            {
                case 0:
                    Facing = Direction.forward;
                    angleFacing = (float)Math.PI / 2;
                    velocity = new Vector2(0, this.speed);
                    sprite.changeState(currentState + " Forward");
                    break;
                case 1:
                    Facing = Direction.forwardLeft;
                    angleFacing = this.speed * (float)Math.PI / 4;
                    velocity = new Vector2(-this.speed, this.speed);
                    sprite.changeState(currentState + " Forward Left");
                    break;
                case 2:
                    Facing = Direction.Left;
                    angleFacing = (float)Math.PI;
                    velocity = new Vector2(-this.speed, 0);
                    sprite.changeState(currentState + " Left");
                    break;
                case 3:
                    Facing = Direction.backLeft;
                    angleFacing = 5 * (float)Math.PI / 4;
                    velocity = new Vector2(-this.speed, -this.speed);
                    sprite.changeState(currentState + " Back Left");
                    break;
                case 4:
                    Facing = Direction.back;
                    angleFacing = this.speed * (float)Math.PI / 2;
                    velocity = new Vector2(0, -this.speed);
                    sprite.changeState(currentState + " Back");
                    break;
                case 5:
                    Facing = Direction.backRight;
                    angleFacing = 7 * (float)Math.PI / 4;
                    velocity = new Vector2(this.speed, -this.speed);
                    sprite.changeState(currentState + " Back Right");
                    break;
                case 6:
                    Facing = Direction.Right;
                    angleFacing = 0;
                    velocity = new Vector2(this.speed, 0);
                    sprite.changeState(currentState + " Right");
                    break;
                case 7:
                    Facing = Direction.forwardRight;
                    angleFacing = (float)Math.PI / 4;
                    velocity = new Vector2(this.speed, this.speed);
                    sprite.changeState(currentState + " Forward Right");
                    break;
                default:
                    Facing = Direction.forward;
                    angleFacing = (float)Math.PI / 2;
                    velocity = new Vector2(0, this.speed);
                    sprite.changeState(currentState + " Forward");
                    break;
            }
        }

        public virtual void Interact()
        {
        }
    }
}
