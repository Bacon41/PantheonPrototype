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
    public class DialogueNode
    {
        // VARIABLE DECLARATION --
        protected string text;
        protected int nextState;

        // ACCESSOR DEFINITION --
        public string Text
        {
            get { return this.text; }
        }

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
        {
            this.nextState = nextState;
            this.text = text;
        }

        /// <summary>
        /// This is a function that does stuff.
        /// It will allow nodes to execute some sort of code
        /// so that expansion of the dialogue events can happen.
        /// 
        /// <param name="param">A arbitrary string for paraming things.</param>
        /// </summary>
        public void Execute(string param) {}
    }
}
