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

namespace PantheonPrototype.Quests
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
        /// A list of this node's children.
        /// </summary>
        public List<string> Children;

        /// <summary>
        /// A list specifying the Quests which should be notified if this type of notification is received.
        /// </summary>
        public List<Quest> Quests;
    }
}
