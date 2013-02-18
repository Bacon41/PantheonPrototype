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
            batch.Draw(blank, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0.00001f);
        }

        /// <summary>
        /// Reduces the passed 0 to 2pi degree angle to one of 8 directions.
        /// </summary>
        /// <param name="angle">An angle between 0 and 2pi</param>
        /// <returns>A value in the Direction enumeration.</returns>
        public static Direction reduceAngle(double angle)
        {
            if (angle > Math.PI / 8 && angle < 3 * Math.PI / 8)
            {
                return Direction.backLeft;
            }

            else if (angle > 3 * Math.PI / 8 && angle < 5 * Math.PI / 8)
            {
                return Direction.back;
            }

            else if (angle > 5 * Math.PI / 8 && angle < 7 * Math.PI / 8)
            {
                return Direction.backRight;
            }

            else if (angle > 7 * Math.PI / 8 && angle < 9 * Math.PI / 8)
            {
                return Direction.Right;
            }
            else if (angle > 9 * Math.PI / 8 && angle < 11 * Math.PI / 8)
            {
                return Direction.forwardRight;
            }
            else if (angle > 11 * Math.PI / 8 && angle < 13 * Math.PI / 8)
            {
                return Direction.forward;
            }
            else if (angle > 13 * Math.PI / 8 && angle < 15 * Math.PI / 8)
            {
                return Direction.forwardLeft;
            }
            else
            {
                return Direction.Left;
            }
        }

        /// <summary>
        /// Reduces the passed vector into one of 8 directions.
        /// </summary>
        /// <param name="direction">A vector indicating the direction to reduce.</param>
        /// <returns>A value in the Direction enumeration.</returns>
        public static Direction reduceAngle(Vector2 direction)
        {
            double angle = Math.Atan2(direction.Y, direction.X) + Math.PI;

            return reduceAngle(angle);
        }
    }
}
