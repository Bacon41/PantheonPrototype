using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PantheonPrototype
{
    public class Map
    {
        double[] textureGrid;
        public double[] TextureGrid
        {
            get { return textureGrid; }
            set { textureGrid = value; }
        }

        double[] collisionGrid;
        public double[] CollisionGrid
        {
            get { return collisionGrid; }
            set { collisionGrid = value; }
        }

        public Map()
        {
        }
    }
}
