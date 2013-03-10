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
    class EnemyNPC : NPCCharacter
    {
        public EnemyNPC(Vector2 location, Rectangle drawBox, Rectangle boundingBox)
            : base(location, drawBox, boundingBox)
        {
            comfortZone = new Rectangle((int)location.X + drawBox.X - 200, (int)location.Y + drawBox.Y - 200, 400, 400);
        }
    }
}
