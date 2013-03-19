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
        protected string text;
        protected int nextState;
        protected TextBubble textBubble;

        public int NextState
        {
            get { return this.nextState; }
        }

        public DialogueNode(int nextState, string text)
        {
            this.nextState = nextState;
            this.text = text;
        }

        public override bool ShouldContinueRunning()
        {
            if (textBubble != null || textBubble.isReadyForDeletion) return false;
            else return true;
        }

        /// <summary>
        /// Executes whatever processing the dialogue node needs.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Creates and returns the text bubble for this conversation node.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override TextBubble GetTextBubble(Vector2 position)
        {
            this.textBubble = new TextBubble(position, text);

            return this.textBubble;
        }
    }
}
