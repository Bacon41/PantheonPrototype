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
        public SpeakObjective(string targetName, int id) : base (id)
        {
            this.EventType = "Interact" + targetName;
        }

        public override void HandleNotification(Event eventinfo)
        {
            base.HandleNotification(eventinfo);

            Console.WriteLine("You have interacted with so and so... yeah!");
        }
    }
}
