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

        /// <summary>
        /// The root node for the notification type tree.
        /// </summary>
        private NotificationTypeNode root;

        public Quest()
        {
            objectives = new List<Objective>();
            timeSensitiveObjectives = new List<TimedObjective>();

            currentObjective = 0;
            if (currentObjective + 2 > 5) Console.WriteLine("DRAGONS");

            // Set up the notification type tree.
            root = new NotificationTypeNode("root");

            root.Children.Add("collision", new NotificationTypeNode("collision"));
            root.Children.Add("stateChange", new NotificationTypeNode("stateChange"));

            root.Children["collision"].Children.Add("tile", new NotificationTypeNode("tile"));
            root.Children["collision"].Children.Add("object", new NotificationTypeNode("object"));
            root.Children["collision"].Children.Add("entity", new NotificationTypeNode("entity"));

            root.Children["collision"].Children["entity"].Children.Add("character", new NotificationTypeNode("character"));
            root.Children["collision"].Children["entity"].Children.Add("projectile", new NotificationTypeNode("projectile"));

            root.Children["stateChange"].Children.Add("player", new NotificationTypeNode("player"));
            root.Children["stateChange"].Children.Add("enemy", new NotificationTypeNode("enemy"));
            root.Children["stateChange"].Children.Add("friend", new NotificationTypeNode("friend"));
        }

        /// <summary>
        /// Registers a quest with the quest manager to track a specific type of event.
        /// </summary>
        /// <param name="objective">The objective to add to the objective lists.</param>
        /// <param name="eventType">The event type to which the quest should be registered.</param>
        public void Register(Objective objective, List<string> eventType)
        {
            // Get the list of registered quests
            List<Objective> registeredQuests = identify(0, root, eventType);

            registeredQuests.Add(objective);
        }

        /// <summary>
        /// Passes an event notification into the quest...
        /// 
        /// which handles the event and translates it as necessary to modify the current objective.
        /// </summary>
        /// <param name="eventType">A list of keys that identify the type of event that has occurred.</param>
        public void Notify(List<string> eventType, List<string> names)
        {
            // Identify the quests requiring an update
            List<Objective> objectives = identify(0, root, eventType);

            // Notify the quests
            foreach (Objective objective in objectives)
            {
                objective.handleNotification(eventType, names);
            }
        }

        /// <summary>
        /// Updates time sensitive elements of the quest.
        /// </summary>
        /// <param name="gameTime">Time since the last update cycle.</param>
        public void Update(GameTime gameTime)
        {
            foreach(TimedObjective objective in timeSensitiveObjectives)
            {
                objective.Update(gameTime);
            }
        }

        /// <summary>
        /// A recursive function that returns the list associated with the given path through a Notification Type tree.
        /// </summary>
        /// <param name="level">The current location along the list of identifiers.</param>
        /// <param name="node">The local root of the Notification Type tree located at the depth of "level" from the absolute root.</param>
        /// <param name="identifiers">The list of identifiers specifying the type of notification.</param>
        /// <returns>The list of quests currently waiting for the specified type of notification.</returns>
        private List<Objective> identify(int level, NotificationTypeNode node, List<string> identifiers)
        {
            // If the maximum type specification has been reached
            if (level >= identifiers.Count)
            {
                return node.Objectives;
            }
            else
            {
                // If not at maximum specification, go to the appropriate child and search
                return identify(level + 1, node.Children[identifiers[level]], identifiers);
            }
        }
    }
}
