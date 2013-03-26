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
        // VARIABLE DECLARATION --
        protected bool readyForDeletion;
        protected string text;
        protected Vector2 position;
        protected Entity entity;

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        // ACCESSORS --
        public bool isReadyForDeletion
        {
            get { return this.readyForDeletion; }
            set { this.readyForDeletion = value; }
        }
        
        // METHOD AND FUNCTION DEFINITION --
        /// <summary>
        /// IT CREATES A TEXT BUBBLE. WOAH.
        /// </summary>
        /// <param name="position">The location of the text bubble anchor.</param>
        /// <param name="text">This text to say.</param>
        public TextBubble(Vector2 position, string text)
        {
            this.entity = null;
            this.position = position;
            this.text = text;
        }

        /// <summary>
        /// IT CREATES A TEXT BUBBLE. WOAH. Also anchors it to a character.
        /// NOTE: At the moment this will just hook it to the character at that current point,
        /// it will not follow, while that could be possible in the future.
        /// </summary>
        /// <param name="entityName">The entity to tie the text bubble to.</param>
        /// <param name="text">This text to say.</param>
        public TextBubble(Entity entity, string text)
        {
            this.entity = entity;
            this.text = text;

            // Set the position of the text bubble based on the position of the character.
            this.position = this.entity.Location;
            this.position.Y = this.position.Y - (float)(this.entity.BoundingBox.Height * 1.1);
        }

        /// <summary>
        /// This updates the bubble which mostly is sure to set up the structure defining the ending time.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, Pantheon gameReference)
        {
            // If the bubble is attached to a character, move it with the character.
            if (this.entity != null)
            {
                this.position = this.entity.Location;
                this.position.Y = this.position.Y - (float)(this.entity.DrawingBox.Height*1.1);
            }
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
