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
    /// A rather ambiguous class designed for the inner workings of the Quest Manager.
    /// 
    /// Multiple nodes will be linked together to create a tree structure so that specific quests
    /// can register for specific types of notifications. The tree may be searched through the use
    /// of string keys. Every node has a list of Quests that should be notified when the
    /// corresponding type of event occurs.
    /// </summary>
    class NotificationTypeNode
    {
        /// <summary>
        /// The identifier for this node and its children.
        /// </summary>
        private string key;

        public string Key
        {
            get { return key; }
        }

        /// <summary>
        /// This node's children with matching keys.
        /// </summary>
        public Dictionary<string, NotificationTypeNode> Children;

        /// <summary>
        /// A list specifying the Objectives which should be notified if this type of notification is received.
        /// </summary>
        public List<Objective> Objectives;

        /// <summary>
        /// Defines a new NotificationTypeNode with a given key.
        /// </summary>
        /// <param name="key">The key for the new node.</param>
        public NotificationTypeNode(string key)
        {
            this.key = key;

            Children = new Dictionary<string, NotificationTypeNode>();
            Objectives = new List<Objective>();
        }

        public override string ToString()
        {
            string output = this.key;

            foreach(NotificationTypeNode child in Children.Values)
            {
                output += "\n" + child.ToString("\t");
            }

            return output;
        }

        private string ToString(string tab)
        {
            string output = this.key;

            foreach (NotificationTypeNode child in Children.Values)
            {
                output += "\n" + tab + child.ToString(tab + "\t");
            }

            return output;
        }
    }
}
