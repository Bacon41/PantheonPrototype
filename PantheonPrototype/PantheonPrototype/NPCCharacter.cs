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
    class NPCCharacter : CharacterEntity
    {
        public NPCCharacter(): base(Vector2.Zero, new Rectangle(0, 0, 40, 40), new Rectangle(15, 25, 10, 10))
        {
        }
    }
}
