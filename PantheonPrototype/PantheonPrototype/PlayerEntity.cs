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
using PantheonPrototype;

namespace PantheonPrototype
{
    /// <summary>
    /// This is the visual element of the player. It extends the Character
    /// Entity.
    /// </summary>
    class PlayerEntity : CharacterEntity
    {
        /// <summary>
        /// The constructor for the player entity class.
        /// </summary>
        public PlayerEntity(GraphicsDevice graphicsDevice): base()
        {
            TotalArmor = 100;
            CurrentArmor = 75;
            ShieldCapacity = 100;
            ShieldStrength = 100;
        }

        /// <summary>
        /// Load the player entity.
        /// </summary>
        /// <param name="contentManager">Read the parameter name... that's what it is.</param>
        public override void Load(ContentManager contentManager)
        {
            base.Load(contentManager);

            Texture2D sprite;

            //Load the image
            sprite = contentManager.Load<Texture2D>("PlayerSprite");

            //If the image has been loaded
            if (sprite != null)
            {
                //Load the sprite
                this.Sprite.loadSprite(sprite, 17, 20, 30);

                //Add all the states to the sprite
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
        /// Updates the player entity.
        /// 
        /// Unlike the name suggests... (What did you think it did?)
        /// </summary>
        /// <param name="gameTime">The time object that lets the Update object know how old it is.</param>
        /// <param name="gameReference">A reference to the entire game universe for the purpose of making the player feel small.</param>
        public override void Update(GameTime gameTime, Pantheon gameReference)
        {
            //Update the velocity and facing
            updateLocation(gameReference);

            //Update the sprite appropriately
            updateSprite();

            base.Update(gameTime, gameReference);
        }

        /// <summary>
        /// Draw the player.
        /// </summary>
        /// <param name="canvas">The sprite batch to which the player will be drawn.</param>
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
            int movementSpeed = 5;

            //Reset the velocity to nothing...
            velocity = Vector2.Zero;

            //Poll for input and update velocity accordingly
            if (gameReference.controlManager.actions.MoveForward)
            {
                velocity += new Vector2(0, 1);
            }

            if (gameReference.controlManager.actions.MoveBackward)
            {
                velocity += new Vector2(0, -1);
            }

            if (gameReference.controlManager.actions.MoveLeft)
            {
                velocity += new Vector2(-1, 0);
            }

            if (gameReference.controlManager.actions.MoveRight)
            {
                velocity += new Vector2(1, 0);
            }

            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
                velocity *= movementSpeed;
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
