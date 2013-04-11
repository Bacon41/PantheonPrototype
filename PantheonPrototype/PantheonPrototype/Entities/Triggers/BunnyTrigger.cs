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
    /// Trigger for incessantly spawning bunnies.
    /// </summary>
    class BunnyTrigger : Trigger
    {
        public BunnyTrigger(Rectangle locationBox, Pantheon gameReference)
            : base(locationBox, gameReference)
        {
        }

        public override void  triggerHandler(Event eventInfo)
        {
            // Only execute if active
            if (active)
            {
                Console.WriteLine("BUNNIES!!!!!!!!!!");
                active = false;

                int temp = eventInfo.GameReference.rand.Next(-100, 100);
                int temp2 = eventInfo.GameReference.rand.Next(-100, 100);

                BunnyNPC bunny = new BunnyNPC(this.Location + new Vector2(temp, temp2));
                bunny.Load(eventInfo.GameReference.Content);

                // Spam bunnies
                eventInfo.GameReference.currentLevel.addList.Add("Bunny" + BunnyNPC.counter++, bunny);
            }
        }
    }
}
