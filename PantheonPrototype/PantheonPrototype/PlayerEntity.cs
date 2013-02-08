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
    /// This is the visual element of the player. It extends the Character
    /// Entity.
    /// </summary>
    class PlayerEntity : CharacterEntity
    {
        /// <summary>
        /// Load the player entity.
        /// </summary>
        /// <param name="contentManager">Read the parameter name... that's what it is.</param>
        public override void Load(ContentManager contentManager)
        {
            base.Load(contentManager);
        }

        /// <summary>
        /// Updates the player entity.
        /// 
        /// Unlike the name suggests... (What did you think it did?)
        /// </summary>
        /// <param name="gameTime">The time object that lets the Update object know how old it is.</param>
        /// <param name="gameReference">A reference to the entire game universe for the purpose of making the player feel small.</param>
        public override void Update(GameTime gameTime, Pantheon gameReference)
        {
            base.Update(gameTime, gameReference);
        }

        /// <summary>
        /// Draw the player.
        /// </summary>
        /// <param name="canvas">The sprite batch to which the player will be drawn.</param>
        public override void Draw(SpriteBatch canvas)
        {
            base.Draw(canvas);
        }
    }
}
