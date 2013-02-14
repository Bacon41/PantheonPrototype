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
    class OldManNPC : NPCCharacter
    {
        /// <summary>
        /// The constuctor for the OldMan NPC character. Takes care of setting up the NPCCharacter base class.
        /// </summary>
        /// <param name="location">The initial location of the Old Man!</param>
        public OldManNPC(Vector2 location)
            : base(location, new Rectangle(0, 0, 40, 40), new Rectangle(15, 25, 10, 10))
        {
        }
    }
}
