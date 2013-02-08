using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FuncWorks.XNA.XTiled {
    /// <summary>
    /// Represents an object layer
    /// </summary>
    public class ObjectLayer {
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
        /// Optional color for the layer, alpha is set to the value of Opacity
        /// </summary>
        public Color? Color;
        /// <summary>
        /// Collection of objects on this layer
        /// </summary>
        public MapObject[] MapObjects;
    }
}
