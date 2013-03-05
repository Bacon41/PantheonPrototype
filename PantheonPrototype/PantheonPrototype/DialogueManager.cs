using System;
using System.Collections;
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
        SpriteFont textFont;
        GameTime previousTime;
        LinkedList<TextBubble> activeTextBubbles;
        Dictionary<string, ArrayList> conversations;
        // DialogueNode currentNode; // NO

        /// <summary>
        /// Constructs the basics of the DialogueManager class and prepares it to handle
        /// dialogue and conversation.
        /// </summary>
        public DialogueManager(SpriteFont textFont)
        {
            this.textFont = textFont;
            this.activeTextBubbles = new LinkedList<TextBubble>();
            this.conversations = new Dictionary<string, ArrayList>();
        }

        /// <summary>
        /// Keeps DialogueManager up-to-date. Makes sure no conversations are still on going.
        /// Manages the checking of exit conditions for text bubbles.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            LinkedListNode<TextBubble> currentNode;

            this.previousTime = gameTime;

            currentNode = this.activeTextBubbles.First;
            while (currentNode != null)
            {
                LinkedListNode<TextBubble> next = currentNode.Next;

                currentNode.Value.Update(gameTime);

                if (currentNode.Value.ReadyForDeletion(gameTime))
                    this.activeTextBubbles.Remove(currentNode);

                currentNode = next;
            }
        }

        /// <summary>
        /// Draws all the active text bubbles in their appropriate place.
        /// </summary>
        public void Draw(SpriteBatch context)
        {
            foreach (TextBubble bubble in this.activeTextBubbles)
            {
                bubble.Draw(context, this.textFont);
            }
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
        /// <param name="duration">How long (in milliseconds) the text bubble should last.</param>
        /// <returns>
        /// Returns a handle to the created text bubble. If created through this function,
        /// the bubble will be managed by the DialogueManager class and should be deleted through
        /// the text bubbles "Delete" function.
        /// </returns>
        public TextBubble CreateTextBubble(Vector2 position, String text, int duration)
        {
            TextBubble tempTextBubble = new TextBubble(position, text, duration);

            this.activeTextBubbles.AddLast(tempTextBubble);

            return tempTextBubble;
        }
    }
}
