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

namespace CustomLoad
{
    public class EntityLoader
    {
        private Rectangle drawingBox;
        private Rectangle boundingBox;
        private Vector2 actionPoint;

        public Rectangle DrawingBox
        {
            get { return drawingBox; }
            set { drawingBox = value; }
        }
        public Rectangle BoundingBox
        {
            get { return boundingBox; }
            set { boundingBox = value; }
        }
        public Vector2 ActionPoint
        {
            get { return actionPoint; }
            set
            {
                actionPoint = value;

                drawingBox.X = -(int)actionPoint.X;
                drawingBox.Y = -(int)actionPoint.Y;
            }
        }

        public string ImagePath;

        public override string ToString()
        {
            return "Drawing box: (" + drawingBox.X + ", " + drawingBox.Y + ") " + drawingBox.Width + " x " + drawingBox.Height + "\n" +
                "Bounding box: (" + boundingBox.X + ", " + boundingBox.Y + ") " + boundingBox.Width + " x " + boundingBox.Height + "\n" +
                "Action point: (" + actionPoint.X + ", " + actionPoint.Y + ")\n" +
                "Image path: " + ImagePath + "\n";
        }
    }
}
