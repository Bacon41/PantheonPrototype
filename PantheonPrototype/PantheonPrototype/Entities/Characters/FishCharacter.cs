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
    class FishCharacter : NPCCharacter
    {
        /// <summary>
        /// The constructor for the fish NPC, currently only passes that variables from its constructor to the base classes constructor.
        /// </summary>
        /// <param name="location">The initialize placement of the Fish.</param>
        public FishCharacter(Vector2 location)
            : base(location, new Rectangle(0, 0, 40, 40), new Rectangle(15, 25, 10, 10))
        {
        }

        /// <summary>
        /// Update the FEESH
        /// </summary>
        /// <param name="gameTime">The game time object for letting you know how old you've gotten since starting the game.</param>
        /// <param name="gameReference">Another global game reference.</param>
        public override void Update(GameTime gameTime, Pantheon gameReference)
        {
            base.Update(gameTime, gameReference);
        }

        /// <summary>
        /// Draw the character class... and override the default behavior...
        /// take over the entity. Let none of it survive... except all of it.
        /// </summary>
        /// <param name="canvas">An initialized SpriteBatch.</param>
        public override void Draw(SpriteBatch canvas)
        {
            base.Draw(canvas);
        }
    }
}
