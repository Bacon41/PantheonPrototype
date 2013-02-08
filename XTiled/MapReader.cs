using System;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FuncWorks.XNA.XTiled {

    /// <summary/>
    public sealed class MapReader : ContentTypeReader<Map> {
        /// <summary/>
        protected override Map Read(ContentReader input, Map existingInstance) {
            Map m = new Map();
            Int32 props = 0;
            
            m.Orientation = input.ReadBoolean() ? MapOrientation.Orthogonal : MapOrientation.Isometric;
            m.Width = input.ReadInt32();
            m.Height = input.ReadInt32();
            m.TileHeight = input.ReadInt32();
            m.TileWidth = input.ReadInt32();
            m.Bounds.X = input.ReadInt32();
            m.Bounds.Y = input.ReadInt32();
            m.Bounds.Height = input.ReadInt32();
            m.Bounds.Width = input.ReadInt32();
            m.LoadTextures = input.ReadBoolean();

            m.Tilesets = new Tileset[input.ReadInt32()];
            for (int i = 0; i < m.Tilesets.Length; i++) {
                m.Tilesets[i] = new Tileset();
                m.Tilesets[i].ImageFileName = input.ReadString();
                m.Tilesets[i].ImageHeight = input.ReadInt32();
                m.Tilesets[i].ImageWidth = input.ReadInt32();
                m.Tilesets[i].Margin = input.ReadInt32();
                m.Tilesets[i].Name = input.ReadString();
                m.Tilesets[i].Spacing = input.ReadInt32();
                m.Tilesets[i].TileHeight = input.ReadInt32();
                m.Tilesets[i].TileOffsetX = input.ReadInt32();
                m.Tilesets[i].TileOffsetY = input.ReadInt32();
                m.Tilesets[i].TileWidth = input.ReadInt32();

                if (input.ReadBoolean()) {
                    Color c = Color.White;
                    c.A = input.ReadByte();
                    m.Tilesets[i].ImageTransparentColor = c;
                }

                m.Tilesets[i].Tiles = new Tile[input.ReadInt32()];
                for (int j = 0; j < m.Tilesets[i].Tiles.Length; j++) {
                    m.Tilesets[i].Tiles[j] = new Tile();
                    m.Tilesets[i].Tiles[j].TilesetID = input.ReadInt32();
                    m.Tilesets[i].Tiles[j].Origin.X = input.ReadSingle();
                    m.Tilesets[i].Tiles[j].Origin.Y = input.ReadSingle();
                    m.Tilesets[i].Tiles[j].Source.X = input.ReadInt32();
                    m.Tilesets[i].Tiles[j].Source.Y = input.ReadInt32();
                    m.Tilesets[i].Tiles[j].Source.Height = input.ReadInt32();
                    m.Tilesets[i].Tiles[j].Source.Width = input.ReadInt32();

                    props = input.ReadInt32();
                    m.Tilesets[i].Tiles[j].Properties = new Dictionary<String, Property>();
                    for (int p = 0; p < props; p++) {
                        m.Tilesets[i].Tiles[j].Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
                    }
                }

                props = input.ReadInt32();
                m.Tilesets[i].Properties = new Dictionary<String, Property>(props);
                for (int p = 0; p < props; p++) {
                    m.Tilesets[i].Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
                }
            }

            props = input.ReadInt32();
            m.Properties = new Dictionary<String, Property>(props);
            for (int p = 0; p < props; p++) {
                m.Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
            }

            m.TileLayers = new TileLayerList();
            int tileLayers = input.ReadInt32();
            for (int i = 0; i < tileLayers; i++) {

                m.TileLayers.Add(new TileLayer());
                m.TileLayers[i].Name = input.ReadString();
                m.TileLayers[i].Opacity = input.ReadSingle();
                m.TileLayers[i].OpacityColor = Color.White;
                m.TileLayers[i].OpacityColor.A = input.ReadByte();
                m.TileLayers[i].Visible = input.ReadBoolean();

                props = input.ReadInt32();
                m.TileLayers[i].Properties = new Dictionary<String, Property>(props);
                for (int p = 0; p < props; p++) {
                    m.TileLayers[i].Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
                }

                m.TileLayers[i].Tiles = new TileData[input.ReadInt32()][];
                for (int x = 0; x < m.TileLayers[i].Tiles.Length; x++) {
                    m.TileLayers[i].Tiles[x] = new TileData[input.ReadInt32()];
                    for (int y = 0; y < m.TileLayers[i].Tiles[x].Length; y++) {

                        if (input.ReadBoolean()) {
                            m.TileLayers[i].Tiles[x][y] = new TileData();
                            m.TileLayers[i].Tiles[x][y].Rotation = input.ReadSingle();
                            m.TileLayers[i].Tiles[x][y].SourceID = input.ReadInt32();
                            m.TileLayers[i].Tiles[x][y].Target.X = input.ReadInt32();
                            m.TileLayers[i].Tiles[x][y].Target.Y = input.ReadInt32();
                            m.TileLayers[i].Tiles[x][y].Target.Height = input.ReadInt32();
                            m.TileLayers[i].Tiles[x][y].Target.Width = input.ReadInt32();

                            Boolean FlippedHorizontally = input.ReadBoolean();
                            Boolean FlippedVertically = input.ReadBoolean();

                            if (FlippedVertically && FlippedHorizontally)
                                m.TileLayers[i].Tiles[x][y].Effects = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                            else if (FlippedVertically)
                                m.TileLayers[i].Tiles[x][y].Effects = SpriteEffects.FlipVertically;
                            else if (FlippedHorizontally)
                                m.TileLayers[i].Tiles[x][y].Effects = SpriteEffects.FlipHorizontally;
                            else
                                m.TileLayers[i].Tiles[x][y].Effects = SpriteEffects.None;
                        }
                        else
                            m.TileLayers[i].Tiles[x][y] = null;
                    }
                }
            }

            m.SourceTiles = new Tile[input.ReadInt32()];
            for (int i = 0; i < m.SourceTiles.Length; i++) {
                m.SourceTiles[i] = new Tile();
                m.SourceTiles[i].Origin.X = input.ReadSingle();
                m.SourceTiles[i].Origin.Y = input.ReadSingle();
                m.SourceTiles[i].Source.X = input.ReadInt32();
                m.SourceTiles[i].Source.Y = input.ReadInt32();
                m.SourceTiles[i].Source.Height = input.ReadInt32();
                m.SourceTiles[i].Source.Width = input.ReadInt32();
                m.SourceTiles[i].TilesetID = input.ReadInt32();

                props = input.ReadInt32();
                m.SourceTiles[i].Properties = new Dictionary<String, Property>(props);
                for (int p = 0; p < props; p++) {
                    m.SourceTiles[i].Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
                }
            }

            Int32 olayers = input.ReadInt32();
            m.ObjectLayers = new ObjectLayerList();
            for (int i = 0; i < olayers; i++) {

                m.ObjectLayers.Add(new ObjectLayer());
                m.ObjectLayers[i].Name = input.ReadString();
                m.ObjectLayers[i].Opacity = input.ReadSingle();
                m.ObjectLayers[i].OpacityColor = Color.White;
                m.ObjectLayers[i].OpacityColor.A = input.ReadByte();
                m.ObjectLayers[i].Visible = input.ReadBoolean();

                if (input.ReadBoolean()) {
                    Color c = Color.White;
                    c.R = input.ReadByte();
                    c.G = input.ReadByte();
                    c.B = input.ReadByte();
                    c.A = input.ReadByte();
                    m.ObjectLayers[i].Color = c;
                }
                else
                    m.ObjectLayers[i].Color = null;

                m.ObjectLayers[i].MapObjects = new MapObject[input.ReadInt32()];
                for (int mo = 0; mo < m.ObjectLayers[i].MapObjects.Length; mo++) {

                    m.ObjectLayers[i].MapObjects[mo] = new MapObject();
                    m.ObjectLayers[i].MapObjects[mo].Bounds.X = input.ReadInt32();
                    m.ObjectLayers[i].MapObjects[mo].Bounds.Y = input.ReadInt32();
                    m.ObjectLayers[i].MapObjects[mo].Bounds.Height = input.ReadInt32();
                    m.ObjectLayers[i].MapObjects[mo].Bounds.Width = input.ReadInt32();
                    m.ObjectLayers[i].MapObjects[mo].Name = input.ReadString();
                    m.ObjectLayers[i].MapObjects[mo].Type = input.ReadString();
                    m.ObjectLayers[i].MapObjects[mo].Visible = input.ReadBoolean();

                    if (input.ReadBoolean())
                        m.ObjectLayers[i].MapObjects[mo].TileID = input.ReadInt32();
                    else
                        m.ObjectLayers[i].MapObjects[mo].TileID = null;

                    if (input.ReadBoolean()) {
                        Point[] points = new Point[input.ReadInt32()];
                        for (int p = 0; p < points.Length; p++) {
                            points[p].X = input.ReadInt32();
                            points[p].Y = input.ReadInt32();
                        }
                        m.ObjectLayers[i].MapObjects[mo].Polyline = Polyline.FromPoints(points);
                    }
                    else
                        m.ObjectLayers[i].MapObjects[mo].Polyline = null;

                    if (input.ReadBoolean()) {
                        Point[] points = new Point[input.ReadInt32()];
                        for (int p = 0; p < points.Length; p++) {
                            points[p].X = input.ReadInt32();
                            points[p].Y = input.ReadInt32();
                        }
                        m.ObjectLayers[i].MapObjects[mo].Polygon = Polygon.FromPoints(points);
                    }
                    else
                        m.ObjectLayers[i].MapObjects[mo].Polygon = null;

                    props = input.ReadInt32();
                    m.ObjectLayers[i].MapObjects[mo].Properties = new Dictionary<String, Property>();
                    for (int p = 0; p < props; p++)
                    {
                        m.ObjectLayers[i].MapObjects[mo].Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
                    }
                }

                props = input.ReadInt32();
                m.ObjectLayers[i].Properties = new Dictionary<String, Property>(props);
                for (int p = 0; p < props; p++) {
                    m.ObjectLayers[i].Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
                } 
            }

            m.LayerOrder = new LayerInfo[input.ReadInt32()];
            for (int li = 0; li < m.LayerOrder.Length; li++) {
                m.LayerOrder[li].ID = input.ReadInt32();
                m.LayerOrder[li].LayerType = input.ReadBoolean() ? LayerType.TileLayer : LayerType.ObjectLayer;
            }

            if (m.LoadTextures) {
                for (int i = 0; i < m.Tilesets.Length; i++) {
                    m.Tilesets[i].Texture = input.ContentManager.Load<Texture2D>(String.Format("{0}/{1:00}", input.AssetName, i));
                }
            }

            if (Map._enableRendering)
                m.CreatePolygonTextures();

            return m;
        }
    }
}
