using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FuncWorks.XNA.XTiled {
    /// <summary />
    public enum MapOrientation {
        /// <summary />
        Orthogonal,
        /// <summary />
        Isometric
    }

    /// <summary />
    public enum LayerType {
        /// <summary />
        TileLayer,
        /// <summary />
        ObjectLayer
    }

    /// <summary>
    /// References either a TileLayer or ObjectLayer
    /// </summary>
    public struct LayerInfo {
        /// <summary>
        /// The Layer Index/ID
        /// </summary>
        public Int32 ID;
        /// <summary>
        /// They type of layer
        /// </summary>
        public LayerType LayerType;
    }

    /// <summary>
    /// An XTiled TMX Map
    /// </summary>
    public class Map {

        internal static Boolean _enableRendering = false;
        internal static Texture2D _whiteTexture = null;
        internal const float _lineThickness = 1.0f;

        /// <summary>
        /// Orientation of the map.
        /// </summary>
        public MapOrientation Orientation;
        /// <summary>
        /// Width, in tiles, of the map
        /// </summary>
        public Int32 Width;
        /// <summary>
        /// Height, in tiles, of the map
        /// </summary>
        public Int32 Height;
        /// <summary>
        /// Pixel width of a single tile
        /// </summary>
        public Int32 TileWidth;
        /// <summary>
        /// Pixel height of a single tile
        /// </summary>
        public Int32 TileHeight;
        /// <summary>
        /// Size of the map in pixels
        /// </summary>
        public Rectangle Bounds;
        /// <summary>
        /// Tilesets associated with this map
        /// </summary>
        public Tileset[] Tilesets;
        /// <summary>
        /// Custom properties
        /// </summary>
        public Dictionary<String, Property> Properties;
        /// <summary>
        /// Ordered collection of tile layers; first is the bottom layer
        /// </summary>
        public TileLayerList TileLayers;
        /// <summary>
        /// Ordered collection of object layers; first is the bottom layer
        /// </summary>
        public ObjectLayerList ObjectLayers;
        /// <summary>
        /// List of all source tiles from tilesets
        /// </summary>
        public Tile[] SourceTiles;
        /// <summary>
        /// True if XTiled loaded tileset textures during map load
        /// </summary>
        public Boolean LoadTextures;
        /// <summary>
        /// Order of tile and object layers combined, first is the bottom layer
        /// </summary>
        public LayerInfo[] LayerOrder;

        /// <summary>
        /// Enables rendering of map objects
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to us in creating textures to support object rendering</param>
        public static void InitObjectDrawing(GraphicsDevice graphicsDevice) {
            Map._whiteTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Map._whiteTexture.SetData(new[] { Color.White });
            Map._enableRendering = true;
        }

        internal void CreatePolygonTextures() {
            for (int i = 0; i < this.ObjectLayers.Count; i++) {
                for (int o = 0; o < this.ObjectLayers[i].MapObjects.Length; o++) {
                    if (this.ObjectLayers[i].MapObjects[o].Polygon != null) {
                        this.ObjectLayers[i].MapObjects[o].Polygon.GenerateTexture(Map._whiteTexture.GraphicsDevice, Color.White);
                    }
                }
            }
        }

        /// <summary>
        /// Draws all visible tile layers
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        public void Draw(SpriteBatch spriteBatch, Rectangle region) {
            this.Draw(spriteBatch, ref region, false);
        }

        /// <summary>
        /// Draws all visible tile layers
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        public void Draw(SpriteBatch spriteBatch, ref Rectangle region) {
            this.Draw(spriteBatch, ref region, false);
        }

        /// <summary>
        /// Draws all visible tile layers
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        /// <param name="drawHiddenLayers">If true, draws layers regardless of TileLayer.Visible flag</param>
        public void Draw(SpriteBatch spriteBatch, Rectangle region, Boolean drawHiddenLayers) {
            this.Draw(spriteBatch, ref region, drawHiddenLayers);
        }

        /// <summary>
        /// Draws all visible tile layers
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        /// <param name="drawHiddenLayers">If true, draws layers regardless of TileLayer.Visible flag</param>
        public void Draw(SpriteBatch spriteBatch, ref Rectangle region, Boolean drawHiddenLayers) {

            Int32 txMin = region.X / this.TileWidth;
            Int32 txMax = (region.X + region.Width) / this.TileWidth;
            Int32 tyMin = region.Y / this.TileHeight;
            Int32 tyMax = (region.Y + region.Height) / this.TileHeight;

            if (this.Orientation == MapOrientation.Isometric) {
                tyMax = tyMax * 2 + 1;
                txMax = txMax * 2 + 1;
            }

            for (int l = 0; l < this.TileLayers.Count; l++) {
                if (this.TileLayers[l].Visible || drawHiddenLayers) {
                    // Changed the leayer depth to change based on the height of the layer
                    DrawLayer(spriteBatch, l, ref region, txMin, txMax, tyMin, tyMax, (float)(1-(l+1) * 0.1f));
                }
            }
        }

        /// <summary>
        /// Draws given tile layer
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="layerID">Index of the layer to draw in the Map.TileLayers collection</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        public void DrawLayer(SpriteBatch spriteBatch, Int32 layerID, Rectangle region, Single layerDepth) {
            DrawLayer(spriteBatch, layerID, ref region, layerDepth);
        }

        /// <summary>
        /// Draws given tile layer
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="layerID">Index of the layer to draw in the Map.TileLayers collection</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        public void DrawLayer(SpriteBatch spriteBatch, Int32 layerID, ref Rectangle region, Single layerDepth) {
            Int32 txMin = region.X / this.TileWidth;
            Int32 txMax = (region.X + region.Width) / this.TileWidth;
            Int32 tyMin = region.Y / this.TileHeight;
            Int32 tyMax = (region.Y + region.Height) / this.TileHeight;

            if (this.Orientation == MapOrientation.Isometric) {
                tyMax = tyMax * 2 + 1;
                txMax = txMax * 2 + 1;
            }

            DrawLayer(spriteBatch, layerID, ref region, txMin, txMax, tyMin, tyMax, layerDepth);
        }

        private void DrawLayer(SpriteBatch spriteBatch, Int32 layerID, ref Rectangle region, Int32 txMin, Int32 txMax, Int32 tyMin, Int32 tyMax, Single layerDepth) {
            for (int y = tyMin; y <= tyMax; y++) {
                for (int x = txMin; x <= txMax; x++) {
                    if (x < this.TileLayers[layerID].Tiles.Length && y < this.TileLayers[layerID].Tiles[x].Length && this.TileLayers[layerID].Tiles[x][y] != null) {
                        Rectangle tileTarget = this.TileLayers[layerID].Tiles[x][y].Target;
                        // Removed the offset part of this code.
                        //tileTarget.X = tileTarget.X - region.X;
                        //tileTarget.Y = tileTarget.Y - region.Y;

                        spriteBatch.Draw(
                            this.Tilesets[this.SourceTiles[this.TileLayers[layerID].Tiles[x][y].SourceID].TilesetID].Texture,
                            tileTarget,
                            this.SourceTiles[this.TileLayers[layerID].Tiles[x][y].SourceID].Source,
                            this.TileLayers[layerID].OpacityColor,
                            this.TileLayers[layerID].Tiles[x][y].Rotation,
                            this.SourceTiles[this.TileLayers[layerID].Tiles[x][y].SourceID].Origin,
                            this.TileLayers[layerID].Tiles[x][y].Effects,
                            layerDepth);
                    }
                }
            }
        }

        /// <summary>
        /// Draws all objects on the given object layer
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="objectLayerID">Index of the layer to draw in the Map.ObjectLayers collection</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        public void DrawObjectLayer(SpriteBatch spriteBatch, Int32 objectLayerID, Rectangle region, Single layerDepth) {
            DrawObjectLayer(spriteBatch, objectLayerID, ref region, layerDepth);
        }

        /// <summary>
        /// Draws all objects on the given object layer
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="objectLayerID">Index of the layer to draw in the Map.ObjectLayers collection</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        public void DrawObjectLayer(SpriteBatch spriteBatch, Int32 objectLayerID, ref Rectangle region, Single layerDepth) {
            if (Map._whiteTexture == null) {
                throw new Exception("Map.InitObjectDrawing must be called before Map is loaded to enable object rendering");
            }

            for (int o = 0; o < this.ObjectLayers[objectLayerID].MapObjects.Length; o++) {
                if (region.Contains(this.ObjectLayers[objectLayerID].MapObjects[o].Bounds) || region.Intersects(this.ObjectLayers[objectLayerID].MapObjects[o].Bounds)) {
                    DrawMapObject(spriteBatch, objectLayerID, o, ref region, layerDepth);
                }
            }
        }

        /// <summary>
        /// Method to draw a MapObject that represents a tile object
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="objectLayerID">Index of the layer to draw in the Map.ObjectLayers collection</param>
        /// <param name="objectID">Index of the object to draw in the Map.ObjectLayers.MapObjects collection</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        /// <param name="color">Color of the object</param>
        public void DrawTileObject(SpriteBatch spriteBatch, Int32 objectLayerID, Int32 objectID, Rectangle region, Single layerDepth, Color color) {
            this.DrawTileObject(spriteBatch, objectLayerID, objectID, ref region, layerDepth, ref color);
        }

        /// <summary>
        /// Method to draw a MapObject that represents a tile object
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="objectLayerID">Index of the layer to draw in the Map.ObjectLayers collection</param>
        /// <param name="objectID">Index of the object to draw in the Map.ObjectLayers.MapObjects collection</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        /// <param name="color">Color of the object</param>
        public void DrawTileObject(SpriteBatch spriteBatch, int objectLayerID, int objectID, ref Rectangle region, float layerDepth, ref Color color) {
            spriteBatch.Draw(
                this.Tilesets[this.SourceTiles[this.ObjectLayers[objectLayerID].MapObjects[objectID].TileID.Value].TilesetID].Texture,
                Map.Translate(this.ObjectLayers[objectLayerID].MapObjects[objectID].Bounds, region),
                this.SourceTiles[this.ObjectLayers[objectLayerID].MapObjects[objectID].TileID.Value].Source,
                color,
                0,
                this.SourceTiles[this.ObjectLayers[objectLayerID].MapObjects[objectID].TileID.Value].Origin,
                SpriteEffects.None,
                layerDepth);
        }

        /// <summary>
        /// Method to draw a MapObject
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="objectLayerID">Index of the layer to draw in the Map.ObjectLayers collection</param>
        /// <param name="objectID">Index of the object to draw in the Map.ObjectLayers.MapObjects collection</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        /// <param name="color">Color of the object</param>
        public void DrawMapObject(SpriteBatch spriteBatch, Int32 objectLayerID, Int32 objectID, Rectangle region, Single layerDepth) {
            DrawMapObject(spriteBatch, objectLayerID, objectID, ref region, layerDepth);
        }

        /// <summary>
        /// Method to draw a MapObject
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="objectLayerID">Index of the layer to draw in the Map.ObjectLayers collection</param>
        /// <param name="objectID">Index of the object to draw in the Map.ObjectLayers.MapObjects collection</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        /// <param name="color">Color of the object</param>
        public void DrawMapObject(SpriteBatch spriteBatch, Int32 objectLayerID, Int32 objectID, ref Rectangle region, Single layerDepth) {
            Color color = this.ObjectLayers[objectLayerID].Color ?? this.ObjectLayers[objectLayerID].OpacityColor;
            Color fillColor = color;
            fillColor.A /= 4;

            if (this.ObjectLayers[objectLayerID].MapObjects[objectID].Polyline != null) {
                this.ObjectLayers[objectLayerID].MapObjects[objectID].Polyline.Draw(spriteBatch, region, Map._whiteTexture, Map._lineThickness, color, layerDepth);
            }
            else if (this.ObjectLayers[objectLayerID].MapObjects[objectID].Polygon != null) {
                this.ObjectLayers[objectLayerID].MapObjects[objectID].Polygon.DrawFilled(spriteBatch, region, Map._whiteTexture, Map._lineThickness, color, fillColor, layerDepth);
            }
            else if (this.ObjectLayers[objectLayerID].MapObjects[objectID].TileID.HasValue) {
                DrawTileObject(spriteBatch, objectLayerID, objectID, ref region, layerDepth, ref color);
            }
            else {
                DrawRectangle(spriteBatch, ref this.ObjectLayers[objectLayerID].MapObjects[objectID].Bounds, ref region, layerDepth, ref color, ref fillColor);
            }
        }

        /// <summary>
        /// Method to draw a Rectangle
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="rect">The Rectangle to draw, in map pixels</param>
        /// <param name="region">Region of the map in pixels currently visible</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        /// <param name="linecolor">Color of the Rectangle border</param>
        /// <param name="fillColor">Color to fill the Rectangle with</param>
        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rect, Rectangle region, Single layerDepth, Color linecolor, Color fillColor) {
            DrawRectangle(spriteBatch, ref rect, ref region, layerDepth, ref linecolor, ref fillColor);
        }

        /// <summary>
        /// Method to draw a Rectangle
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="rect">The Rectangle to draw, in map pixels</param>
        /// <param name="region">Region of the map in pixels currently visible</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        /// <param name="linecolor">Color of the Rectangle border</param>
        /// <param name="fillColor">Color to fill the Rectangle with</param>
        public static void DrawRectangle(SpriteBatch spriteBatch, ref Rectangle rect, ref Rectangle region, Single layerDepth, ref Color linecolor, ref Color fillColor) {
            if (Map._whiteTexture == null) {
                throw new Exception("Map.InitObjectDrawing must be called before Map is loaded to enable object rendering");
            }

            Rectangle target = Map.Translate(rect, region);
            spriteBatch.Draw(Map._whiteTexture, target, null, fillColor, 0, Vector2.Zero, SpriteEffects.None, layerDepth);
            Line.Draw(spriteBatch, Line.FromPoints(new Vector2(rect.Right, rect.Top), new Vector2(rect.Left, rect.Top)), region, Map._whiteTexture, Map._lineThickness, linecolor, layerDepth);
            Line.Draw(spriteBatch, Line.FromPoints(new Vector2(rect.Left, rect.Top), new Vector2(rect.Left, rect.Bottom)), region, Map._whiteTexture, Map._lineThickness, linecolor, layerDepth);
            Line.Draw(spriteBatch, Line.FromPoints(new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Right, rect.Bottom)), region, Map._whiteTexture, Map._lineThickness, linecolor, layerDepth);
            Line.Draw(spriteBatch, Line.FromPoints(new Vector2(rect.Right, rect.Bottom), new Vector2(rect.Right, rect.Top)), region, Map._whiteTexture, Map._lineThickness, linecolor, layerDepth);
        }

        /// <summary>
        /// Translates a location to screen space
        /// </summary>
        /// <param name="location">The location in map pixel coordinates</param>
        /// <param name="relativeTo">Region of the map that is on screen</param>
        /// <returns>A location converted to screen space</returns>
        public static Rectangle Translate(Rectangle location, Rectangle relativeTo) {
            location.X = location.X - relativeTo.X;
            location.Y = location.Y - relativeTo.Y;
            return location;
        }

        /// <summary>
        /// Translates a location to screen space
        /// </summary>
        /// <param name="location">The location in map pixel coordinates</param>
        /// <param name="relativeTo">Region of the map that is on screen</param>
        public static void Translate(ref Rectangle location, ref Rectangle relativeTo) {
            location.X = location.X - relativeTo.X;
            location.Y = location.Y - relativeTo.Y;
        }

        /// <summary>
        /// Translates a location to screen space
        /// </summary>
        /// <param name="location">The location in map pixel coordinates</param>
        /// <param name="relativeTo">Region of the map that is on screen</param>
        /// <returns>A location converted to screen space</returns>
        public static Point Translate(Point location, Rectangle relativeTo) {
            location.X = location.X - relativeTo.X;
            location.Y = location.Y - relativeTo.Y;
            return location;
        }

        /// <summary>
        /// Translates a location to screen space
        /// </summary>
        /// <param name="location">The location in map pixel coordinates</param>
        /// <param name="relativeTo">Region of the map that is on screen</param>
        public static void Translate(ref Point location, ref Rectangle relativeTo) {
            location.X = location.X - relativeTo.X;
            location.Y = location.Y - relativeTo.Y;
        }

        /// <summary>
        /// Translates a location to screen space
        /// </summary>
        /// <param name="location">The location in map pixel coordinates</param>
        /// <param name="relativeTo">Region of the map that is on screen</param>
        /// <returns>A location converted to screen space</returns>
        public static Vector2 Translate(Vector2 location, Rectangle relativeTo) {
            location.X = location.X - relativeTo.X;
            location.Y = location.Y - relativeTo.Y;
            return location;
        }

        /// <summary>
        /// Translates a location to screen space
        /// </summary>
        /// <param name="location">The location in map pixel coordinates</param>
        /// <param name="relativeTo">Region of the map that is on screen</param>
        public static void Translate(ref Vector2 location, ref Rectangle relativeTo) {
            location.X = location.X - relativeTo.X;
            location.Y = location.Y - relativeTo.Y;
        }

        /// <summary>
        /// Returns a collection of MapObjects inside the given region
        /// </summary>
        /// <param name="region">The region, in pixles, to check</param>
        /// <returns>Collection of matching MapObjects</returns>
        public IEnumerable<MapObject> GetObjectsInRegion(Rectangle region) {
            return this.GetObjectsInRegion(ref region);
        }

        /// <summary>
        /// Returns a collection of MapObjects inside the given region
        /// </summary>
        /// <param name="region">The region, in pixles, to check</param>
        /// <returns>Collection of matching MapObjects</returns>
        public IEnumerable<MapObject> GetObjectsInRegion(ref Rectangle region) {
            List<MapObject> results = new List<MapObject>();

            for (int i = 0; i < ObjectLayers.Count; i++)
                results.AddRange(this.GetObjectsInRegion(i, ref region));

            return results;
        }

        /// <summary>
        /// Returns a collection of MapObjects inside the given region
        /// </summary>
        /// <param name="objectLayerID">The object layer to check</param>
        /// <param name="region">The region, in pixles, to check</param>
        /// <returns>Collection of matching MapObjects</returns>
        public IEnumerable<MapObject> GetObjectsInRegion(Int32 objectLayerID, Rectangle region) {
            return this.GetObjectsInRegion(objectLayerID, ref region);
        }

        /// <summary>
        /// Returns a collection of MapObjects inside the given region
        /// </summary>
        /// <param name="objectLayerID">The object layer to check</param>
        /// <param name="region">The region, in pixles, to check</param>
        /// <returns>Collection of matching MapObjects</returns>
        public IEnumerable<MapObject> GetObjectsInRegion(Int32 objectLayerID, ref Rectangle region) {
            List<MapObject> results = new List<MapObject>();

            for (int i = 0; i < this.ObjectLayers[objectLayerID].MapObjects.Length; i++) {
                if (region.Contains(this.ObjectLayers[objectLayerID].MapObjects[i].Bounds) || region.Intersects(this.ObjectLayers[objectLayerID].MapObjects[i].Bounds))
                    results.Add(this.ObjectLayers[objectLayerID].MapObjects[i]);
            }

            return results;
        }

        /// <summary>
        /// Returns a collection of TileData inside the given region
        /// </summary>
        /// <param name="region">The region, in pixles, to check</param>
        /// <returns>Collection of matching TileData</returns>
        public IEnumerable<TileData> GetTilesInRegion(Rectangle region) {
            return this.GetTilesInRegion(ref region);
        }

        /// <summary>
        /// Returns a collection of TileData inside the given region
        /// </summary>
        /// <param name="region">The region, in pixles, to check</param>
        /// <returns>Collection of matching TileData</returns>
        public IEnumerable<TileData> GetTilesInRegion(ref Rectangle region) {
            List<TileData> result = new List<TileData>();

            Int32 txMin = region.X / this.TileWidth;
            Int32 txMax = (region.X + region.Width) / this.TileWidth;
            Int32 tyMin = region.Y / this.TileHeight;
            Int32 tyMax = (region.Y + region.Height) / this.TileHeight;

            if (this.Orientation == MapOrientation.Isometric) {
                tyMax = tyMax * 2 + 1;
                txMax = txMax * 2 + 1;
            }

            for (int i = 0; i < this.TileLayers.Count; i++) {
                for (int y = tyMin; y <= tyMax; y++) {
                    for (int x = txMin; x <= txMax; x++) {
                        if (this.TileLayers[i].Tiles[x][y] != null) {
                            result.Add(this.TileLayers[i].Tiles[x][y]);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a collection of TileData inside the given region
        /// </summary>
        /// <param name="tileLayerID">The tile layer to check</param>
        /// <param name="region">The region, in pixles, to check</param>
        /// <returns>Collection of matching TileData</returns>
        public IEnumerable<TileData> GetTilesInRegion(Int32 tileLayerID, Rectangle region) {
            return this.GetTilesInRegion(tileLayerID, ref region);
        }

        /// <summary>
        /// Returns a collection of TileData inside the given region
        /// </summary>
        /// <param name="tileLayerID">The tile layer to check</param>
        /// <param name="region">The region, in pixles, to check</param>
        /// <returns>Collection of matching TileData</returns>
        public IEnumerable<TileData> GetTilesInRegion(Int32 tileLayerID, ref Rectangle region) {
            List<TileData> result = new List<TileData>();

            Int32 txMin = region.X / this.TileWidth;
            Int32 txMax = (region.X + region.Width) / this.TileWidth;
            Int32 tyMin = region.Y / this.TileHeight;
            Int32 tyMax = (region.Y + region.Height) / this.TileHeight;

            if (this.Orientation == MapOrientation.Isometric) {
                tyMax = tyMax * 2 + 1;
                txMax = txMax * 2 + 1;
            }

            for (int y = tyMin; y <= tyMax; y++) {
                for (int x = txMin; x <= txMax; x++) {
                    if (this.TileLayers[tileLayerID].Tiles[x][y] != null) {
                        result.Add(this.TileLayers[tileLayerID].Tiles[x][y]);
                    }
                }
            }
            return result;
        }
    }
}
