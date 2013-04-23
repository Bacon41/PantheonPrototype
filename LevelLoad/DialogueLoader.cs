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
using System.Collections;

namespace LevelLoad
{
    /// <summary>
    /// Contains all characteristics needed to load dialogue and move it into a DialogueManager.
    /// </summary>
    public class DialogueLoader
    {
        /// <summary>
        /// Contains the conversations for the dialogue manager to load.
        /// </summary>
        Dictionary<string, ArrayList> conversations;
    }
}
