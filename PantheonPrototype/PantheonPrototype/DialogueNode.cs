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
    class DialogueNode
    {
<<<<<<< HEAD

=======
>>>>>>> origin
        // VARIABLE DECLARATION --
        protected string text;
        protected int nextState;
        protected TextBubble textBubble;

<<<<<<< HEAD
=======
        // ACCESSOR DEFINITION --
        public string Text
        {
            get { return this.text; }
        }

>>>>>>> origin
        public int NextState
        {
            get { return this.nextState; }
        }

        // METHOD AND FUNCTIOND DECLARATION --
        /// <summary>
        /// This is the constructor for a basic dialogue node.
        /// </summary>
        /// <param name="nextState">The next state that the node should go to. States start at 0 and specifying a value of 0 will end conversation.</param>
        /// <param name="text">The text in the node in question.</param>
        public DialogueNode(int nextState, string text)
<<<<<<< HEAD
        {
            this.nextState = nextState;
            this.text = text;
        }

        public virtual bool ShouldContinueRunning()
        {
            if (textBubble != null || textBubble.isReadyForDeletion) return false;
            else return true;
        }

        /// <summary>
        /// Executes whatever processing the dialogue node needs.
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Creates and returns the text bubble for this conversation node.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public virtual TextBubble GetTextBubble(Vector2 position)
        {
            this.textBubble = new TextBubble(position, text);

            return this.textBubble;
		}

        // ACCESSOR DEFINITION --
        public string Text
        {
            get { return this.text; }
=======
        {
            this.nextState = nextState;
            this.text = text;
>>>>>>> origin
        }
    }
}
