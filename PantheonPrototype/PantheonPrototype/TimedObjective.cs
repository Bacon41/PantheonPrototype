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
    /// A variation of an Objective. The big difference is that a timed objective
    /// is updated every frame.
    /// </summary>
    class TimedObjective : Objective
    {
        /// <summary>
        /// Updates time sensitive data in the timed objective.
        /// </summary>
        /// <param name="gameTime">Time since the last update cycle.</param>
        public virtual void Update(GameTime gameTime)
        {
        }
    }
}
