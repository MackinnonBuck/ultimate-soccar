using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using MonoEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace MonoEngine.TMX
{
    [ContentProcessor(DisplayName = "MapProcessor - MonoEngine")]
    public class MapProcessor : ContentProcessor<XmlReader, Map>
    {
        private ContentProcessorContext _context;

        /// <summary>
        /// Used for easy access of all elements read by an XmlReader.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IEnumerable<string> AllElements(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.IsStartElement())
                    yield return reader.Name;
            }
        }

        /// <summary>
        /// Processes a new Map from the given XmlReader.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Map Process(XmlReader input, ContentProcessorContext context)
        {
            _context = context;

            context.Logger.LogMessage("Processing Map...");

            Map map = new Map();

            foreach (string name in AllElements(input))
            {
                switch (name)
                {
                    case "map":
                        map.Width = int.Parse(input["width"]);
                        map.Height = int.Parse(input["height"]);
                        map.TileWidth = int.Parse(input["tilewidth"]);
                        map.TileHeight = int.Parse(input["tileheight"]);
                        break;
                    case "properties":
                        ProcessProperties(input.ReadSubtree(), map);
                        break;
                    case "tileset":
                        ProcessTileset(input.ReadSubtree(), map);
                        break;
                    case "layer":
                        ProcessTileLayer(input.ReadSubtree(), map);
                        break;
                    case "objectgroup":
                        ProcessObjectGroup(input.ReadSubtree(), map);
                        break;
                }
            }

            return map;
        }

        /// <summary>
        /// Processes a map's properties from the given XmlReader.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="element"></param>
        private void ProcessProperties(XmlReader reader, Element element)
        {
            foreach (string name in AllElements(reader))
            {
                switch (name)
                {
                    case "property":
                        element.Properties.Add(reader["name"], reader["value"]);
                        break;
                }
            }
        }

        /// <summary>
        /// Processes tileset from the given XmlReader.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="map"></param>
        private void ProcessTileset(XmlReader reader, Map map)
        {
            Tileset tileset = new Tileset();

            foreach (string name in AllElements(reader))
            {
                switch (name)
                {
                    case "tileset":
                        tileset.FirstGID = int.Parse(reader["firstgid"]);
                        tileset.Name = reader["name"];
                        tileset.TileWidth = int.Parse(reader["tilewidth"]);
                        tileset.TileHeight = int.Parse(reader["tileheight"]);
                        tileset.Spacing = int.Parse(reader["spacing"]);
                        tileset.Margin = int.Parse(reader["margin"]);
                        tileset.TileCount = int.Parse(reader["tilecount"]);
                        tileset.Columns = int.Parse(reader["columns"]);
                        break;
                    case "image":
                        string imageSource = reader["source"];
                        tileset.ImageSource = imageSource.Substring(0, imageSource.IndexOf('.'));
                        tileset.ImageWidth = int.Parse(reader["width"]);
                        tileset.ImageHeight = int.Parse(reader["height"]);
                        break;
                    case "properties":
                        ProcessProperties(reader.ReadSubtree(), tileset);
                        break;
                }
            }

            map.Tilesets.Add(tileset);
        }

        /// <summary>
        /// Processes a tile layer from the given XmlReader.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="map"></param>
        private void ProcessTileLayer(XmlReader reader, Map map)
        {
            TileLayer tileLayer = new TileLayer();

            string content = string.Empty;
            List<List<int>> tileData = new List<List<int>>();

            foreach (string name in AllElements(reader))
            {
                switch (name)
                {
                    case "layer":
                        tileLayer.Name = reader["name"];
                        tileLayer.Width = int.Parse(reader["width"]);
                        tileLayer.Height = int.Parse(reader["height"]);
                        break;
                    case "properties":
                        ProcessProperties(reader.ReadSubtree(), tileLayer);
                        break;
                    case "data":
                        content = reader.ReadElementContentAsString();
                        break;
                }
            }

            if (string.IsNullOrEmpty(content))
                return;

            byte[] buffer = Convert.FromBase64String(Regex.Replace(content, @"\s+", ""));
            int[] gids = new int[buffer.Length / sizeof(int)];

            for (int i = 0; i < gids.Length; i++)
                gids[i] = BitConverter.ToInt32(buffer, i * sizeof(int));

            for (int i = 0; i < tileLayer.Height; i++)
            {
                tileData.Add(new List<int>(new int[tileLayer.Width]));

                for (int j = 0; j < tileLayer.Width; j++)
                    tileData[i][j] = gids[i * tileLayer.Width + j];
            }

            tileLayer.TileData = tileData;

            map.Layers.Add(tileLayer);
        }

        /// <summary>
        /// Loads an object group from the given XmlReader.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="map"></param>
        private void ProcessObjectGroup(XmlReader reader, Map map)
        {
            ObjectGroup group = new ObjectGroup();

            foreach (string name in AllElements(reader))
            {
                switch (name)
                {
                    case "objectgroup":
                        group.Name = reader["name"];
                        break;
                    case "object":
                        ProcessSubObject(reader.ReadSubtree(), group);
                        break;
                }
            }

            map.Layers.Add(group);
        }

        /// <summary>
        /// Processes an individual object from the given XmlReader and ObjectGroup.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="group"></param>
        private void ProcessSubObject(XmlReader reader, ObjectGroup group)
        {
            SubObject subObject = new SubObject();

            foreach (string name in AllElements(reader))
            {
                switch (name)
                {
                    case "object":
                        subObject.ID = int.Parse(reader["id"]);

                        if (reader.GetAttribute("type") != null)
                            subObject.Type = reader["type"];

                        subObject.X = float.Parse(reader["x"]);
                        subObject.Y = float.Parse(reader["y"]);

                        if (reader.GetAttribute("width") != null)
                            subObject.Width = float.Parse(reader["width"]);

                        if (reader.GetAttribute("height") != null)
                            subObject.Height = float.Parse(reader["height"]);

                        if (reader.GetAttribute("rotation") != null)
                            subObject.Rotation = float.Parse(reader["rotation"]);
                        break;
                    case "properties":
                        ProcessProperties(reader, subObject);
                        break;
                    default:
                        subObject.VertexDataType = name;

                        if (reader.GetAttribute("points") == null)
                            break;

                        subObject.VertexData = reader["points"];
                        break;
                }
            }

            group.Children.Add(subObject);
        }
    }
}
