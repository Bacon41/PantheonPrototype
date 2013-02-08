using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FuncWorks.XNA.XTiled {
    /// <summary>
    /// Represents a tileset (aka tile sheet) in a Map
    /// </summary>
    public class Tileset {
        /// <summary>
        /// Optional name of the tileset
        /// </summary>
        public String Name;
        /// <summary>
        /// Width of a single tile in pixels
        /// </summary>
        public Int32 TileWidth;
        /// <summary>
        /// Height of a single tile in pixels
        /// </summary>
        public Int32 TileHeight;
        /// <summary>
        /// Spacing between tiles in pixels
        /// </summary>
        public Int32 Spacing;
        /// <summary>
        /// Outside margin of the tilesheet in pixels
        /// </summary>
        public Int32 Margin;
        /// <summary>
        /// Horizontal offset of tiles
        /// </summary>
        public Int32 TileOffsetX;
        /// <summary>
        /// Vertical offset of tiles
        /// </summary>
        public Int32 TileOffsetY;
        /// <summary>
        /// Custom properties
        /// </summary>
        public Dictionary<String, Property> Properties;
        /// <summary>
        /// List of tiles in this tilesheet
        /// </summary>
        public Tile[] Tiles;
        /// <summary>
        /// Full path of the image referenced in the TMX file
        /// </summary>
        public String ImageFileName;
        /// <summary>
        /// Transparent color as set in the Tiled editor; null if not set
        /// </summary>
        public Color? ImageTransparentColor;
        /// <summary>
        /// Width of image in pixels
        /// </summary>
        public Int32 ImageWidth;
        /// <summary>
        /// Height of image in pixels
        /// </summary>
        public Int32 ImageHeight;
        /// <summary>
        /// Image as an XNA Texture2D instance; null if Map.LoadTextures is false
        /// </summary>
        public Texture2D Texture;
    }
}
