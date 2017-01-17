using MonoEngine.TMX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.ResourceManagement;

namespace MonoEngine.Core
{
    public class TileLayer : Entity
    {
        TMX.TileLayer tileLayer;

        /// <summary>
        /// Initializes a new TileLayerObject.
        /// </summary>
        /// <param name="parentMap"></param>
        /// <param name="layer"></param>
        public TileLayer(TMX.TileLayer layer)
        {
            tileLayer = layer;
        }

        /// <summary>
        /// Renders the tiles to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < tileLayer.Height; i++)
            {
                for (int j = 0; j < tileLayer.Width; j++)
                {
                    int id = tileLayer.TileData[i][j];

                    if (id == 0)
                        continue;

                    Tileset tileset = App.Instance.Scene.Map.GetTilesetByID(id);
                    TextureManager.Instance.DrawTile(spriteBatch, App.Instance.Scene.Map.GetTilesetByID(id), id,
                        j * tileset.TileWidth, i * tileset.TileHeight);
                }
            }
        }
    }
}
