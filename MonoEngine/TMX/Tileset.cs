using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.TMX
{
    [Serializable]
    public class Tileset : Element
    {
        /// <summary>
        /// The index of the first tile.
        /// </summary>
        public int FirstGID { get; set; }

        /// <summary>
        /// The width of each individual tile.
        /// </summary>
        public int TileWidth { get; set; }

        /// <summary>
        /// The height of each individual tile.
        /// </summary>
        public int TileHeight { get; set; }

        /// <summary>
        /// The spacing between the tiles.
        /// </summary>
        public int Spacing { get; set; }

        /// <summary>
        /// The number of pixels between the tileset and the edge of the image.
        /// </summary>
        public int Margin { get; set; }

        /// <summary>
        /// The number of tiles in the tileset.
        /// </summary>
        public int TileCount { get; set; }

        /// <summary>
        /// The number of columns.
        /// </summary>
        public int Columns { get; set; }

        /// <summary>
        /// The source of the image.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// The width of the Tileset image.
        /// </summary>
        public int ImageWidth { get; set; }

        /// <summary>
        /// The height of the Tileset image.
        /// </summary>
        public int ImageHeight { get; set; }

        /// <summary>
        /// Creates an empty tileset.
        /// </summary>
        public Tileset()
        {
            FirstGID = 0;
            Name = string.Empty;
            TileWidth = 0;
            TileHeight = 0;
            Spacing = 0;
            Margin = 0;
            TileCount = 0;
            Columns = 0;
            Source = string.Empty;
            ImageWidth = 0;
            ImageHeight = 0;
        }
    }
}
