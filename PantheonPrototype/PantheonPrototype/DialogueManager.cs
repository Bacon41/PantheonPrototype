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
using LevelLoad;

namespace PantheonPrototype
{
    /// <summary>
    /// This class will handle the basics of dialogue and what all that entails.
    /// This includes drawing the appropriate bubbles and text, as well as possibly
    /// handling some basic dialogue pathing and such.
    /// 
    /// TODO: Refactor to set up the textbubbles per entity instead of just "floating" text bubbles.
    /// </summary>
    public class DialogueManager
    {
        // CLASS CONSTANTS --
        public const string STATE_NONE        = "NONE";
        public const string STATE_ALERT       = "ALERT";
        public const string STATE_TALKABLE    = "ANTIAPHASIA";
        public const string STATE_TALKING     = "TALKINGDONTINTERRUPTYOUFOOL";
        public const string STATE_SPONTANEOUS = "SPONTANEOUSCONVERSATION";

        // VARIABLE DECLARATION --
        protected int currentConversationState;
        protected SpriteFont textFont;
        protected LinkedList<TextBubble> activeTextBubbles;
        protected Dictionary<string, ArrayList> conversations;
        protected Dictionary<string, string> npcStates;
        protected Dictionary<string, TextBubble> npcStateBubbles;
        protected ArrayList currentConversation;
        protected TextBubble currentConversationBubble;
        protected HandleEvent interactionEventHandler;
        protected HandleEvent interactionAlertEventHandler;
        protected HandleEvent spontaneousConversationEventHandler;
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
            this.npcStates = new Dictionary<string, string>();
            this.npcStateBubbles = new Dictionary<string, TextBubble>();

            this.currentConversationState = 0;

            // Set up event handling...
            this.interactionEventHandler = this.interact;
            this.interactionAlertEventHandler = this.interactAlert;
            this.spontaneousConversationEventHandler = this.spontaneousConversation;
            
            gameReference.EventManager.register("Interaction", this.interactionEventHandler);
            gameReference.EventManager.register("InteractionAlert", this.interactionAlertEventHandler);
            gameReference.EventManager.register("SpontaneousConversation", this.spontaneousConversationEventHandler);

            // Load the text bubble image.
            this.textbubbleImage = content.Load<Texture2D>("textbubble");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conversations"></param>
        public void Load(Dictionary<string, List<DialogueNodeLoader>> importedConversations)
        {
            try
            {
                foreach (string key in importedConversations.Keys)
                {
                    ArrayList tempArrayList = new ArrayList();

                    // Build the list of DialogueNodes
                    foreach (Object item in importedConversations[key])
                    {
                        DialogueNodeLoader loader = (DialogueNodeLoader)item;

                        tempArrayList.Add(new DialogueNode(loader.NextState, loader.Text));
                    }

                    // Add the list of nodes to the conversations.
                    this.conversations.Add(key, tempArrayList);
                    this.npcStates.Add(key, null);
                    this.npcStateBubbles.Add(key, null);
                }
            }
            catch (Exception except)
            {
                Console.Error.WriteLine("Bad things happened: " + except.Message);
            }
        }

        /// <summary>
        /// Keeps DialogueManager up-to-date. Makes sure no conversations are still on going.
        /// Manages the checking of exit conditions for text bubbles.
        /// </summary>
        public void Update(GameTime gameTime, Pantheon gameReference)
        {
            LinkedListNode<TextBubble> currentNode;

            // Update the states...
            foreach (string key in this.npcStates.Keys)
            {
                if (!gameReference.currentLevel.Entities.Keys.Contains(key)) continue; // BAD, FIX

                Entity theEntity = gameReference.currentLevel.Entities[key];

                switch (this.npcStates[key])
                {
                    case DialogueManager.STATE_NONE:
                        if (this.npcStateBubbles[key] != null)
                        {
                            this.npcStateBubbles[key].isReadyForDeletion = true;
                            this.npcStateBubbles[key] = null;
                        }

                        break;

                    case DialogueManager.STATE_TALKING: // DON'T INTERRUPT, IT'S RUDE
                        if (this.npcStateBubbles[key] != null)
                        {
                            this.npcStateBubbles[key].isReadyForDeletion = true;
                            this.npcStateBubbles[key] = null;
                        }

                        break;

                    case DialogueManager.STATE_ALERT:
                        if (this.npcStateBubbles[key] == null)
                        {
                            this.npcStateBubbles[key] = new TextBubble(theEntity, "!");
                        }
                        else if (this.npcStateBubbles[key].Text != "!")
                        {
                            this.npcStateBubbles[key].isReadyForDeletion = true;
                            this.npcStateBubbles[key] = new TextBubble(theEntity, "!");
                        }

                        break;

                    case DialogueManager.STATE_TALKABLE:
                        if (this.npcStateBubbles[key] == null)
                        {
                            this.npcStateBubbles[key] = new TextBubble(theEntity, "...");
                        }
                        else if (this.npcStateBubbles[key].Text != "...")
                        {
                            this.npcStateBubbles[key].isReadyForDeletion = true;
                            this.npcStateBubbles[key] = new TextBubble(theEntity, "...");
                        }

                        break;
                }

                if(this.npcStateBubbles[key] != null)
                    this.npcStateBubbles[key].Update(gameTime, gameReference);
            }

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
            foreach (string key in this.npcStateBubbles.Keys)
            {
                if(this.npcStateBubbles[key] != null) this.npcStateBubbles[key].Draw(context, this.textFont, this.textbubbleImage);
            }

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

            // Flag that we're currently talking, it's rude to interrupt.
            this.npcStates[entityName] = DialogueManager.STATE_TALKING;

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

            if (this.conversations.Keys.Contains(firedEvent.payload["EntityKey"]))
            {


                if (this.currentConversation == null)
                {
                    this.StartConversation(entityName, entity);
                }
                else if (((DialogueNode)this.currentConversation[this.currentConversationState]).NextState == 0)
                {
                    this.EndConversation(entityName);
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
        /// Handles an event for an interaction alert, such as a "close poximity" alert or a flagged NPC.
        /// </summary>
        /// <param name="firedEvent">The incoming event.</param>
        protected void interactAlert(Event firedEvent)
        {
            string entityName = firedEvent.payload["EntityKey"];
            Entity entity = firedEvent.gameReference.currentLevel.Entities[entityName];

            try
            {
                if (this.npcStates[entityName] == DialogueManager.STATE_TALKING) return;

                if (this.conversations.Keys.Contains(entityName))
                {
                    this.npcStates[entityName] = firedEvent.payload["State"];
                }
                else
                {
                    this.npcStates.Add(entityName, firedEvent.payload["State"]);
                }
            }
            catch (KeyNotFoundException e)
            {
                //Console.Error.WriteLine("No NPC found for interaction alert.");
            }
        }

        /// <summary>
        /// Adds the ability for spontaneous conversation, such as timed text boxes or temporary text boxes.
        /// </summary>
        /// <param name="firedEvent">The incoming event.</param>
        protected void spontaneousConversation(Event firedEvent)
        {
            string entityName = firedEvent.payload["EntityKey"];
            Entity entity = firedEvent.gameReference.currentLevel.Entities[entityName];

            if (this.npcStates[entityName] == DialogueManager.STATE_TALKING) return;

            if (this.conversations.Keys.Contains(entityName))
            {
                this.npcStates[entityName] = DialogueManager.STATE_SPONTANEOUS;
                this.npcStateBubbles[entityName].isReadyForDeletion = true;
                this.npcStateBubbles[entityName] = new TextBubble(entity, firedEvent.payload["Text"]);
            }
        }

        /// <summary>
        /// Ends all current conversations and resets the conversation handles and anchors.
        /// </summary>
        public void EndConversation(string entityName)
        {
            this.currentConversation = null;
            this.currentConversationState = 0;

            // If there is an active text bubble, remember to kill it before nulling.
            if (this.currentConversationBubble != null)
            {
                this.currentConversationBubble.isReadyForDeletion = true;
                this.currentConversationBubble = null;
            }

            // Switch the NPC's talking state off.
            this.npcStates[entityName] = DialogueManager.STATE_NONE;
        }
    }
}
