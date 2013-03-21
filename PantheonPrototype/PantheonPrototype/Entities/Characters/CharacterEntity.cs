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
    /// The character entity provides the properties and methods
    /// used for a character in the game. It inherits the basic
    /// traits of an Entity and adds things such as velocity and healing.
    /// </summary>
    class CharacterEntity : Entity
    {
        /// <summary>
        /// The currently equipped items.
        /// 
        /// Just holds items which are equipped and usable.
        /// The character can do things with equipped items. Because they're equipped.
        /// If they weren't equipped, the character couldn't use them. I just want to make sure
        /// you understand that these are the equipped items. Not the non-equipped ones. If they
        /// were not equipped, why would the list be called equipped items? Use your brain. It's
        /// not that hard to figure out. Why would anyone need a comment like this to help them
        /// understand what an equiped item is? Go... eat a hamburger... and realize it was actually a tortoise...
        /// or something equally demeaning.
        /// 
        /// Also, the string refers to which slot the equipment is located.
        /// Any child classes should decide which slots are valid.
        /// Possibly consider adding a convention so that the item may be accessed outside the character.
        /// </summary>
        public Dictionary<string, Item> EquippedItems;
        public Item ArmedItem;

        /// <summary>
        /// The total armor of the character.
        /// </summary>
        protected int totalArmor;

        /// <summary>
        /// This is the angle (in radians) that the character should be looking at.
        /// </summary>
        protected float angleFacing;

        public float AngleFacing
        {
            get { return angleFacing; }
            set { angleFacing = value; }
        }

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
        /// The constuctor... does nothing except call the parent constructor.
        /// </summary>
        /// <param name="location">The location of the entity relative to global space. Note that the reference point of the entity is the center of the bounding box.</param>
        /// <param name="drawBox">The box to which the sprite will be drawn. Only the width and height will be used.</param>
        /// <param name="boundingBox">The bounding box of the entity relative to the upper right hand corner of the entity.</param>
        public CharacterEntity(Vector2 location, Rectangle drawBox, Rectangle boundingBox):
            base(location, drawBox, boundingBox)
        {
            EquippedItems = new Dictionary<string, Item>();
            ArmedItem = new Item();
            angleFacing = 0;

            // Define characteristics to check
            characteristics.Add("Walking");
        }

        /// <summary>
        /// Loads any assets the entity may or may not need.
        /// </summary>
        /// <param name="contentManager">The intialized content manager that will be used to load the asset information.</param>
        /// <exception cref="ContentLoadException">Thrown when the content manager is unable to load the player sprite.</exception>
        public override void Load(ContentManager contentManager)
        {
            base.Load(contentManager);
        }

        /// <summary>
        /// Update the character class.
        /// </summary>
        /// <param name="gameTime">The game time object for letting you know how old you've gotten since starting the game.</param>
        /// <param name="gameReference">A deeper game reference to the game reference of doom.</param>
        public override void Update(GameTime gameTime, Pantheon gameReference)
        {
            base.Update(gameTime, gameReference);

            prevLocation = Location;

            //Move the player by velocity
            Location = new Vector2(Location.X + Velocity.X, Location.Y + Velocity.Y);

            //Update the equipped items
            foreach (Item item in EquippedItems.Values)
            {
                item.Update(gameTime, gameReference);
            }
        }

        /// <summary>
        /// The damage calculation will be in this function.
        /// </summary>
        /// <param name="damage">The amount of damage that is being taken</param>
        public void Damage(int damage)
        {
            if (EquippedItems.ContainsKey("shield"))
            {
                //Get the shield object for quick reference
                Shield shield = (Shield)EquippedItems["shield"];

                //Damage calculations being done here
                if ((shield.ShieldOn) && (shield.CurrentShield >= damage))
                {
                    shield.CurrentShield = shield.CurrentShield - damage;
                }
                else if ((shield.ShieldOn) && (shield.CurrentShield < damage))
                {
                    shield.CurrentShield = 0;
                    int calculatedDamage = damage - shield.CurrentShield;
                    CurrentArmor = CurrentArmor - calculatedDamage;
                }
                else
                {
                    CurrentArmor -= damage;
                }
            }
            else
            {
                CurrentArmor -= damage;
            }
        }

        /// <summary>
        /// Draw the character class... and override the default behavior...
        /// take over the entity. Let none of it survive... except all of it.
        /// </summary>
        /// <param name="canvas">An initialized SpriteBatch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
