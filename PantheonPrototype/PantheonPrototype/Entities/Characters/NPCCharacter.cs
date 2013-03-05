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
        /// <summary>
        /// The time until the NPC needs to change directions for their random movement.
        /// </summary>
        protected TimeSpan changeDirection;

        public NPCCharacter(Vector2 location, Rectangle drawBox, Rectangle boundingBox): base(location, drawBox, boundingBox)
        {
        }

        public override void Update(GameTime gameTime, Pantheon gameReference)
        {
            base.Update(gameTime, gameReference);

            switch (facing)
            {
                case Direction.Left:
                    angleFacing = (float)Math.PI;
                    break;
                case Direction.Right:
                    angleFacing = 0;
                    break;
                case Direction.forward:
                    angleFacing = (float)Math.PI / 2;
                    break;
                case Direction.back:
                    angleFacing = 3 * (float)Math.PI / 2;
                    break;
            }
        }

        public virtual void Interact()
        {
        }
    }
}
