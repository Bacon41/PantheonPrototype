using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace FuncWorks.XNA.XTiled {
    /// <summary>
    /// List of TileLayers, indexable by id or name
    /// </summary>
    public class TileLayerList : List<TileLayer> {
        /// <summary>
        /// Gets the layer with the given name; read-only propery.
        /// </summary>
        /// <param name="name">Name of the layer</param>
        public TileLayer this[String name] {
            get {
                return this.FirstOrDefault(x => x.Name.Equals(name));
            }
        }
    }

    /// <summary>
    /// List of ObjectLayer, indexable by id or name
    /// </summary>
    public class ObjectLayerList : List<ObjectLayer> {
        /// <summary>
        /// Gets the layer with the given name; read-only propery.
        /// </summary>
        /// <param name="name">Name of the layer</param>
        public ObjectLayer this[String name] {
            get {
                return this.FirstOrDefault(x => x.Name.Equals(name));
            }
        }
    }
}
