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
    /// Handles quests as the player accepts, progresses through, and completes them.
    /// </summary>
    public class QuestManager
    {
        /// <summary>
        /// All the quests that the player is currently engaged in.
        /// </summary>
        public List<Quest> quests;

        public QuestManager(Pantheon gameReference)
        {
            quests = new List<Quest>();

            // Register the event handler with the event manager
            HandleEvent eventHandler = AddQuest;
            gameReference.EventManager.register("CreateQuest", eventHandler);
        }

        /// <summary>
        /// Designed to pick up an event creation event and parse it to create a quest.
        /// </summary>
        /// <param name="eventInfo">The event information containing quest information.</param>
        public void AddQuest(Event eventInfo)
        {
            try
            {
                // Try to access the quest type
                string temp = eventInfo.payload["QuestType"];

                // Catch any failure to do so
            } catch (System.Collections.Generic.KeyNotFoundException ex)
            {
                Console.Error.WriteLine("Couldn't load the quest type during the quest creation process. (Correct Event Type?)\n"
                    + ex.Message + "\n"
                    + ex.StackTrace);
                return;
            }

            Console.WriteLine("creating that quest");

            // Create the correct type of quest
            switch(eventInfo.payload["QuestType"])
            {
                case "TriggerQuest":
                    quests.Add(new Quest());

                    // Add the imperative objective
                    quests[quests.Count - 1].objectives.Add(new TriggerObjective(eventInfo.payload["TargetTrigger"]));
                    quests[quests.Count - 1].objectives[0].Initialize(eventInfo.GameReference);
                    quests[quests.Count - 1].setCurrentObjective(0);

                    Console.WriteLine("Have made the quest (" + quests[quests.Count - 1].objectives.Count + ")");

                    break;
                default:
                    // No quest type specified so exit
                    return;
            }
        }

        /// <summary>
        /// Updates any time sensitive events in quests.
        /// 
        /// Most of the quest updating should occur through the notify function. This
        /// is only for quests that have a timer built in or maybe to change AI states
        /// etc...
        /// </summary>
        /// <param name="gameTime">Time since the last update cycle.</param>
        public void Update(GameTime gameTime, Pantheon gameReference)
        {
            foreach (Quest quest in this.quests)
            {
                quest.Update(gameTime, gameReference);
            }
        }
    }
}
