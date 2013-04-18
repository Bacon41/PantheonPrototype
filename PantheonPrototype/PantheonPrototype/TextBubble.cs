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
        // CLASS CONST/STATIC DECLARATION --
        public const int MINIMUM_WIDTH = 5;
        // VARIABLE DECLARATION --
        protected bool readyForDeletion;
        protected string text;
        protected Vector2 position;
        protected Entity entity;
        protected Texture2D bubbleImage;

        // ACCESSORS --
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

        public string Text
        {
            get { return this.text; }
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

            if (this.text.Length < TextBubble.MINIMUM_WIDTH)
            {
                int padding = (int)((TextBubble.MINIMUM_WIDTH - this.text.Length) / 2);

                this.text.PadRight(this.text.Length + padding);
                this.text.PadLeft(this.text.Length + padding);
            }
        }

        // METHOD AND FUNCTION DEFINITION --
        /// <summary>
        /// IT CREATES A TEXT BUBBLE. WOAH.
        /// </summary>
        /// <param name="position">The location of the text bubble anchor.</param>
        /// <param name="text">This text to say.</param>
        public TextBubble(Vector2 position, string text, Texture2D image)
            : this(position, text)
        {
            this.bubbleImage = image;
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

            if (this.text.Length < TextBubble.MINIMUM_WIDTH)
            {
                this.text = this.text.PadLeft((int)(this.text.Length + (TextBubble.MINIMUM_WIDTH - this.text.Length) / 2));
                this.text = this.text.PadRight(TextBubble.MINIMUM_WIDTH);
            }

            // Set the position of the text bubble based on the position of the character.
            this.position = this.entity.Location;
            this.position.Y = this.position.Y - (float)(this.entity.BoundingBox.Height);
        }

        public TextBubble(Entity entity, string text, Texture2D image)
            : this(entity, text)
        {
            this.bubbleImage = image;
        }

        /// <summary>
        /// IT CREATES A TEXT BUBBLE. WOAH. Also anchors it to a character.
        /// NOTE: At the moment this will just hook it to the character at that current point,
        /// it will not follow, while that could be possible in the future.
        /// </summary>
        /// <param name="entityName">The entity to tie the text bubble to.</param>
        /// <param name="text">This text to say.</param>
        public TextBubble(Entity entity, string text, ContentManager content, string imageName)
            : this(entity, text)
        {
            this.Load(content, imageName);
        }

        /// <summary>
        /// Loads the image for use as a textbubble.
        /// </summary>
        /// <param name="content">The content manager to load the image from.</param>
        /// <param name="imageName">The name of the image to load.</param>
        public void Load(ContentManager content, string imageName)
        {
            this.bubbleImage = content.Load<Texture2D>(imageName);
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
                this.position.Y = this.position.Y - (float)(this.entity.DrawingBox.Height);
            }
        }

        /// <summary>
        /// Draws the text bubble on the screen at its given position.
        /// </summary>
        /// <param name="context">The SpriteBatch the bubble should be drawn on.</param>
        /// <param name="textFont">The font used to draw the text bubbles.</param>
        public void Draw(SpriteBatch context, SpriteFont textFont, Texture2D textbubbleImage)
        {
            float depth = 0.00001f;
            Vector2 newPosition = Vector2.Zero;
            Vector2 measurement = textFont.MeasureString(this.text);

            // Drawing Rectangles (Sources)
            Rectangle cornerRect = new Rectangle(0, 0, 10, 10);
            Rectangle hSideRect = new Rectangle(11, 0, 1, 10);
            Rectangle vSideRect = new Rectangle(0, 11, 10, 1);
            Rectangle tailRect = new Rectangle(30, 54, 8, 7);
            Rectangle background = new Rectangle(11, 11, 1, 1);

            newPosition.X = this.position.X - measurement.X/2;
            newPosition.Y = this.position.Y - measurement.Y - tailRect.Height;

            // Draw the text bubble thingy.
            // -- Corners
            context.Draw(textbubbleImage, new Rectangle((int)(newPosition.X - cornerRect.Width / 2), (int)(newPosition.Y - cornerRect.Height / 2), (int)cornerRect.Width, (int)cornerRect.Height), cornerRect, Color.White, 0.0f, new Vector2(cornerRect.Width / 2, cornerRect.Height / 2), SpriteEffects.None, depth);
            context.Draw(textbubbleImage, new Rectangle((int)(newPosition.X - cornerRect.Width / 2), (int)(newPosition.Y + measurement.Y + cornerRect.Height / 2), (int)cornerRect.Width, (int)cornerRect.Height), cornerRect, Color.White, -(float)(Math.PI / 2), new Vector2(cornerRect.Width / 2, cornerRect.Height / 2), SpriteEffects.None, depth);
            context.Draw(textbubbleImage, new Rectangle((int)(newPosition.X + measurement.X + cornerRect.Width / 2), (int)(newPosition.Y - cornerRect.Height / 2), (int)cornerRect.Width, (int)cornerRect.Height), cornerRect, Color.White, (float)(Math.PI/2), new Vector2(cornerRect.Width / 2, cornerRect.Height / 2), SpriteEffects.None, depth);
            context.Draw(textbubbleImage, new Rectangle((int)(newPosition.X + measurement.X + cornerRect.Width / 2), (int)(newPosition.Y + measurement.Y + cornerRect.Height / 2), (int)cornerRect.Width, (int)cornerRect.Height), cornerRect, Color.White, (float)(Math.PI), new Vector2(cornerRect.Width / 2, cornerRect.Height / 2), SpriteEffects.None, depth);

            // -- Horizontal Sides
            context.Draw(textbubbleImage, new Rectangle((int)(newPosition.X), (int)(newPosition.Y - hSideRect.Height/2), (int)(measurement.X), (int)(hSideRect.Height)), hSideRect, Color.White, 0.0f, new Vector2(hSideRect.Width / 2, hSideRect.Height / 2), SpriteEffects.None, depth);
            context.Draw(textbubbleImage, new Rectangle((int)(newPosition.X + measurement.X), (int)(newPosition.Y + measurement.Y + hSideRect.Height/2), (int)(measurement.X), (int)(hSideRect.Height)), hSideRect, Color.White, (float)(Math.PI), new Vector2(hSideRect.Width / 2, hSideRect.Height / 2), SpriteEffects.None, depth);

            // -- Vertical Sides
            context.Draw(textbubbleImage, new Rectangle((int)(newPosition.X - vSideRect.Width / 2), (int)(newPosition.Y), (int)vSideRect.Width, (int)measurement.Y), vSideRect, Color.White, 0.0f, new Vector2(vSideRect.Width / 2, vSideRect.Height / 2), SpriteEffects.None, depth);
            context.Draw(textbubbleImage, new Rectangle((int)(newPosition.X + measurement.X + vSideRect.Width / 2), (int)(newPosition.Y + measurement.Y), (int)vSideRect.Width, (int)measurement.Y), vSideRect, Color.White, (float)(Math.PI), new Vector2(vSideRect.Width / 2, vSideRect.Height / 2), SpriteEffects.None, depth);

            // -- Tail
            context.Draw(textbubbleImage, new Rectangle((int)(this.position.X + this.entity.BoundingBox.Width * 0.35), (int)(this.position.Y + tailRect.Height / 2), (int)tailRect.Width, (int)tailRect.Height), tailRect, Color.White, 0.0f, new Vector2(tailRect.Width / 2, tailRect.Height / 2), SpriteEffects.None, depth / 2f);

            // -- Background
            context.Draw(textbubbleImage, new Rectangle((int)newPosition.X, (int)newPosition.Y, (int)measurement.X, (int)measurement.Y), background, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, depth);

            // -- The text!
            context.DrawString(textFont, this.text, newPosition, Color.Black);
        }
    }
}
