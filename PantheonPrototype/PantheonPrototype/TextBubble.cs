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
        int duration;
        String text;
        Vector2 position;
        System.TimeSpan endingAnchor;

        /// <summary>
        /// IT CREATES A TEXT BUBBLE. WOAH.
        /// </summary>
        /// <param name="position">The location of the text bubble anchor.</param>
        /// <param name="text">This text to say.</param>
        /// <param name="duration">How long (in milliseconds) the text bubble should last.</param>
        public TextBubble(Vector2 position, String text, int duration)
        {
            this.position = position;
            this.text = text;
            this.duration = duration;
        }

        /// <summary>
        /// This updates the bubble which mostly is sure to set up the structure defining the ending time.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (this.endingAnchor == null)
                this.endingAnchor = gameTime.TotalGameTime.Add(System.TimeSpan.FromMilliseconds(this.duration));
        }

        /// <summary>
        /// Draws the text bubble on the screen at its given position.
        /// </summary>
        /// <param name="context">The SpriteBatch the bubble should be drawn on.</param>
        public void Draw(SpriteBatch context)
        {
            // context.DrawString();
        }

        /// <summary>
        /// ReadyForDeletion checks to see if this text bubble is ready to be deleted.
        /// If the duration is set to zero, or less, the text bubble must be
        /// removed manually.
        /// </summary>
        /// <param name="currentGameTime">The current in-game time.</param>
        /// <returns>
        /// If the text bubble should be deleted (true) or not (false).
        /// Will return false if duration was set to 0, or if the ending time has not be initialized.
        /// </returns>
        public bool ReadyForDeletion(GameTime currentGameTime)
        {
            if (this.endingAnchor == null) return false;

            if (currentGameTime.TotalGameTime >= this.endingAnchor && this.duration > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Sets the text bubble to be deleted by any managers that are using it.
        /// </summary>
        public void Delete()
        {
            this.duration = -1;
        }
    }
}
