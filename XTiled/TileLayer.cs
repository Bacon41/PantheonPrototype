using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FuncWorks.XNA.XTiled {
    /// <summary>
    /// Represents an tile layer
    /// </summary>
    public class TileLayer {
        /// <summary>
        /// Optional name of the layer
        /// </summary>
        public String Name;
        /// <summary>
        /// Opacity of the layer, 1 is opaque and 0 is complete transparent
        /// </summary>
        public Single Opacity;
        /// <summary>
        /// A color value of white, with the alpha component set to the layer Opacity
        /// </summary>
        public Color OpacityColor;
        /// <summary>
        /// True if the layer is visible
        /// </summary>
        public Boolean Visible;
        /// <summary>
        /// Custom properties
        /// </summary>
        public Dictionary<String, Property> Properties;
        /// <summary>
        /// Collection of tiles on this layer
        /// </summary>
        public TileData[][] Tiles;
    }
}
