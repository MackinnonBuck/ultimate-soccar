using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.TMX
{
    [Serializable]
    public class Map : Element
    {
        /// <summary>
        /// The Tilesets associated with the Map.
        /// </summary>
        public List<Tileset> Tilesets { get; private set; }

        /// <summary>
        /// The Layers associated with the Map.
        /// </summary>
        public List<Layer> Layers { get; private set; }

        /// <summary>
        /// Creates a new Map instance.
        /// </summary>
        public Map()
        {
            Tilesets = new List<Tileset>();
            Layers = new List<Layer>();
        }
    }
}
