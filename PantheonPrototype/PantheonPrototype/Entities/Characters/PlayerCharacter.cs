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
    /// This is the visual element of the player. It extends the Character
    /// Entity.
    /// </summary>
    class PlayerCharacter : CharacterEntity
    {
        /// <summary>
        /// Class variables.
        /// </summary>
        protected Vector2 cursorLocation;
        protected Vector2 totalOffset;
        protected Vector2 offset;
        protected Texture2D laserTexture;
        protected Texture2D laserDot;
        protected bool drawLasar;
        protected int currentArmedItem;

        public Vector2 CursorLocation
        {
            get { return cursorLocation; }
        }

        public struct inventory
        {
            static public List<Item> unequipped;
            static public List<Item> equipped;
        }

        public Dictionary<string, Item> Equippeditems
        {
            get { return EquippedItems; }
            set { EquippedItems = value; }
        }

        /// <summary>
        /// Determines which equipped weapon is currently armed. Can be 0 or 1;
        /// </summary>
        public int CurrentArmedItem
        {
            get { return currentArmedItem; }
            set { currentArmedItem = value; }
        }

        /// <summary>
        /// The constructor for the player entity class.
        /// </summary>
        public PlayerCharacter(Pantheon gameReference):
            base(
                Vector2.Zero,
                new Rectangle(0,0,200,159),
                new Rectangle(85,110,30,30))
        {
            initializeInventory();

            TotalArmor = 100;
            CurrentArmor = 100;
            //TotalShield = 300;
            //CurrentShield = 300;
            cursorLocation = Vector2.Zero;
            totalOffset = Vector2.Zero;
            offset = Vector2.Zero;
            laserTexture = new Texture2D(gameReference.GraphicsDevice, 1, 1);

            EquippedItems.Add("weapon", new Weapon(gameReference.Content));
            inventory.equipped.RemoveAt(0);
            inventory.equipped.Insert(0, EquippedItems["weapon"]);
            inventory.unequipped.RemoveAt(0);
            inventory.unequipped.Insert(0, new Weapon(gameReference.Content));
            EquippedItems.Add("shield", new Shield(gameReference.Content));
            inventory.equipped.RemoveAt(6);
            inventory.equipped.Insert(6, EquippedItems["shield"]);

            ArmedItem = EquippedItems["weapon"];
            drawLasar = true;

            characteristics.Add("Player");
        }


        void initializeInventory()
        {
            inventory.unequipped = new List<Item>(24);
            for (int i = 0; i < 24; i++)
            {
                inventory.unequipped.Insert(i, new Item());
            }
            inventory.equipped = new List<Item>(7);
            for (int i = 0; i < 7; i++)
            {
                inventory.equipped.Insert(i, new Item());
            }
            CurrentArmedItem = 0;
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

            //Load the laser Dot
            laserDot = contentManager.Load<Texture2D>("laserDot");

            //If the image has been loaded
            if (sprite != null)
            {
                //Load the sprite
                this.Sprite.loadSprite(sprite, 4, 4, 30);

                //Add all the states to the sprite
                this.Sprite.addState("Attack Right", 0, 3, false, true);
                this.Sprite.addState("Attack Back", 12, 15, false, true);
                this.Sprite.addState("Attack Back Right", 12, 15, false, true);
                this.Sprite.addState("Attack Back Left", 12, 15, false, true);
                this.Sprite.addState("Attack Forward", 8, 11, false, true);
                this.Sprite.addState("Attack Forward Right", 8, 11, false, true);
                this.Sprite.addState("Attack Forward Left", 8, 11, false, true);
                this.Sprite.addState("Attack Left", 4, 7, false, true);
                this.Sprite.addState("Die Right", 0, 3, false, true);
                this.Sprite.addState("Die Back", 12, 15, false, true);
                this.Sprite.addState("Die Back Right", 12, 15, false, true);
                this.Sprite.addState("Die Back Left", 12, 15, false, true);
                this.Sprite.addState("Die Forward", 8, 11, false, true);
                this.Sprite.addState("Die Forward Right", 8, 11, false, true);
                this.Sprite.addState("Die Forward Left", 8, 11, false, true);
                this.Sprite.addState("Die Left", 4, 7, false, true);
                this.Sprite.addState("Idle Right", 0, 0, false, false);
                this.Sprite.addState("Idle Back", 12, 12, false, false);
                this.Sprite.addState("Idle Back Right", 12, 12, false, false);
                this.Sprite.addState("Idle Back Left", 12, 12, false, false);
                this.Sprite.addState("Idle Forward", 8, 8, false, false);
                this.Sprite.addState("Idle Forward Right", 8, 8, false, false);
                this.Sprite.addState("Idle Forward Left", 8, 8, false, false);
                this.Sprite.addState("Idle Left", 4, 4, false, false);
                this.Sprite.addState("Move Right", 0, 3, true, true);
                this.Sprite.addState("Move Back", 12, 15, true, true);
                this.Sprite.addState("Move Back Right", 12, 15, true, true);
                this.Sprite.addState("Move Back Left", 12, 15, true, true);
                this.Sprite.addState("Move Forward", 8, 11, true, true);
                this.Sprite.addState("Move Forward Right", 8, 11, true, true);
                this.Sprite.addState("Move Forward Left", 8, 11, true, true);
                this.Sprite.addState("Move Left", 4, 7, true, true);
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
            updateLaser(gameReference, Vector2.Zero);
            updateScope(gameReference);
            updateEquipped(gameReference, gameTime);

            if (gameReference.controlManager.actions.beingDamaged == true)
            {
                Damage(10);
                gameReference.controlManager.actions.beingDamaged = false;
            }
            if (currentArmor <= 0)
            {
                currentState = "Die";
                gameReference.controlManager.disableControls(false);
            }
            if (!gameReference.controlManager.actions.isControlEnabled)
            {
                drawLasar = false;
            }
            else
            {
                drawLasar = true;
            }

            // swap equipped weapons code goes here
            if (gameReference.controlManager.actions.SwitchWeapon)
            {
                if (currentArmedItem == 0)
                {
                    currentArmedItem = 1;
                }
                else
                {
                    currentArmedItem = 0;
                }

                ArmedItem = PlayerCharacter.inventory.equipped.ElementAt(currentArmedItem);
            }

            //Update the sprite appropriately
            updateSprite();

            //Equippeditems.

            base.Update(gameTime, gameReference);
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
        /// Updates where the camera should be looking while the player is scoping.
        /// </summary>
        /// <param name="gameReference">The key to accessing the camera.</param>
        private void updateScope(Pantheon gameReference)
        {
            if (gameReference.controlManager.actions.Aim)
            {
                int offsetNum = 15;
                gameReference.controlManager.disableMotion();
                if (offset.Length() == 0)
                {
                    switch (facing)
                    {
                        case Direction.forward:
                            offset.Y = offsetNum;
                            break;
                        case Direction.forwardLeft:
                            offset.X = (int)(-offsetNum / Math.Sqrt(2));
                            offset.Y = (int)(offsetNum / Math.Sqrt(2));
                            break;
                        case Direction.Left:
                            offset.X = -offsetNum;
                            break;
                        case Direction.backLeft:
                            offset.X = (int)(-offsetNum / Math.Sqrt(2));
                            offset.Y = (int)(-offsetNum / Math.Sqrt(2));
                            break;
                        case Direction.back:
                            offset.Y = -offsetNum;
                            break;
                        case Direction.backRight:
                            offset.X = (int)(offsetNum / Math.Sqrt(2));
                            offset.Y = (int)(-offsetNum / Math.Sqrt(2));
                            break;
                        case Direction.Right:
                            offset.X = offsetNum;
                            break;
                        case Direction.forwardRight:
                            offset.X = (int)(offsetNum / Math.Sqrt(2));
                            offset.Y = (int)(offsetNum / Math.Sqrt(2));
                            break;
                        default:
                            offset = Vector2.Zero;
                            break;
                    }
                }
                if (totalOffset.Length() < 300)
                {
                    gameReference.GetCamera().Pos += offset;
                    totalOffset += offset;
                }
                updateLaser(gameReference, totalOffset);
            }
            else
            {
                gameReference.controlManager.enableMotion();
                totalOffset = Vector2.Zero;
                offset = Vector2.Zero;
            }
        }

        /// <summary>
        /// Puts the laser in the right spot on screen.
        /// </summary>
        /// <param name="gameReference">Object to access the control manager.</param>
        private void updateLaser(Pantheon gameReference, Vector2 offset)
        {
            cursorLocation = gameReference.controlManager.actions.CursorPosition;
            cursorLocation.X += Location.X - gameReference.GraphicsDevice.Viewport.Width / 2 + offset.X;
            cursorLocation.Y += Location.Y - gameReference.GraphicsDevice.Viewport.Height / 2 + offset.Y;
            angleFacing = (float)Math.Atan2(cursorLocation.Y - Location.Y, cursorLocation.X - Location.X);

            //Modify the direction in which the character faces
            if (gameReference.controlManager.actions.isControlEnabled)
            {
                facing = HamburgerHelper.reduceAngle(cursorLocation - Location);
            }
        }

        /// <summary>
        /// Updates the equipped items according to user input.
        /// </summary>
        /// <param name="gameReference">A reference to the game so that the items can do their jobs.</param>
        private void updateEquipped(Pantheon gameReference, GameTime gameTime)
        {
            //Fire all (one of) the weapons!
            if (gameReference.controlManager.actions.Attack)
            {
                if (this.ArmedItem.type == (Item.Type.WEAPON))
                {
                    this.ArmedItem.activate(gameReference, this);
                }
            }

            //reload button
            if (this.ArmedItem.type == (Item.Type.WEAPON))
            {
                if (gameReference.controlManager.actions.Reload && !((Weapon)this.ArmedItem).Reloading)
                {

                    ((Weapon)this.ArmedItem).Reload(gameTime);

                }
            }
            //Ammo and shield cheat
            if (gameReference.controlManager.actions.MoveBackward
                && gameReference.controlManager.actions.MoveForward
                && gameReference.controlManager.actions.MoveLeft
                && gameReference.controlManager.actions.MoveRight)
            {
                if (this.Equippeditems.ContainsKey("weapon"))
                {
                    ((Weapon)this.EquippedItems["weapon"]).CurrentAmmo = ((Weapon)this.EquippedItems["weapon"]).TotalAmmo;
                }
                if (this.Equippeditems.ContainsKey("shield"))
                {
                    ((Shield)this.EquippedItems["shield"]).CurrentShield = ((Shield)this.EquippedItems["shield"]).TotalShield;
                }
            }

            //Activate the shield
            if (gameReference.controlManager.actions.Shield == true)
            {
                if (this.Equippeditems.ContainsKey("shield"))
                {
                    EquippedItems["shield"].activate(gameReference, this);
                }
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

        /// <summary>
        /// Draw the player.
        /// </summary>
        /// <param name="canvas">The sprite batch to which the player will be drawn.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (drawLasar)
            {
                HamburgerHelper.DrawLine(spriteBatch, laserTexture, 1.25f, Color.Red, Location, this.cursorLocation);
            }
            base.Draw(spriteBatch);

            Vector2 laserDotCoords = new Vector2((int)(cursorLocation.X - laserDot.Width/2), (int)(cursorLocation.Y - laserDot.Height/2));
            spriteBatch.Draw(laserDot, new Rectangle((int)laserDotCoords.X, (int)laserDotCoords.Y, laserDot.Width, laserDot.Height),
                null, Color.White, 0, Vector2.Zero, SpriteEffects.None, .1f);
        }
    }
}
