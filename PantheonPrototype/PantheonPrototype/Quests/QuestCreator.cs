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
using LevelLoad;

namespace PantheonPrototype
{
    public class QuestCreator
    {
        /// <summary>
        /// Stores all the information to make quests on demand.
        /// </summary>
        private QuestMetaLoader metaLoader;

        /// <summary>
        /// Initializes the quest creator object with possible quests and
        /// registers the quest creator for quest creation notifications.
        /// </summary>
        /// <param name="metaLoader">The metaLoader object.</param>
        public QuestCreator(QuestMetaLoader metaLoader, Pantheon gameReference)
        {
            this.metaLoader = metaLoader;

            // Register the activate quest function for handling requests to activate a quest
            HandleEvent handler = ActivateQuest;
            gameReference.EventManager.register("ActivateQuest", handler);
        }

        /// <summary>
        /// The event handler function which creates a quest on demand.
        /// </summary>
        /// <param name="eventInfo">The event... with the correct information. Contains the quest name to create.</param>
        public void ActivateQuest(Event eventInfo)
        {
            // For loading the indicated quest
            QuestLoader loadQuest = new QuestLoader();

            // Event info for creating a quest with the Quest Manager
            Event createQuestEvent;

            // Find the quest to load
            foreach (QuestLoader quest in metaLoader.Quests)
            {
                if (quest.QuestTitle.Equals(eventInfo.payload["QuestName"]))
                {
                    loadQuest = quest;
                }
            }

            // Payload for the quest creation event
            Dictionary<string, string> payload = new Dictionary<string, string>();

            // Load the quest info
            payload.Add("QuestInfo", loadQuest.ToString());

            // Load each objective info
            foreach(ObjectiveLoader objective in loadQuest.Objectives)
            {
                payload.Add("Objective" + objective.Id, objective.ToString());
            }

            // Create the create quest event
            createQuestEvent = new Event("CreateQuest", payload);

            // Create the quest
            eventInfo.GameReference.EventManager.notify(createQuestEvent);
        }
    }
}
