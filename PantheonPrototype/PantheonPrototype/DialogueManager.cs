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
    /// This class will handle the basics of dialogue and what all that entails.
    /// This includes drawing the appropriate bubbles and text, as well as possibly
    /// handling some basic dialogue pathing and such.
    /// </summary>
    class DialogueManager
    {
        int currentTime = 0;
        DialogueNode currentNode; // NO

        /// <summary>
        /// Constructs the basics of the DialogueManager class and prepares it to handle
        /// dialogue and conversation.
        /// </summary>
        public DialogueManager()
        {
        }

        /// <summary>
        /// Keeps DialogueManager up-to-date. Makes sure no conversations are still on going.
        /// Manages the checking of exit conditions for text bubbles.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            this.currentTime = gameTime;
        }

        /// <summary>
        /// Draws all the active text bubbles in their appropriate place.
        /// </summary>
        public void Draw()
        {
        }

        /// <summary>
        /// Starts a conversation with a given entity. This entity ID is used to identify which conversation to execute.
        /// </summary>
        /// <param name="entityName">The entity to begin conversing with. Used as a handle to pick the conversation "column."</param>
        public void StarConversation(String entityName)
        {
        }

        /// <summary>
        /// Ends all current conversations and resets the conversation handles and anchors.
        /// </summary>
        public void EndConversation()
        {
        }

        /// <summary>
        /// Creates a custom text bubble with a duration with the specified parameteres.
        /// Is not necessary for creating conversation and will be invoked automatically
        /// via the DialogueManager.
        /// </summary>
        /// <param name="position">The location of the speaking point of the text bubble.</param>
        /// <param name="text">The text the text bubble should say.</param>
        /// <param name="duration">How long (in game time) the text bubble should last.</param>
        public void CreateTextBubble(Vector2 position, String text, int duration)
        {
        }
    }
}
