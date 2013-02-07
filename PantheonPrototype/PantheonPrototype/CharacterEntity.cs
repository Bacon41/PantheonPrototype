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
    /// The facing directions.
    /// </summary>
    public enum Direction
    {
        forward, forwardLeft, Left, backLeft, back, backRight, Right, forwardRight
    }

    /// <summary>
    /// The character entity provides the properties and methods
    /// used for a character in the game. It inherits the basic
    /// traits of an Entity and adds things such as velocity and healing.
    /// </summary>
    class CharacterEntity : Entity
    {
        /// <summary>
        /// The total armor of the character.
        /// </summary>
        protected int totalArmor;

        public int TotalArmor
        {
            get { return totalArmor; }
            set { totalArmor = value; }
        }

        /// <summary>
        /// The current armor of the character.
        /// </summary>
        protected int currentArmor;

        public int CurrentArmor
        {
            get { return currentArmor; }
            set { currentArmor = value; }
        }

        /// <summary>
        /// The total amount of shield energy available when fully charged.
        /// </summary>
        protected int shieldCapacity;

        public int ShieldCapacity
        {
            get { return shieldCapacity; }
            set { shieldCapacity = value; }
        }

        /// <summary>
        /// The current shield strength for the character.
        /// </summary>
        protected int shieldStrength;

        public int ShieldStrength
        {
            get { return shieldStrength; }
            set { shieldStrength = value; }
        }

        /// <summary>
        /// Flag indicating if the shield is currently on.
        /// </summary>
        protected bool shieldOn;

        public bool ShieldOn
        {
            get { return shieldOn; }
            set { shieldOn = value; }
        }

        /// <summary>
        /// The velocity of the character.
        /// </summary>
        protected Vector2 velocity;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        /// <summary>
        /// The direction in which the character is facing.
        /// </summary>
        protected Direction facing;

        public Direction Facing
        {
            get { return facing; }
            set { facing = value; }
        }

        /// <summary>
        /// Loads any assets the entity may or may not need.
        /// </summary>
        /// <param name="contentManager">The intialized content manager that will be used to load the asset information.</param>
        /// <exception cref="ContentLoadException">Thrown when the content manager is unable to load the player sprite.</exception>
        public override void Load(ContentManager contentManager)
        {
            Texture2D sprite;

            base.Load(contentManager);

            sprite = contentManager.Load<Texture2D>("PlayerSprite");

            if (sprite != null)
            {
                this.Sprite.loadSprite(sprite, 17, 20, 30);
                this.Sprite.addState("Attack Right", 0, 12);
                this.Sprite.addState("Attack Back", 13, 25);
                this.Sprite.addState("Attack Back Right", 26, 38);
                this.Sprite.addState("Attack Back Left", 39, 51);
                this.Sprite.addState("Attack Forward", 52, 64);
                this.Sprite.addState("Attack Forward Right", 65, 77);
                this.Sprite.addState("Attack Forward Left", 78, 90);
                this.Sprite.addState("Attack Left", 91, 103);
                this.Sprite.addState("Die Right", 104, 113);
                this.Sprite.addState("Die Back", 114, 123);
                this.Sprite.addState("Die Back Right", 124, 133);
                this.Sprite.addState("Die Back Left", 134, 143);
                this.Sprite.addState("Die Forward", 144, 153);
                this.Sprite.addState("Die Forward Right", 154, 163);
                this.Sprite.addState("Die Forward Left", 164, 173);
                this.Sprite.addState("Die Left", 174, 183);
                this.Sprite.addState("Idle Right", 184, 186);
                this.Sprite.addState("Idle Back", 187, 189);
                this.Sprite.addState("Idle Back Right", 190, 192);
                this.Sprite.addState("Idle Back Left", 193, 195);
                this.Sprite.addState("Idle Forward", 196, 198);
                this.Sprite.addState("Idle Forward Right", 199, 201);
                this.Sprite.addState("Idle Forward Left", 202, 204);
                this.Sprite.addState("Idle Left", 205, 207);
                this.Sprite.addState("Move Right", 208, 223);
                this.Sprite.addState("Move Back", 224, 239);
                this.Sprite.addState("Move Back Right", 240, 255);
                this.Sprite.addState("Move Back Left", 256, 271);
                this.Sprite.addState("Move Forward", 272, 287);
                this.Sprite.addState("Move Forward Right", 288, 303);
                this.Sprite.addState("Move Forward Left", 304, 319);
                this.Sprite.addState("Move Left", 320, 335);
            }
            else
            {
                throw new ContentLoadException();
            }

            velocity = Vector2.Zero;
        }

        /// <summary>
        /// Update the character class.
        /// </summary>
        /// <param name="gameTime">The game time object for letting you know how old you've gotten since starting the game.</param>
        /// <param name="gameReference">A deeper game reference to the game reference of doom.</param>
        public override void Update(GameTime gameTime, Pantheon gameReference)
        {
            base.Update(gameTime, gameReference);

            //Update the character's location
            updateLocation(gameReference);

            //If the shield is on, drain it
            if (shieldOn)
            {
                shieldStrength--;
            }
            else //Otherwise, charge it
            {
                shieldStrength++;
            }

            //Update the sprite appropriately
            updateSprite();
        }

        /// <summary>
        /// Draw the character class... and override the default behavior...
        /// take over the entity. Let none of it survive... except all of it.
        /// </summary>
        /// <param name="canvas">An initialized SpriteBatch.</param>
        public override void Draw(SpriteBatch canvas)
        {
            base.Draw(canvas);
        }

        /// <summary>
        /// Updates the location of the character. This is a subfunction of the update function
        /// meant to take some of the logic and stick it istor a logical subdivision. All
        /// game logic concerning movement and the player's character should go here.
        /// 
        /// Note: Forces the entity state into Idle if not moving. This should be overwritten
        /// by other functions which can update the state to Attack or some other relevant
        /// state.
        /// </summary>
        /// <param name="gameReference">A reference to the entire game for purposes of finding the holy grail.
        /// Also, provides access to the ControlManager class.</param>
        private void updateLocation(Pantheon gameReference)
        {
            ///<summary>
            ///TEMPORARY: This should be replaced by an entity feature... probably.
            ///</summary>
            int movementSpeed = 10;

            //Reset the velocity to nothing...
            velocity = Vector2.Zero;

            //Poll for input and update velocity accordingly
            if (gameReference.controlManager.actions.MoveForward)
            {
                velocity += new Vector2(0, movementSpeed);
            }

            if (gameReference.controlManager.actions.MoveBackward)
            {
                velocity += new Vector2(0, -movementSpeed);
            }

            if (gameReference.controlManager.actions.MoveLeft)
            {
                velocity += new Vector2(-movementSpeed, 0);
            }

            if (gameReference.controlManager.actions.MoveRight)
            {
                velocity += new Vector2(movementSpeed, 0);
            }

            //Modify the direction in which the character faces
            if (velocity.X > 0)
            {
                if (velocity.Y > 0)
                { facing = Direction.forwardRight; }
                else if (velocity.Y < 0)
                { facing = Direction.backRight; }
                else
                { facing = Direction.Right; }
            }
            else if (velocity.X < 0)
            {
                if (velocity.Y > 0)
                { facing = Direction.forwardLeft; }
                else if (velocity.Y < 0)
                { facing = Direction.backLeft; }
                else
                { facing = Direction.Left; }
            }
            else
            {
                if (velocity.Y > 0)
                { facing = Direction.forward; }
                else if (velocity.Y < 0)
                { facing = Direction.back; }
            }

            //Update the entity state to show movement
            if (velocity != Vector2.Zero)
            {
                currentState = "Move";
            }
            else
            {
                currentState = "Idle";
            }

            //Move the player by velocity
            location.X += (int)velocity.X;
            location.Y += (int)velocity.Y;
        }

        /// <summary>
        /// Updates the sprite into the correct state.
        /// </summary>
        private void updateSprite()
        {
            switch (facing)
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
    }
}
