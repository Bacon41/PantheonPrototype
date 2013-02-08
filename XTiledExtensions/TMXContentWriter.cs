using System;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;

namespace FuncWorks.XNA.XTiled {
    [ContentTypeWriter]
    public class TMXContentWriter : ContentTypeWriter<Map> {
        protected override void Write(ContentWriter output, Map value) {

            output.Write(value.Orientation == MapOrientation.Orthogonal);
            output.Write(value.Width);
            output.Write(value.Height);
            output.Write(value.TileHeight);
            output.Write(value.TileWidth);
            output.Write(value.Bounds.X);
            output.Write(value.Bounds.Y);
            output.Write(value.Bounds.Height);
            output.Write(value.Bounds.Width);
            output.Write(value.LoadTextures);

            output.Write(value.Tilesets.Length);
            foreach (var ts in value.Tilesets) {
                output.Write(ts.ImageFileName == null ? String.Empty : ts.ImageFileName);
                output.Write(ts.ImageHeight);
                output.Write(ts.ImageWidth);
                output.Write(ts.Margin);
                output.Write(ts.Name == null ? String.Empty : ts.Name);
                output.Write(ts.Spacing);
                output.Write(ts.TileHeight);
                output.Write(ts.TileOffsetX);
                output.Write(ts.TileOffsetY);
                output.Write(ts.TileWidth);

                output.Write(ts.ImageTransparentColor.HasValue);
                if (ts.ImageTransparentColor.HasValue)
                    output.Write(ts.ImageTransparentColor.Value.A);

                output.Write(ts.Tiles.Length);
                foreach (var t in ts.Tiles) {
                    output.Write(t.TilesetID);
                    output.Write(t.Origin.X);
                    output.Write(t.Origin.Y);
                    output.Write(t.Source.X);
                    output.Write(t.Source.Y);
                    output.Write(t.Source.Height);
                    output.Write(t.Source.Width);

                    output.Write(t.Properties.Count);
                    foreach (var kv in t.Properties) {
                        output.Write(kv.Key == null ? String.Empty : kv.Key);
                        output.Write(kv.Value.Value == null ? String.Empty : kv.Value.Value);
                    }
                }

                output.Write(ts.Properties.Count);
                foreach (var kv in ts.Properties) {
                    output.Write(kv.Key == null ? String.Empty : kv.Key);
                    output.Write(kv.Value.Value == null ? String.Empty : kv.Value.Value);
                }
            }

            output.Write(value.Properties.Count);
            foreach (var kv in value.Properties) {
                output.Write(kv.Key == null ? String.Empty : kv.Key);
                output.Write(kv.Value.Value == null ? String.Empty : kv.Value.Value);
            }

            output.Write(value.TileLayers.Count);
            foreach (var l in value.TileLayers) {
                output.Write(l.Name == null ? String.Empty : l.Name);
                output.Write(l.Opacity);
                output.Write(l.OpacityColor.A);
                output.Write(l.Visible);

                output.Write(l.Properties.Count);
                foreach (var kv in l.Properties) {
                    output.Write(kv.Key == null ? String.Empty : kv.Key);
                    output.Write(kv.Value.Value == null ? String.Empty : kv.Value.Value);
                }

                output.Write(l.Tiles.Length);
                foreach (var row in l.Tiles) {
                    output.Write(row.Length);
                    foreach (var t in row) {
                        output.Write(t != null);
                        if (t != null) {
                            output.Write(t.Rotation);
                            output.Write(t.SourceID);
                            output.Write(t.Target.X);
                            output.Write(t.Target.Y);
                            output.Write(t.Target.Height);
                            output.Write(t.Target.Width);
                            output.Write(t.Effects.HasFlag(SpriteEffects.FlipHorizontally));
                            output.Write(t.Effects.HasFlag(SpriteEffects.FlipVertically));
                        }
                    }
                }
            }

            output.Write(value.SourceTiles.Length);
            foreach (var t in value.SourceTiles) {
                output.Write(t.Origin.X);
                output.Write(t.Origin.Y);
                output.Write(t.Source.X);
                output.Write(t.Source.Y);
                output.Write(t.Source.Height);
                output.Write(t.Source.Width);
                output.Write(t.TilesetID);

                output.Write(t.Properties.Count);
                foreach (var kv in t.Properties) {
                    output.Write(kv.Key == null ? String.Empty : kv.Key);
                    output.Write(kv.Value.Value == null ? String.Empty : kv.Value.Value);
                }
            }

            output.Write(value.ObjectLayers.Count);
            foreach (var ol in value.ObjectLayers) {
                output.Write(ol.Name == null ? String.Empty : ol.Name);
                output.Write(ol.Opacity);
                output.Write(ol.OpacityColor.A);
                output.Write(ol.Visible);

                output.Write(ol.Color.HasValue);
                if (ol.Color.HasValue) {
                    output.Write(ol.Color.Value.R);
                    output.Write(ol.Color.Value.G);
                    output.Write(ol.Color.Value.B);
                    output.Write(ol.Color.Value.A);
                }

                output.Write(ol.MapObjects.Length);
                foreach (var m in ol.MapObjects) {
                    output.Write(m.Bounds.X);
                    output.Write(m.Bounds.Y);
                    output.Write(m.Bounds.Height);
                    output.Write(m.Bounds.Width);
                    output.Write(m.Name == null ? String.Empty : m.Name);
                    output.Write(m.Type == null ? String.Empty : m.Type);
                    output.Write(m.Visible);

                    output.Write(m.TileID.HasValue);
                    if (m.TileID.HasValue)
                        output.Write(m.TileID.Value);

                    output.Write(m.Polyline != null);
                    if (m.Polyline != null) {
                        output.Write(m.Polyline.Points.Length);
                        foreach (var p in m.Polyline.Points) {
                            output.Write(p.X);
                            output.Write(p.Y);
                        }
                    }

                    output.Write(m.Polygon != null);
                    if (m.Polygon != null) {
                        output.Write(m.Polygon.Points.Length);
                        foreach (var p in m.Polygon.Points) {
                            output.Write(p.X);
                            output.Write(p.Y);
                        }
                    }

                    output.Write(m.Properties.Count);
                    foreach (var kv in m.Properties)
                    {
                        output.Write(kv.Key == null ? String.Empty : kv.Key);
                        output.Write(kv.Value.Value == null ? String.Empty : kv.Value.Value);
                    }
                }

                output.Write(ol.Properties.Count);
                foreach (var kv in ol.Properties) {
                    output.Write(kv.Key == null ? String.Empty : kv.Key);
                    output.Write(kv.Value.Value == null ? String.Empty : kv.Value.Value);
                }
            }

            output.Write(value.LayerOrder.Length);
            foreach (var lo in value.LayerOrder) {
                output.Write(lo.ID);
                output.Write(lo.LayerType == LayerType.TileLayer ? true : false);
            }

        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform) {
            return "FuncWorks.XNA.XTiled.MapReader, FuncWorks.XNA.XTiled";
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform) {
            return "FuncWorks.XNA.XTiled.Map, FuncWorks.XNA.XTiled";
        }

    }
}
