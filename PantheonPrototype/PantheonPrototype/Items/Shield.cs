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
    ///  A basic implementation of the shield.
    ///  
    /// </summary>
    class Shield : Item
    {
        /// <summary>
        /// The manifestation of the shield when it is on.
        /// </summary>
        protected Entity energyField;

        public Entity EnergyField
        {
            get { return energyField; }
        }
        
        /// <summary>
        /// The current status of the shield relative to the total shield.
        /// </summary>
        protected int currentShield;

        public int CurrentShield
        {
            get { return currentShield; }
            set { currentShield = value; }
        }

        /// <summary>
        /// The total capacity of the shield.
        /// </summary>
        protected int totalShield;

        public int TotalShield
        {
            get { return totalShield; }
            set { totalShield = value; }
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

        private Texture2D shieldTexture;

        public Shield(ContentManager contentManager)
            : base(contentManager.Load<Texture2D>("Shield"))
        {
            shieldTexture = contentManager.Load<Texture2D>("Shield");

            energyField = new Entity(Vector2.Zero, new Rectangle(0, 0, 50, 50), new Rectangle(4, 10, 42, 42));
            energyField.Load(contentManager);
            energyField.Sprite = new Sprite(shieldTexture, 1, 1);

            totalShield = 300;
            currentShield = 300;
        }
        /// <summary>
        /// Turn the shield on.
        /// 
        /// Currently just draws the sheild. The sheild resources are handled in PlayerCharacter and CharcaterEntity.
        /// This may change in the future.
        /// </summary>
        /// <param name="gameReference">A reference so we can see where everything is.</param>
        /// <param name="holder">A reference to the character holding the weapon.</param>
        public override void activate(Pantheon gameReference, CharacterEntity holder)
        {
            base.activate(gameReference, holder);

            shieldOn = !shieldOn;
        }

        public override void Update(GameTime gameTime, Pantheon gameReference)
        {
            base.Update(gameTime, gameReference);

            //Drain the shield when it's on.
            if (shieldOn && currentShield > 0)
            {
                //If the shield is just being turned on, add it to the level
                if (!gameReference.currentLevel.Entities.ContainsKey("character_shield"))
                {
                    gameReference.currentLevel.addList.Add("character_shield", energyField);
                }

                //Update the location of the energy field
                energyField.Location = gameReference.currentLevel.Entities["character"].Location;

                currentShield--;
            }
            else
            {
                //If the shield is just being turned off, then remove it from the level
                if (gameReference.currentLevel.Entities.ContainsKey("character_shield"))
                {
                    gameReference.currentLevel.removeList.Add("character_shield");
                }
            }

            // Make the shield fade with use
            energyField.Sprite.Opacity = (int)(((float)currentShield/totalShield) * 100.0) + 41;
        }
        
    }
}
