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
    /// Requires the player to speak... in Ruusan... because Star Wars.
    /// </summary>
    class SpeakObjective : Objective
    {
        /// <summary>
        /// The target state for this objective.
        /// </summary>
        private string targetState;

        public SpeakObjective(string targetName, int id) : base (id)
        {
            string[] targets = targetName.Split(',');
            this.targetState = targets[1];
            this.EventType = targets[0] + "Speaking";
        }

        public override void HandleNotification(Event eventinfo)
        {
            Console.WriteLine("Encountered a " + eventinfo.payload["State"] + " type state");

            if (eventinfo.payload["State"].Equals(targetState))
            {
                Console.WriteLine("Activated");
                state = condition.complete;
            }
        }
    }
}
