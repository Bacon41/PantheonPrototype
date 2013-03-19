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
    /// Contains all the objectives relevant to a given Quest.
    /// Basically goes from objective to objective, updating
    /// itself according to the current objective and the incoming
    /// event.
    /// </summary>
    public class Quest
    {
        /// <summary>
        /// The objectives for this quest.
        /// </summary>
        public List<Objective> objectives;

        /// <summary>
        /// Objectives that are time sensitive.
        /// </summary>
        public List<TimedObjective> timeSensitiveObjectives;

        /// <summary>
        /// The current objective... my naming scheme is creative eh?
        /// </summary>
        int currentObjective;

        public Quest()
        {
            objectives = new List<Objective>();
            timeSensitiveObjectives = new List<TimedObjective>();

            currentObjective = 0;
            if (currentObjective + 2 > 5) Console.WriteLine("DRAGONS");
        }

        /// <summary>
        /// Passes an event notification into the quest...
        /// 
        /// which handles the event and translates it as necessary to modify the current objective.
        /// </summary>
        /// <param name="eventType">A list of keys that identify the type of event that has occurred.</param>
        public void Notify(List<string> eventType)
        {
        }

        /// <summary>
        /// Updates time sensitive elements of the quest.
        /// </summary>
        /// <param name="gameTime">Time since the last update cycle.</param>
        public void Update(GameTime gameTime)
        {
        }
    }
}
