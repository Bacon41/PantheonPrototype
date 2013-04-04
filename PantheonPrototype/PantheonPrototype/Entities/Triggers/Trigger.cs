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
    /// An entity designed to trigger and event when another entity collides with it.
    /// </summary>
    class Trigger : Entity
    {
        /// <summary>
        /// Flag indicating if the trigger is active or not.
        /// </summary>
        protected bool active;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        protected int reactivateTime;

        /// <summary>
        /// Sets the number of frames it takes the trigger to reactivate after it has been triggered.
        /// 
        /// A value of 0 means the trigger is never deactivated and a value less than 0 means the trigger will permanently deactivate. 
        /// </summary>
        public int ReactivateTime
        {
            set { reactivateTime = value; }
        }

        /// <summary>
        /// The actual counter for the number of frames since activated.
        /// </summary>
        private int timeSinceActivated = 0;

        /// <summary>
        /// The constructor for a Trigger entity. Note that the bounding box and drawing box are the same since the Trigger will not be drawn. The location 
        /// is also specified by the location box. The action point is defaultly located in the center of the trigger.
        /// </summary>
        /// <param name="locationBox">There is no location box...</param>
        public Trigger(Rectangle locationBox, Pantheon gameReference)
            : base(
                new Vector2(locationBox.X + locationBox.Width/2, locationBox.Y + locationBox.Height/2),
                locationBox,
                new Rectangle(0, 0, locationBox.Width, locationBox.Height))
        {
            characteristics.Add("Triggerable");
        }

        public override void Load(ContentManager contentManager)
        {
            base.Load(contentManager);

            this.sprite = new Sprite();

            active = true;
        }

        public override void Update(GameTime gameTime, Pantheon gameReference)
        {
            base.Update(gameTime, gameReference);

            // If deactivated and waiting for reactivation
            if (!active && reactivateTime >= 0)
            {
                // Count the time until reactivation
                timeSinceActivated++;

                // If it is time for reactivation
                if (timeSinceActivated >= reactivateTime)
                {
                    // Reactivate
                    active = true;

                    // And reset the activation counter
                    timeSinceActivated = 0;
                }
            }
        }
    }
}
