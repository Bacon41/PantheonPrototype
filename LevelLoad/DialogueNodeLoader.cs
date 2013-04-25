using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelLoad
{
    /// <summary>
    /// Contains all the characteristics necessary to load and supply a DialogueNode with the necessary attributes.
    /// </summary>
    public class DialogueNodeLoader
    {
        /// <summary>
        /// Contains the number, in array index, of the next conversation state.
        /// </summary>
        public int NextState;

        /// <summary>
        /// Contains the text spoken by the NPC.
        /// </summary>
        public string Text;
    }
}
