using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FuncWorks.XNA.XTiled;

namespace PantheonPrototype
{
    public class HamburgerHelper
    {
        /// <summary>
        /// The method to draw the lines for the level edging.
        /// </summary>
        /// <param name="batch">The object that allows the drawing.</param>
        /// <param name="blank">A Texture2D for iteration over the line.</param>
        /// <param name="width">The thickness of the line.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="point1">The start point for the line.</param>
        /// <param name="point2">The end point for the line.</param>
        public static void DrawLine(SpriteBatch batch, Texture2D blank, float width, Color color, Vector2 point1, Vector2 point2)
        {
            // The angle between the two points (to get the rotation of the texture) and the distance between them
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);
            blank.SetData(new[] { color });

            // Drawing the empty texture from point1 to point2
            batch.Draw(blank, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0.2f);
        }
    }
}
