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
    class ButterflyEnemy : EnemyCharacter
    {
        /// <summary>
        /// UPDATE THE BUTTERFLY CLASS FOR THE WIN
        /// </summary>
        /// <param name="gameTime">The game time object for letting you know how old you've gotten since starting the game.</param>
        /// <param name="gameReference">Game reference of doom</param>
        public override void Update(GameTime gameTime, Pantheon gameReference)
        {

        }

        /// <summary>
        /// DRAW THE BUTTERFULLYMONK
        /// </summary>
        /// <param name="canvas">An initialized SpriteBatch.</param>
        public override void Draw(SpriteBatch canvas)
        {
            base.Draw(canvas);
        }
    }
}
