using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FuncWorks.XNA.XTiled {
    /// <summary>
    /// A list of two points that define a line
    /// </summary>
    public struct Line {
        /// <summary>
        /// The starting point of the line
        /// </summary>
        public Vector2 Start;
        /// <summary>
        /// The ending point of the line
        /// </summary>
        public Vector2 End;
        /// <summary>
        /// Length of the line
        /// </summary>
        public float Length;
        /// <summary>
        /// Rotation of the line, suitable for using with SpriteBatch
        /// </summary>
        public float Angle;

        /// <summary>
        /// Create a line from start and end points and calculate the length and angle
        /// </summary>
        /// <param name="start">The first point of the line</param>
        /// <param name="end">The end of the line</param>
        /// <returns>A Line created from the points</returns>
        public static Line FromPoints(Vector2 start, Vector2 end) {
            Line l = new Line();
            l.Start = start;
            l.End = end;
            l.Length = Convert.ToSingle(Math.Sqrt(Math.Pow(Math.Abs(start.X - end.X), 2) + Math.Pow(Math.Abs(start.Y - end.Y), 2)));
            l.Angle = Convert.ToSingle(Math.Atan2(end.Y - start.Y, end.X - start.X));
            return l;
        }

        /// <summary>
        /// Draws a Line
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="line">The Line to draw</param>
        /// <param name="region">Region of the map in pixels to draw</param> 
        /// <param name="texture">A texture to use in drawing the line</param>
        /// <param name="lineWidth">The width of the line in pixels</param>
        /// <param name="color">The color value to apply to the given texture</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        public static void Draw(SpriteBatch spriteBatch, Line line, Rectangle region, Texture2D texture, Single lineWidth, Color color, Single layerDepth) {
            Vector2 start = Map.Translate(line.Start, region);
            spriteBatch.Draw(texture, start, null, color, line.Angle, Vector2.Zero, new Vector2(line.Length, lineWidth), SpriteEffects.None, layerDepth);
        }

        /// <summary>
        /// Determines if a specificed Line intersects with this Line
        /// </summary>
        /// <param name="line">Line to compare to</param>
        /// <returns>True if the lines intersect</returns>
        public Boolean Intersects(Line line) {
            return this.Intersects(ref line);
        }

        /// <summary>
        /// Determines if a specificed Line intersects with this Line
        /// </summary>
        /// <param name="line">Line to compare to</param>
        /// <returns>True if the lines intersect</returns>
        public Boolean Intersects(ref Line line) {
            Boolean result;
            Vector2 intersection;
            Intersects(ref line, out result, out intersection);
            return result;
        }

        /// <summary>
        /// Determines if a specificed Line intersects with this Line
        /// </summary>
        /// <param name="line">Line to compare to</param>
        /// <param name="intersection">If the lines intersect this will be set to the point of intersection</param>
        /// <returns>True if the lines intersect</returns>
        public Boolean Intersects(Line line, out Vector2 intersection) {
            return Intersects(ref line, out intersection);
        }

        /// <summary>
        /// Determines if a specificed Line intersects with this Line
        /// </summary>
        /// <param name="line">Line to compare to</param>
        /// <param name="intersection">If the lines intersect this will be set to the point of intersection</param>
        /// <returns>True if the lines intersect</returns>
        public Boolean Intersects(ref Line line, out Vector2 intersection) {
            Boolean result;
            Intersects(ref line, out result, out intersection);
            return result;
        }

        /// <summary>
        /// Determines if a specificed Rectangle intersects with this Line
        /// </summary>
        /// <param name="rect">Rectangle to compare to</param>
        /// <returns>True if the Rectangle intersects</returns>
        public Boolean Intersects(Rectangle rect) {
            return this.Intersects(ref rect);
        }

        /// <summary>
        /// Determines if a specificed Rectangle intersects with this Line
        /// </summary>
        /// <param name="rect">Rectangle to compare to</param>
        /// <returns>True if the Rectangle intersects</returns>
        public Boolean Intersects(ref Rectangle rect) {
            if (this.Intersects(Line.FromPoints(new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Top))))
                return true;
            if (this.Intersects(Line.FromPoints(new Vector2(rect.Right, rect.Top), new Vector2(rect.Right, rect.Bottom))))
                return true;
            if (this.Intersects(Line.FromPoints(new Vector2(rect.Right, rect.Bottom), new Vector2(rect.Left, rect.Bottom))))
                return true;
            if (this.Intersects(Line.FromPoints(new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Left, rect.Top))))
                return true; 
            
            return false;
        }

        /// <summary>
        /// Determines if a specificed Line intersects with this Line
        /// </summary>
        /// <param name="line">Line to compare to</param>
        /// <param name="result">True if the lines intersect</param>
        /// <param name="intersection">If the lines intersect this will be set to the point of intersection</param>
        public void Intersects(ref Line line, out Boolean result, out Vector2 intersection) {
            // Method from http://paulbourke.net/geometry/lineline2d/
            // C# implementation based on version by Olaf Rabbachin (same link)
            result = false;
            intersection = Vector2.Zero;

            float d = (line.End.Y - line.Start.Y) * (this.End.X - this.Start.X) -
                      (line.End.X - line.Start.X) * (this.End.Y - this.Start.Y);

            if (d == 0) return;

            float n_a = (line.End.X - line.Start.X) * (this.Start.Y - line.Start.Y) -
                        (line.End.Y - line.Start.Y) * (this.Start.X - line.Start.X);

            float n_b = (this.End.X - this.Start.X) * (this.Start.Y - line.Start.Y) -
                        (this.End.Y - this.Start.Y) * (this.Start.X - line.Start.X);

            float ua = n_a / d;
            float ub = n_b / d;

            if (ua >= 0d && ua <= 1d && ub >= 0d && ub <= 1d) {
                intersection.X = this.Start.X + (ua * (this.End.X - this.Start.X));
                intersection.Y = this.Start.Y + (ua * (this.End.Y - this.Start.Y));
                result = true;
            }
        }

    }
}
