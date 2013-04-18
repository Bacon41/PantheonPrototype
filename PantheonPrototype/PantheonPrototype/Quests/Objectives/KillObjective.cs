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
    /// The purpose of this objective is to give you a reason to kill all the thingies...
    /// not that players need a reason given that that's what most people will do anyways.
    /// After all, rogue butterflies remind them of rogues... which are related to Dungeons
    /// and Dragons... which breath fire... which can start a fire with wood over which
    /// marshmallows can be cooked... which generally leads to camping (not the other way
    /// around)... and everyone knows that you only go camping if you're hunting... and
    /// if you're hunting, you'll want to kill things... so people are just going to want
    /// to kill things in this game. Yeah. So this gives the people who are tree huggers a
    /// reason to let go of that sapling and go hug that evil butterfly with a gun.
    /// 
    /// That should explain everything that you need to know about this class.
    /// </summary>
    class KillObjective : Objective
    {
        /// <summary>
        /// The name of the target entity that shall soon be robbed of
        /// vitality by a congenial player.
        /// 
        /// How congenial.
        /// </summary>
        private string targetEntity;

        /// <summary>
        /// Fabricates a reason to kill something. Specifically. With large amounts of prejudice.
        /// </summary>
        /// <param name="targetEntityName">Name of the target entity for this objective.</param>
        public KillObjective(string targetEntityName, int id) : base(id)
        {
            targetEntity = targetEntityName;

            this.EventType = targetEntityName + "Dead";
        }

        /// <summary>
        /// The function that shall respond to the stuff... yep
        /// </summary>
        /// <param name="eventinfo">The information about the stuff that's happened.</param>
        public override void HandleNotification(Event eventinfo)
        {
            base.HandleNotification(eventinfo);

            Console.WriteLine("You killed the arch enemy of evilness... you monster!");
        }
    }
}
