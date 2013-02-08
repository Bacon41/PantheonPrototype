using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FuncWorks.XNA.XTiled {
    /// <summary>
    /// A sequence of lines that form a closed shape
    /// </summary>
    public class Polygon {
        /// <summary>
        /// A texture representing the polygon, must be first created by calling GenerateTexure
        /// </summary>
        public Texture2D Texture;
        /// <summary>
        /// The points that make up the polygon, in order
        /// </summary>
        public Point[] Points;
        /// <summary>
        /// The lines that make up the polygon, in order
        /// </summary>
        public Line[] Lines;
        /// <summary>
        /// Bounding rectangle of this Polygon
        /// </summary>
        public Rectangle Bounds;

        /// <summary>
        /// Creates a Polygon from a list of points and calculates the lines and bounds of the result
        /// </summary>
        /// <param name="points">The list of points that define the polygon; the last point must be the same as the first to close the polygon</param>
        /// <returns>a Polygon object</returns>
        public static Polygon FromPoints(IEnumerable<Point> points) {
            Polygon poly = new Polygon();
            poly.Points = points.ToArray();

            poly.Bounds.X = points.Min(x => x.X);
            poly.Bounds.Y = points.Min(x => x.Y);
            poly.Bounds.Width = points.Max(x => x.X) - points.Min(x => x.X);
            poly.Bounds.Height = points.Max(x => x.Y) - points.Min(x => x.Y);

            poly.Points = points.ToArray();
            if (poly.Points.Length > 1) {
                poly.Lines = new Line[poly.Points.Length - 1];
                for (int i = 0; i < poly.Lines.Length; i++) {
                    poly.Lines[i] = Line.FromPoints(
                        new Vector2(poly.Points[i].X, poly.Points[i].Y),
                        new Vector2(poly.Points[i + 1].X, poly.Points[i + 1].Y));
                }
            }

            return poly;
        }

        /// <summary>
        /// Initializes the Polygon.Texture field
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice to create the texture on</param>
        /// <param name="color">Color for the polygon</param>
        public void GenerateTexture(GraphicsDevice graphicsDevice, Color color) {
            Color[] colorData = new Color[this.Bounds.Width * this.Bounds.Height];
            for (int y = this.Bounds.Y; y < this.Bounds.Bottom; y++) {
                for (int x = this.Bounds.X; x < this.Bounds.Right; x++) {
                    int c = (x - this.Bounds.X) +
                        (y - this.Bounds.Y) * this.Bounds.Width;
                    if (this.Contains(new Vector2(x, y)))
                        colorData[c] = color;
                    else
                        colorData[c] = Color.Transparent;
                }
            }

            this.Texture = new Texture2D(graphicsDevice, this.Bounds.Width, this.Bounds.Height, false, SurfaceFormat.Color);
            this.Texture.SetData(colorData);
        }

        /// <summary>
        /// Draws the lines that make up the Polygon
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="region">Region of the map in pixels to draw</param> 
        /// <param name="texture">A texture to use in drawing the lines</param>
        /// <param name="lineWidth">The width of the lines in pixels</param>
        /// <param name="color">The color value to apply to the given texture</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        public void Draw(SpriteBatch spriteBatch, Rectangle region, Texture2D texture, Single lineWidth, Color color, Single layerDepth) {
            for (int i = 0; i < Lines.Length; i++)
                Line.Draw(spriteBatch, Lines[i], region, texture, lineWidth, color, layerDepth);
        }

        /// <summary>
        /// Draws the lines that make up the Polygon
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="region">Region of the map in pixels to draw</param> 
        /// <param name="texture">A texture to use in drawing the lines</param>
        /// <param name="lineWidth">The width of the lines in pixels</param>
        /// <param name="color">The color value to apply to the polgon lines</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        /// <param name="fillColor">The color value to fill the polygon</param>
        public void DrawFilled(SpriteBatch spriteBatch, Rectangle region, Texture2D texture, Single lineWidth, Color color, Color fillColor, Single layerDepth) {
            for (int i = 0; i < Lines.Length; i++)
                Line.Draw(spriteBatch, Lines[i], region, texture, lineWidth, color, layerDepth);

            spriteBatch.Draw(this.Texture, Map.Translate(this.Bounds, region), null, fillColor, 0, Vector2.Zero, SpriteEffects.None, layerDepth);
        }

        /// <summary>
        /// Determines if a Vector2 is inside this Polygon
        /// </summary>
        /// <param name="vector">Vector2 to compare to</param>
        /// <returns>True if the Vector2 is inside the Polygon</returns>
        public bool Contains(ref Vector2 vector) {
            bool result = false;

            // modified method from http://stackoverflow.com/questions/2379818/does-xna-have-a-polygon-like-rectangle
            // fixed bugs by following http://funplosion.com/devblog/collision-detection-line-vs-point-circle-and-rectangle.html
            foreach (var side in Lines) {
                if (vector.Y > Math.Min(side.Start.Y, side.End.Y))
                    if (vector.Y <= Math.Max(side.Start.Y, side.End.Y))
                        if (vector.X <= Math.Max(side.Start.X, side.End.X)) {
                            if (side.Start.Y != side.End.Y) {
                                float xIntersection = (vector.Y - side.Start.Y) * (side.End.X - side.Start.X) / (side.End.Y - side.Start.Y) + side.Start.X;
                                if (side.Start.X == side.End.X || vector.X <= xIntersection)
                                    result = !result;
                            }
                        }
            }

            return result;
        }

        /// <summary>
        /// Determines if a Vector2 is inside this Polygon
        /// </summary>
        /// <param name="vector">Vector2 to compare to</param>
        /// <returns>True if the Vector2 is inside the Polygon</returns>
        public bool Contains(Vector2 vector) {
            return Contains(ref vector);
        }

        /// <summary>
        /// Determines if a Point is inside this Polygon
        /// </summary>
        /// <param name="point">Point to compare to</param>
        /// <returns>True if the Point is inside the Polygon</returns>
        public bool Contains(ref Point point) {
            Vector2 v = new Vector2(point.X, point.Y);
            return Contains(ref v);
        }

        /// <summary>
        /// Determines if a Point is inside this Polygon
        /// </summary>
        /// <param name="point">Point to compare to</param>
        /// <returns>True if the Point is inside the Polygon</returns>
        public bool Contains(Point point) {
            return Contains(ref point);
        }

        /// <summary>
        /// Determines if a Line is inside this Polygon; a Line is considered inside if both Start and End points are inside
        /// </summary>
        /// <param name="line">Line to compare to</param>
        /// <returns>True if the Line is inside the Polygon</returns>
        public bool Contains(ref Line line) {
            return Contains(ref line.Start) && Contains(ref line.End);
        }

        /// <summary>
        /// Determines if a Line is inside this Polygon; a Line is considered inside if both Start and End points are inside
        /// </summary>
        /// <param name="line">Line to compare to</param>
        /// <returns>True if the Line is inside the Polygon</returns>
        public bool Contains(Line line) {
            return Contains(ref line);
        }

        /// <summary>
        /// Determines if a Rectangle is inside this Polygon; a Rectangle is considered inside if all four corners are inside
        /// </summary>
        /// <param name="rect">Rectangle to compare to</param>
        /// <returns>True if the Rectangle is inside the Polygon</returns>
        public bool Contains(ref Rectangle rect) {
            return Contains(new Vector2(rect.Left, rect.Top)) &&
                   Contains(new Vector2(rect.Left, rect.Bottom)) &&
                   Contains(new Vector2(rect.Right, rect.Top)) &&
                   Contains(new Vector2(rect.Right, rect.Bottom));
        }

        /// <summary>
        /// Determines if a Rectangle is inside this Polygon; a Rectangle is considered inside if all four corners are inside
        /// </summary>
        /// <param name="rect">Rectangle to compare to</param>
        /// <returns>True if the Rectangle is inside the Polygon</returns>
        public bool Contains(Rectangle rect) {
            return Contains(ref rect);
        }


        /// <summary>
        /// Determines if a Rectangle intersects this Polygon; a Rectangle is considered to intersect if at least one corner is contained but not all four corners are contained
        /// </summary>
        /// <param name="rect">Rectangle to compare to</param>
        /// <returns>True if the Rectangle is intersects the Polygon</returns>
        public bool Intersects(ref Rectangle rect) {
            int pointsContained = ((Contains(new Vector2(rect.Left, rect.Top)) ? 1 : 0) +
                (Contains(new Vector2(rect.Left, rect.Bottom)) ? 1 : 0) +
                (Contains(new Vector2(rect.Right, rect.Top)) ? 1 : 0) +
                (Contains(new Vector2(rect.Right, rect.Bottom)) ? 1 : 0));

            return pointsContained > 0 && pointsContained < 4;
        }

        /// <summary>
        /// Determines if a Rectangle intersects this Polygon; a Rectangle is considered to intersect if at least one corner is contained but not all four corners are contained
        /// </summary>
        /// <param name="rect">Rectangle to compare to</param>
        /// <returns>True if the Rectangle is intersects the Polygon</returns>
        public bool Intersects(Rectangle rect) {
            return Intersects(ref rect);
        }

        /// <summary>
        /// Determines if a Line intersects this Polygon; a Line is considered to intersect if one but not both points are contained
        /// </summary>
        /// <param name="line">Line to compare to</param>
        /// <returns>True if the Line is intersects the Polygon</returns>
        public bool Intersects(ref Line line) {
            int pointsContained = ((Contains(line.Start) ? 1 : 0) + (Contains(line.End) ? 1 : 0));
            return pointsContained == 1;
        }

        /// <summary>
        /// Determines if a Line intersects this Polygon; a Line is considered to intersect if one but not both points are contained
        /// </summary>
        /// <param name="line">Line to compare to</param>
        /// <returns>True if the Line is intersects the Polygon</returns>
        public bool Intersects(Line line) {
            return Intersects(ref line);
        }
    }
}
