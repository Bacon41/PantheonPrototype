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

        /// <summary>
        /// Load the old man NPC.
        /// </summary>
        /// <param name="contentManager">Read the parameter name... that's what it is.</param>
        public override void Load(ContentManager contentManager)
        {
            base.Load(contentManager);

            Texture2D sprite;

            //Load the image
            sprite = contentManager.Load<Texture2D>("oldman");

            this.Sprite.loadSprite(sprite, 1, 1, 30);

            //Load the interaction information
            // DO IT --
            // ADD IT --
            // ETC --

            velocity = Vector2.Zero;
        }

        /// <summary>
        /// Runs the NPC's interaction 
        /// </summary>
        public override void Interact()
        {

        }

        /// <summary>
        /// Updates the OLDE MAN
        /// </summary>
        /// <param name="gameTime">SHOWS JUST HOW OLD OF A MAN HE IS.</param>
        /// <param name="gameReference">THE OLD MAN NEEDS NO SUCH REFERENCE BECAUSE HE ALREADY HAS THE WISDOM OF THE UNIVERSE.</param>
        public override void Update(GameTime gameTime, Pantheon gameReference)
        {
            base.Update(gameTime, gameReference);
        }

        /// <summary>
        /// Draw the OLD MAN.
        /// </summary>
        /// <param name="canvas">YOU SHALL SEE IN THIS BATCH JUST HOW OLD HE IS.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
