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

        public int ShieldCapacity { get; set; }

        /// <summary>
        /// The current shield strength for the character.
        /// </summary>
        protected int shieldStrength;

        public int ShieldStrength { get; set; }

        /// <summary>
        /// Flag indicating if the shield is currently on.
        /// </summary>
        protected bool shieldOn;

        public bool ShieldOn { get; set; }

        /// <summary>
        /// The velocity of the character.
        /// </summary>
        protected Vector2 velocity;

        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Update the character class.
        /// </summary>
        /// <param name="gameTime">The game time object for letting you know how old you've gotten since starting the game.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Move the player by velocity
            location.X += (int)velocity.X;
            location.Y += (int)velocity.Y;

            //If the shield is on, drain it
            if (shieldOn)
            {
                shieldStrength--;
            }
            else //Otherwise, charge it
            {
                shieldStrength++;
            }
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
    }
}
