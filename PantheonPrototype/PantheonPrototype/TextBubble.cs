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
    public class TextBubble
    {
        // VARIABLE DECLARATION --
        protected bool readyForDeletion;
        protected string text;
        protected Vector2 position;
        protected Entity entity;
        protected Texture2D bubbleImage;

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
        public void Draw(SpriteBatch context, SpriteFont textFont, Texture2D textbubbleImage)
        {
            Vector2 newPosition = Vector2.Zero;
            Vector2 measurement = textFont.MeasureString(this.text);

            newPosition.X = this.position.X - measurement.X/2;
            newPosition.Y = this.position.Y - measurement.Y + 5;

            // Draw the text bubble thingy.
            // -- Corners
            Rectangle cornerRect = new Rectangle(0, 0, 10, 10);
            context.Draw(textbubbleImage, new Rectangle((int)(newPosition.X - cornerRect.Width), (int)(newPosition.Y - cornerRect.Height), (int)cornerRect.Width, (int)cornerRect.Height), cornerRect, Color.White, 0.0f, new Vector2(cornerRect.Width / 2, cornerRect.Height / 2), SpriteEffects.None, 0.0f);
            context.Draw(textbubbleImage, new Rectangle((int)(newPosition.X - cornerRect.Width), (int)(newPosition.Y + measurement.Y), (int)cornerRect.Width, (int)cornerRect.Height), cornerRect, Color.White, -(float)(Math.PI / 2), new Vector2(cornerRect.Width / 2, cornerRect.Height / 2), SpriteEffects.None, 0.0f);
            context.Draw(textbubbleImage, new Rectangle((int)(newPosition.X + measurement.X + cornerRect.Width), (int)(newPosition.Y - cornerRect.Height), (int)cornerRect.Width, (int)cornerRect.Height), cornerRect, Color.White, (float)(Math.PI/2), new Vector2(cornerRect.Width / 2, cornerRect.Height / 2), SpriteEffects.None, 0.0f);
            context.Draw(textbubbleImage, new Rectangle((int)(newPosition.X + measurement.X + cornerRect.Width), (int)(newPosition.Y + measurement.Y), (int)cornerRect.Width, (int)cornerRect.Height), cornerRect, Color.White, (float)(Math.PI), new Vector2(cornerRect.Width / 2, cornerRect.Height / 2), SpriteEffects.None, 0.0f);

            // -- Horizontal Sides
            Rectangle hSideRect = new Rectangle(10, 0, 120, 10);

            // -- Vertical Sides
            Rectangle vSideRect = new Rectangle(0, 10, 10, 34);

            // -- Tail
            Rectangle tailRect = new Rectangle(20, 20, 64, 47);

            // -- The text!
            context.DrawString(textFont, this.text, newPosition, Color.WhiteSmoke);
        }
    }
}
