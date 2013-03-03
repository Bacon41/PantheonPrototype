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
    class EnemyCharacter : CharacterEntity
    {
        /// <summary>
        /// The time until the NPC needs to change directions for their random movement.
        /// </summary>
        protected TimeSpan changeDirection;

        public EnemyCharacter(Vector2 location, Rectangle drawBox, Rectangle boundingBox): base(location, drawBox, boundingBox)
        {
        }
    }
}
