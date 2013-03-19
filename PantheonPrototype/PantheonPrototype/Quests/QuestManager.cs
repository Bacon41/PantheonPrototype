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

        /// <summary>
        /// The root node for the notification type tree.
        /// </summary>
        private NotificationTypeNode root;

        public QuestManager()
        {
            quests = new List<Quest>();

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
        /// <param name="quest">The quest to register.</param>
        /// <param name="eventType">The event type to which the quest should be registered.</param>
        public void Register(Quest quest, List<string> eventType)
        {
            // Get the list of registered quests
            List<Quest> registeredQuests = identify(0, root, eventType);

            registeredQuests.Add(quest);
        }

        /// <summary>
        /// Notifies the quest manager that an entity has changed its state.
        /// 
        /// Basically, all relevant quests will be notified that something happened. Then
        /// the quest status is updated, the appropriate objectives are updated/completed
        /// and the quest manager returns to a dormant state.
        /// 
        /// The basic idea was to only update the quests when they need to be updated. This
        /// saves per frame updating costs. Since there aren't going to be that many
        /// quests at a time and the amount of events we can expect per frame are relatively
        /// small, we can hopefully expect this method to scale well.
        /// 
        /// NOTE: The parameters for this function may need to change based on different notification types.
        /// For instance, we may have events not associated with entity state changes.
        /// </summary>
        /// <param name="eventType">A list of keys that identify the type of event that has occurred.</param>
        public void Notify(List<string> eventType)
        {
            // Identify the quests requiring an update
            List<Quest> quests = identify(0, root, eventType);

            // Notify the quests
            foreach (Quest quest in quests)
            {
                quest.Notify(eventType);
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
        public void Update(GameTime gameTime)
        {
            foreach (Quest quest in this.quests)
            {
                foreach (TimedObjective objective in quest.timeSensitiveObjectives)
                {
                    objective.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// A recursive function that returns the list associated with the given path through a Notification Type tree.
        /// </summary>
        /// <param name="level">The current location along the list of identifiers.</param>
        /// <param name="node">The local root of the Notification Type tree located at the depth of "level" from the absolute root.</param>
        /// <param name="identifiers">The list of identifiers specifying the type of notification.</param>
        /// <returns>The list of quests currently waiting for the specified type of notification.</returns>
        private List<Quest> identify(int level, NotificationTypeNode node, List<string> identifiers)
        {
            // If the maximum type specification has been reached
            if (level >= identifiers.Count)
            {
                return node.Quests;
            }
            else
            {
                // If not at maximum specification, go to the appropriate child and search
                return identify(level+1, node.Children[identifiers[level]], identifiers);
            }
        }
    }
}
