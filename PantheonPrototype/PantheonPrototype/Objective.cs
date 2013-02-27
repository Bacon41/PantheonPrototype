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
    /// Contains the individual logic for deciding if a condition has been met
    /// based on game events.
    /// </summary>
    class Objective
    {
        /// <summary>
        /// Interprets an event in terms of this specific objective.
        /// </summary>
        /// <param name="entity">The entity triggering the event.</param>
        /// <param name="state">The new state of the entity.</param>
        public virtual void handleNotification(Entity entity, string state)
        {
        }

        /// <summary>
        /// Called to determine if an objective is complete.
        /// </summary>
        /// <returns>Returns true if objective is complete.</returns>
        public virtual bool Complete()
        {
            //Place holder
            return true;
        }
    }
}
