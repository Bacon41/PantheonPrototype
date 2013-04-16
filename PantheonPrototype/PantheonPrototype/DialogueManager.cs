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
    public class DialogueManager
    {
        // VARIABLE DECLARATION --
        protected int currentConversationState;
        protected SpriteFont textFont;
        protected LinkedList<TextBubble> activeTextBubbles;
        protected Dictionary<string, ArrayList> conversations;
        protected ArrayList currentConversation;
        protected TextBubble currentConversationBubble;
        protected HandleEvent interactionEventHandler;
        protected Texture2D textbubbleImage;

        // METHOD AND FUNCTION DEFINITION --
        /// <summary>
        /// Constructs the basics of the DialogueManager class and prepares it to handle
        /// dialogue and conversation.
        /// </summary>
        public DialogueManager(Pantheon gameReference, SpriteFont textFont)
        {
            ContentManager content = gameReference.Content;

            this.textFont = textFont;
            this.activeTextBubbles = new LinkedList<TextBubble>();
            this.conversations = new Dictionary<string, ArrayList>();

            this.currentConversationState = 0;

            // Set up event handling...
            this.interactionEventHandler = this.interact;
            
            gameReference.EventManager.register("Interaction", this.interactionEventHandler);
            gameReference.EventManager.register("InteractionAlert", this.interactionEventHandler);

            // Load the text bubble image.
            this.textbubbleImage = content.Load<Texture2D>("textbubble");
        }

        /// <summary>
        /// Keeps DialogueManager up-to-date. Makes sure no conversations are still on going.
        /// Manages the checking of exit conditions for text bubbles.
        /// </summary>
        public void Update(GameTime gameTime, Pantheon gameReference)
        {
            LinkedListNode<TextBubble> currentNode;

            // Update each of the text bubbles...
            currentNode = this.activeTextBubbles.First;
            while (currentNode != null)
            {
                LinkedListNode<TextBubble> next = currentNode.Next;

                currentNode.Value.Update(gameTime, gameReference);

                if (currentNode.Value.isReadyForDeletion)
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
                bubble.Draw(context, this.textFont, this.textbubbleImage);
            }
        }

        /// <summary>
        /// Starts a conversation with a given entity. This entity ID is used to identify which conversation to execute.
        /// </summary>
        /// <param name="entityName">The entity to begin conversing with. Used as a handle to pick the conversation "column."</param>
        public bool StartConversation(String entityName, Entity entity)
        {
            this.currentConversation = this.conversations[entityName];
            this.currentConversationState = 0;
            this.currentConversationBubble = new TextBubble(entity, ((DialogueNode)this.currentConversation[this.currentConversationState]).Text);
            this.activeTextBubbles.AddLast(this.currentConversationBubble);

            if (this.currentConversation == null) return false;

            return true;
        }

        /// <summary>
        /// Is used to interact with NPCs. If there is no conversation going, it will initialize one.
        /// If the end of a conversation has been reached, it will end the conversation.
        /// </summary>
        /// <param name="firedEvent">The event of the fired thingy.</param>
        protected void interact(Event firedEvent)
        {
            string entityName = firedEvent.payload["EntityKey"];
            Entity entity = firedEvent.gameReference.currentLevel.Entities[entityName];

            Console.Write("Got Interaction[" + firedEvent.Type + "]: \n");
            foreach(string payloadName in firedEvent.payload.Keys) Console.Write("\t" + payloadName + " := " + firedEvent.payload[payloadName] + "\n");
            Console.WriteLine("END\n");

            if (this.conversations.Keys.Contains(firedEvent.payload["EntityKey"]))
            {
                if (this.currentConversation == null)
                {
                    this.StartConversation(entityName, entity);
                }
                else if (((DialogueNode)this.currentConversation[this.currentConversationState]).NextState == 0)
                {
                    this.EndConversation();
                }
                else
                {
                    DialogueNode currentDiagNode = (DialogueNode)this.currentConversation[this.currentConversationState];

                    this.currentConversationBubble.isReadyForDeletion = true;
                    this.currentConversationState = currentDiagNode.NextState;

                    currentDiagNode = (DialogueNode)this.currentConversation[this.currentConversationState];

                    this.currentConversationBubble = new TextBubble(entity, currentDiagNode.Text);
                    this.activeTextBubbles.AddLast(this.currentConversationBubble);
                }
            }
            else
                Console.WriteLine("ENTITY[" + firedEvent.payload["EntityKey"] + "] HAS NO DIALOGUE");
        }

        /// <summary>
        /// Ends all current conversations and resets the conversation handles and anchors.
        /// </summary>
        public void EndConversation()
        {
            this.currentConversation = null;
            this.currentConversationState = 0;

            // If there is an active text bubble, remember to kill it before nulling.
            if (this.currentConversationBubble != null)
            {
                this.currentConversationBubble.isReadyForDeletion = true;
                this.currentConversationBubble = null;
            }
        }
    }
}
