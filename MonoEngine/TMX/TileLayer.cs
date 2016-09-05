using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.TMX
{
    [Serializable]
    public class TileLayer : Layer
    {
        /// <summary>
        /// The width of the TileLayer in tiles.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The height of the TileLayer in tiles.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// A multidimensional list of tile IDs.
        /// </summary>
        public List<List<int>> TileData { get; set; }

        /// <summary>
        /// Creates an empty TileLayer
        /// </summary>
        public TileLayer()
        {
            Width = 0;
            Height = 0;
            TileData = new List<List<int>>();
        }
    }
}
