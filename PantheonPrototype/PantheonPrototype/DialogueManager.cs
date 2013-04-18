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
        // CLASS CONSTANTS --
        public const string STATE_NONE     = "NONE";
        public const string STATE_ALERT    = "ALERT";
        public const string STATE_TALKABLE = "ANTIAPHASIA";

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
            
            gameReference.EventManager.register("Interaction", this.interactionEventHandler);
            gameReference.EventManager.register("InteractionAlert", this.interactionAlertEventHandler);

            // Load the text bubble image.
            this.textbubbleImage = content.Load<Texture2D>("textbubble");

            // Set up some temporary dialogue...
            Random innocencegod = new Random();
            int innocence = 1;
            int innocenceFactor = innocencegod.Next(3, 20);
            int evilFactor = 20;
            StringBuilder theMessage = new StringBuilder();
            ArrayList convo = new ArrayList();

            convo.Add(new DialogueNode(innocence++, "Hello."));
            convo.Add(new DialogueNode(innocence++, "It is dangerous to go alone."));
            convo.Add(new DialogueNode(innocence++, "Here, take this."));
            convo.Add(new DialogueNode(innocence++, "         Actually.\nGerbils < Hamsters."));
            convo.Add(new DialogueNode(innocence++, "How much temporary dialogue is there? Who knows! Should be fun to find out! Lahlhalha!"));
            convo.Add(new DialogueNode(innocence++, "You are stuck here!\n(Not really...)\n"));
            convo.Add(new DialogueNode(innocence++, "\n\nThis is offset weird and is really really really long on a couple of lines. It is a test to see the positino of dialogue bubbles.\nYes, I actually mean text bubbles, but who is counting, seriously. Some people are just so crazy. Like you, reading all this text. It's almost like you're textually sadistic. I don't know that that is a word but it certainly is now.\n\n"));
            convo.Add(new DialogueNode(innocence++, "Who knows when it will end? I sure don't!"));
            convo.Add(new DialogueNode(innocence++, "Actually, I do, you just have to sit and wait to see how long!"));

            for (int i = 0; i < innocenceFactor; ++i)
            {
                switch (innocencegod.Next(4))
                {
                    case 3:
                        theMessage.Clear();

                        switch (innocencegod.Next(50))
                        {
                            case 0: theMessage.Append("Green"); break;
                            case 1: theMessage.Append("Red"); break;
                            case 2: theMessage.Append("Black"); break;
                            case 3: theMessage.Append("Yellow"); break;
                            case 4: theMessage.Append("Purple"); break;
                            case 5: theMessage.Append("Puce"); break;
                            case 6: theMessage.Append("Orange"); break;
                            case 7: theMessage.Append("Blue"); break;
                            case 8: theMessage.Append("Starry"); break;
                            case 9: theMessage.Append("Exploding"); break;
                            case 10: theMessage.Append("Gaseous"); break;
                            case 11: theMessage.Append("Bright"); break;
                            case 12: theMessage.Append("Dark"); break;
                            case 13: theMessage.Append("Keen"); break;
                            case 14: theMessage.Append("Funky"); break;
                            case 15: theMessage.Append("Cool"); break;
                            case 16: theMessage.Append("Dirty"); break;
                            case 17: theMessage.Append("Slimey"); break;
                            case 18: theMessage.Append("Wet"); break;
                            case 19: theMessage.Append("White"); break;
                            case 20: theMessage.Append("Gross"); break;
                            case 21: theMessage.Append("Falling"); break;
                            case 22: theMessage.Append("Raining"); break;
                            case 23: theMessage.Append("Wooden"); break;
                            case 24: theMessage.Append("Stellar"); break;
                            case 25: theMessage.Append("Stinky"); break;
                            case 26: theMessage.Append("Pink"); break;
                            case 27: theMessage.Append("Annoying"); break;
                            case 28: theMessage.Append("Jedi"); break;
                            case 29: theMessage.Append("Grown"); break;
                            case 30: theMessage.Append("Inappropriate"); break;
                            case 31: theMessage.Append("Bloody"); break;
                            case 32: theMessage.Append("Random"); break;
                            case 33: theMessage.Append("Dead"); break;
                            case 34: theMessage.Append("Sad"); break;
                            case 35: theMessage.Append("Happy"); break;
                            case 36: theMessage.Append("Sparkly"); break;
                            case 37: theMessage.Append("Real Deal"); break;
                            case 38: theMessage.Append("Acidic"); break;
                            case 39: theMessage.Append("Fuzzy"); break;
                            case 40: theMessage.Append("Serious"); break;
                            case 41: theMessage.Append("Spotty"); break;
                            case 42: theMessage.Append("Hungry"); break;
                            case 43: theMessage.Append("Raging"); break;
                            case 44: theMessage.Append("Flaming"); break;
                            case 45: theMessage.Append("Normalized"); break;
                            case 46: theMessage.Append("Arbitrary"); break;
                            case 47: theMessage.Append("Large"); break;
                            case 48: theMessage.Append("Small"); break;
                            case 49: theMessage.Append("Rare"); break;
                            default: theMessage.Append("WHAT"); break;
                        }

                        theMessage.Append(" ");

                        switch (innocencegod.Next(10))
                        {
                            case 0: theMessage.Append("Orange"); break;
                            case 1: theMessage.Append("Golems"); break;
                            case 2: theMessage.Append("People"); break;
                            case 3: theMessage.Append("Hippie"); break;
                            case 4: theMessage.Append("Dinosaurs"); break;
                            case 5: theMessage.Append("Pearls"); break;
                            case 6: theMessage.Append("Dog"); break;
                            case 7: theMessage.Append("Cat"); break;
                            case 8: theMessage.Append("Tumbler"); break;
                            case 9: theMessage.Append("Catapult"); break;
                            case 10: theMessage.Append("Rocket Launcher"); break;
                            case 11: theMessage.Append("Barbie"); break;
                            case 12: theMessage.Append("Tanks"); break;
                            case 13: theMessage.Append("Cheese"); break;
                            case 14: theMessage.Append("Books"); break;
                            case 15: theMessage.Append("Building"); break;
                            case 16: theMessage.Append("Sockets"); break;
                            case 17: theMessage.Append("Television"); break;
                            case 18: theMessage.Append("Animal"); break;
                            case 19: theMessage.Append("Dragons"); break;
                            case 20: theMessage.Append("Window"); break;
                            case 21: theMessage.Append("Protoss"); break;
                            case 22: theMessage.Append("Integers"); break;
                            case 23: theMessage.Append("Kitchen"); break;
                            case 24: theMessage.Append("Tree"); break;
                            case 25: theMessage.Append("Carpets"); break;
                            case 26: theMessage.Append("Time Capsule"); break;
                            case 27: theMessage.Append("Airplanes"); break;
                            case 28: theMessage.Append("Coyotes"); break;
                            case 29: theMessage.Append("Programers"); break;
                            case 30: theMessage.Append("Engineers"); break;
                            case 31: theMessage.Append("Shield"); break;
                            case 32: theMessage.Append("Parties"); break;
                            case 33: theMessage.Append("Hands"); break;
                            case 34: theMessage.Append("Clothes"); break;
                            case 35: theMessage.Append("Pencils"); break;
                            case 36: theMessage.Append("Phone"); break;
                            case 37: theMessage.Append("Faucet"); break;
                            case 38: theMessage.Append("Computers"); break;

                        }
                        break;

                    case 2:
                        theMessage.Clear();

                        switch (innocencegod.Next(11))
                        {
                            case 0: theMessage.Append("The force will be with you, always."); break;
                            case 1: theMessage.Append("I AM IRON MAN."); break;
                            case 2: theMessage.Append("Who let the dogs out?"); break;
                            case 3: theMessage.Append("Jump off a bridge, all the cool nudes are doing it!"); break;
                            case 4: theMessage.Append("What goes around comes around."); break;
                            case 5: theMessage.Append("     EVERYBODY DANCE NOW.*\n\n\n\n\n*Please regard LeTU student guidelines and policies."); break;
                            case 6: theMessage.Append("EXTERMINATE"); break;
                            case 7: theMessage.Append("Dragon free!"); break;
                            case 8: theMessage.Append("I AM YOUR FATHER."); break;
                            case 9: theMessage.Append("Why can't I kill you?"); break;
                            case 10: theMessage.Append("Where does he get those wonderful toys?"); break;
                        }
                        break;

                    case 1:
                        theMessage.Clear();

                        switch (innocencegod.Next(5))
                        {
                            case 0: theMessage.Append("Sometimes "); break;
                            case 1: theMessage.Append("All the time "); break;
                            default: theMessage.Append(""); break;
                        }

                        theMessage.Append("I " + (innocencegod.Next(2) > 0 ? "like " : "hate ") + "to ");

                        switch (innocencegod.Next(12))
                        {
                            case 0: theMessage.Append("count."); break;
                            case 1: theMessage.Append("play videogames!"); break;
                            case 2: theMessage.Append("dance! *gasp*"); break;
                            case 3: theMessage.Append("do science!"); break;
                            case 4: theMessage.Append("DO LEGGGGGOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOS"); break;
                            case 5: theMessage.Append("punchcat."); break;
                            case 6: theMessage.Append("do math."); break;
                            case 7: theMessage.Append("read."); break;
                            case 8: theMessage.Append("clean."); break;
                            case 9: theMessage.Append("watch movies."); break;
                            case 10: theMessage.Append("listen to music."); break;
                            case 11: theMessage.Append("get money!"); break;
                        }
                        break;

                    case 0:
                        theMessage.Clear();

                        switch(innocencegod.Next(9))
                        {
                            case 0: theMessage.Append("I"); break;
                            case 1: theMessage.Append("My brother"); break;
                            case 2: theMessage.Append("My sister"); break;
                            case 3: theMessage.Append("My father"); break;
                            case 4: theMessage.Append("My mother"); break;
                            case 5: theMessage.Append("My friend"); break;
                            case 6: theMessage.Append("My cousin's friend's roommate's brother's father's second cousin's spouse"); break;
                            case 7: theMessage.Append("Bob"); break;
                            case 8: theMessage.Append("Darth Vader"); break;
                            default: theMessage.Append("Sung Gy"); break;
                        }

                        theMessage.Append(" likes ");

                        switch (innocencegod.Next(21))
                        {
                            case 0: theMessage.Append("trains."); break;
                            case 1: theMessage.Append("not cats."); break;
                            case 2: theMessage.Append("dogs."); break;
                            case 3: theMessage.Append("DotA."); break;
                            case 4: theMessage.Append("Counter-Strike."); break;
                            case 5: theMessage.Append("grass."); break;
                            case 6: theMessage.Append("blocks."); break;
                            case 7: theMessage.Append("cords."); break;
                            case 8: theMessage.Append("eggs."); break;
                            case 9: theMessage.Append("clothes."); break;
                            case 10: theMessage.Append("parchment."); break;
                            case 11: theMessage.Append("electricty."); break;
                            case 12: theMessage.Append("EXPLOSIONS."); break;
                            case 13: theMessage.Append("riots."); break;
                            case 14: theMessage.Append("fire."); break;
                            case 15: theMessage.Append("beds."); break;
                            case 16: theMessage.Append("something."); break;
                            case 17: theMessage.Append("sports."); break;
                            case 18: theMessage.Append("the Bible."); break;
                            case 19: theMessage.Append("art."); break;
                            case 20: theMessage.Append("CHAOS\nNASKJGN:>?LKJ>::ASN}{++_+LKJAGLNKJSANLKGJNA\"?:>}}GNLKJASNJGJNCNM<XNVIUWENTOI@UTJNA:WLFN:AW???????OI:GWEMN:AGE"); break;
                        }
                        break;

                    default:
                        theMessage.Clear();
                        theMessage.Append("WHAT YOU TALKIN' 'BOUT WILLIS?");

                        break;
                }

                convo.Add(new DialogueNode(innocence + i, theMessage.ToString()));
            }

            if (innocencegod.Next(100) < evilFactor) 
                convo.Add(new DialogueNode((innocence + innocenceFactor) - innocencegod.Next(10) - innocencegod.Next((int)(innocenceFactor/2)) + 1, "Almost there!"));
            else
                convo.Add(new DialogueNode(0, "Goodbye!"));

            this.conversations.Add("FriendtheOldMan", convo);
            this.npcStates.Add("FriendtheOldMan", DialogueManager.STATE_NONE);
            this.npcStateBubbles.Add("FriendtheOldMan", null);
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
                // if(this.npcStateBubbles[key] != null) this.npcStateBubbles[key].Draw(context, this.textFont, this.textbubbleImage);
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
                this.npcStates[entityName] = DialogueManager.STATE_NONE;

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
        /// Handles an event for an interaction alert, such as a "close poximity" alert or a flagged NPC.
        /// </summary>
        /// <param name="firedEvent">The incoming event.</param>
        protected void interactAlert(Event firedEvent)
        {
            string entityName = firedEvent.payload["EntityKey"];
            Entity entity = firedEvent.gameReference.currentLevel.Entities[entityName];

            if (this.conversations.Keys.Contains(entityName))
            {
                this.npcStates[entityName] = firedEvent.payload["State"];
            }
            else
            {
                this.npcStates.Add(entityName, firedEvent.payload["State"]);
            }
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
