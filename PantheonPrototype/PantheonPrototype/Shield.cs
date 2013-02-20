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
    ///  A basic implementation of the shield.
    ///  
    /// </summary>
    class Shield : Item
    {
        protected Texture2D shieldTexture;

        public Texture2D ShieldTexture
        {
            get { return shieldTexture; }
        }

        public Shield(ContentManager Content) 
        {
            shieldTexture = Content.Load<Texture2D>("Shield");
        }
        /// <summary>
        /// Turn the shield on.
        /// 
        /// Currently just draws the sheild. The sheild resources are handled in PlayerCharacter and CharcaterEntity.
        /// This may change in the future.
        /// </summary>
        /// <param name="gameReference">A reference so we can see where everything is.</param>
        /// <param name="holder">A reference to the character holding the weapon.</param>
        public override void activate(Pantheon gameReference, CharacterEntity holder)
        {
            base.activate(gameReference, holder);

            //Draw(gameReference.spriteBatch, holder);
        }

        
        
    }
}
