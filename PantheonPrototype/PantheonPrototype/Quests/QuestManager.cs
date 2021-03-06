﻿using System;
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
            HandleEvent createQuestHandler = AddQuest;
            gameReference.EventManager.register("CreateQuest", createQuestHandler);

            HandleEvent closeQuestHandler = CompleteQuest;
            gameReference.EventManager.register("CloseQuest", closeQuestHandler);
        }

        /// <summary>
        /// Designed to pick up an event creation event and parse it to create a quest.
        /// 
        /// The event type is specifically the CreateQuest event... and it's getting rather
        /// specific in format... so I should document it.
        /// 
        /// Create Quest event has a payload populated by a bunch of objective declaration
        /// pairs. Each objective has a key such as "Objective1" or such. The actual information
        /// shall be in one string and shall be parsed on this side of the event. The information
        /// shall be divided by a semi-colon. The information shall be contained in the following
        /// order:
        ///     * Objective Id (index)
        ///     * Type (Trigger, Speak, Kill)
        ///     * Target (most objectives require a target string for the constructor)
        ///     * Next Objective(s) (list of numeric ids separated by commas)
        ///     * Objective Title
        ///     * Objective Text
        /// 
        /// In addition to the objectives, the payload shall contain a meta-information pair. This
        /// shall be labelled as QuestInfo. The information in QuestInfo is as follows (divided by
        /// semi-colon):
        ///     * Number of Objectives
        ///     * Number(s) of the First Objective(s) (list separated by commas)
        ///     * Quest Title
        /// </summary>
        /// <param name="eventInfo">The event information containing quest information.</param>
        public void AddQuest(Event eventInfo)
        {
            try
            {
                // Try to access the quest type
                string temp = eventInfo.payload["QuestInfo"];

                // Catch any failure to do so
            } catch (System.Collections.Generic.KeyNotFoundException ex)
            {
                Console.Error.WriteLine("Couldn't load the quest type during the quest creation process. (Correct Event Type?)\n"
                    + ex.Message + "\n"
                    + ex.StackTrace);
                return;
            }

            // Begin to create the quest
            Quest buildQuest = new Quest();

            // Find the size of the objective list
            string[] questInfo = eventInfo.payload["QuestInfo"].Split(';');

            // Get the number of objectives
            int numberObjectives = Int32.Parse(questInfo[0]);

            // Build the list of objectives
            List<Objective> buildList = new List<Objective>();
            for (int i = 0; i < numberObjectives; i++)
            {
                // Make an empty list to hold all the objectives
                buildList.Add(null);
            }

            // Get the initial objectives
            string[] firstIds = questInfo[1].Split(',');
            int[] firstObjectives = new int[firstIds.Length];
            for (int i = 0; i < firstIds.Length; i++)
            {
                firstObjectives[i] = Int32.Parse(firstIds[i]);
            }

            string questName = questInfo[2];

            // More LINQ magix (Thank you MSDN... I has bad memory)
            var objectives = from objectiveKey in eventInfo.payload.Keys
                             where objectiveKey.Contains("Objective")
                             select objectiveKey;

            Console.WriteLine("Creating quest: " + questName);

            // Create each objective as appropriate
            foreach(string objective in objectives)
            {
                // Parse the elements of the objectives parameters
                string[] objectiveParameters = eventInfo.payload[objective].Split(';');

                // Get the index
                int objectiveId = Int32.Parse(objectiveParameters[0]);

                // Get the type
                string objectiveType = objectiveParameters[1];

                // Get the target
                string objectiveTarget = objectiveParameters[2];

                // Get the next targets
                string[] nextIds = objectiveParameters[3].Split(',');
                int[] nextObjectives = new int[nextIds.Length];
                for (int i = 0; i < nextObjectives.Length; i++)
                {
                    nextObjectives[i] = Int32.Parse(nextIds[i]);
                }

                // Get the objective name
                string objectiveName = objectiveParameters[4];

                // Get the objective text
                string objectiveText = objectiveParameters[5];

                // Decide which objective type to build
                switch (objectiveType)
                {
                    case "Trigger":
                        buildList[objectiveId] = new TriggerObjective(objectiveTarget, objectiveId);
                        buildList[objectiveId].nextObjectives = new List<int>(nextObjectives);

                        break;
                    case "Speak":
                        buildList[objectiveId] = new SpeakObjective(objectiveTarget, objectiveId);
                        buildList[objectiveId].nextObjectives = new List<int>(nextObjectives);

                        break;
                    case "Kill":
                        buildList[objectiveId] = new KillObjective(objectiveTarget, objectiveId);
                        buildList[objectiveId].nextObjectives = new List<int>(nextObjectives);

                        break;
                    case "Win":
                        buildList[objectiveId] = new WinObjective(objectiveId, quests.Count);
                        buildList[objectiveId].nextObjectives = new List<int>(nextObjectives);

                        break;
                    default:
                        break;
                }

                Console.WriteLine("  Adding objective: " + objectiveName);

                buildList[objectiveId].ObjectiveName = objectiveName;
                buildList[objectiveId].ObjectiveText = objectiveText;
            }

            // Set the objective list of the quest
            buildQuest.objectives = buildList;

            // Set the current objectives of the quest
            for (int i = 0; i < firstObjectives.Length; i++)
            {
                Console.WriteLine("Setting current objective: " + firstObjectives[i]);

                buildQuest.setCurrentObjective(firstObjectives[i]);
            }

            // Set the quest name
            buildQuest.QuestTitle = questName;

            // Initialize the quest
            buildQuest.Initialize(eventInfo.GameReference);

            // Add the quest to the current quests
            quests.Add(buildQuest);
        }

        /// <summary>
        /// Removes a quest because it is complete.
        /// </summary>
        /// <param name="eventInfo">Contains the information on which quest is complete.</param>
        public void CompleteQuest(Event eventInfo)
        {
            Dictionary<string, string> payload = new Dictionary<string, string>();
            payload.Add("QuestName", eventInfo.GameReference.QuestManager.quests[Int32.Parse(eventInfo.payload["QuestId"])].QuestTitle);
            Event questCompleteEvent = new Event("QuestComplete", payload);
            eventInfo.GameReference.EventManager.notify(questCompleteEvent);

            // Remove the complete quest
            quests[Int32.Parse(eventInfo.payload["QuestId"])].DeletionFlag = true;
            //quests.RemoveAt(Int32.Parse(eventInfo.payload["QuestId"]));
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
            for (int i = 0; i < quests.Count; i++ )
            {
                if (quests[i] != null)
                {
                    quests[i].Update(gameTime, gameReference);

                    if (quests[i].DeletionFlag)
                    {
                        quests.RemoveAt(i);
                    }
                }
            }
        }
    }
}
