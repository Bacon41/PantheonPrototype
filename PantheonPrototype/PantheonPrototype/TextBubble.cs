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
    class TextBubble
    {
        protected bool readyForDeletion;
        protected String text;
        protected Vector2 position;

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public bool isReadyForDeletion
        {
            get { return this.readyForDeletion; }
            set { this.readyForDeletion = value; }
        }

        /// <summary>
        /// IT CREATES A TEXT BUBBLE. WOAH.
        /// </summary>
        /// <param name="position">The location of the text bubble anchor.</param>
        /// <param name="text">This text to say.</param>
        /// <param name="duration">How long (in milliseconds) the text bubble should last.</param>
        public TextBubble(Vector2 position, String text)
        {
            this.position = position;
            this.text = text;
        }

        /// <summary>
        /// This updates the bubble which mostly is sure to set up the structure defining the ending time.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Draws the text bubble on the screen at its given position.
        /// </summary>
        /// <param name="context">The SpriteBatch the bubble should be drawn on.</param>
        /// <param name="textFont">The font used to draw the text bubbles.</param>
        public void Draw(SpriteBatch context, SpriteFont textFont)
        {
            Vector2 newPosition = Vector2.Zero;
            Vector2 measurement = textFont.MeasureString(this.text);

            newPosition.X = this.position.X - measurement.X/2;
            newPosition.Y = this.position.Y - measurement.Y + 5;

            context.DrawString(textFont, this.text, newPosition, Color.WhiteSmoke);
        }
    }
}
