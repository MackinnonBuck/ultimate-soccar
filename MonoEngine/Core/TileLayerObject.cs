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
    public class TileLayerObject : Entity
    {
        Map map;
        TileLayer tileLayer;
        int layerID;

        public TileLayerObject(Map parentMap, int layer)
        {
            map = parentMap;
            layerID = layer;
        }

        public override void Initialize()
        {
            if ((tileLayer = map.Layers[layerID] as TileLayer) == null)
            {
                Debug.Log("TileLayerRenderer can only render TileLayers.", Debug.LogSeverity.ERROR);
                Destroy();
                return;
            }
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < tileLayer.Height; i++)
            {
                for (int j = 0; j < tileLayer.Width; j++)
                {
                    int id = tileLayer.TileData[i][j];

                    if (id == 0)
                        continue;

                    Tileset tileset = GetTilesetByID(id);

                    DrawTile(spriteBatch, tileset.ImageSource, tileset.Margin, tileset.Spacing, j * tileset.TileWidth, i * tileset.TileHeight,
                        tileset.TileWidth, tileset.TileHeight, (id - tileset.FirstGID) / Math.Max(tileset.Columns, 1),
                        (id - tileset.FirstGID) % Math.Max(tileset.Columns, 1));
                }
            }
        }

        protected override void OnDestroy()
        {
        }

        private Tileset GetTilesetByID(int id)
        {
            for (int i = 0; i < map.Tilesets.Count; i++)
            {
                if (i + 1 < map.Tilesets.Count - 1)
                {
                    if (id >= map.Tilesets[i].FirstGID && id < map.Tilesets[i + 1].FirstGID)
                        return map.Tilesets[i];
                }
                else
                {
                    return map.Tilesets[i];
                }
            }

            Debug.Log("Could not find tileset from ID " + id + ". Returning empty tileset.", Debug.LogSeverity.WARNING);

            return new Tileset();
        }

        private void DrawTile(SpriteBatch spriteBatch, string id, int margin, int spacing, int x, int y, int width, int height,
            int row, int column)
        {
            Rectangle srcRect;
            srcRect.X = margin + (spacing + width) * column;
            srcRect.Y = margin + (spacing + height) * row;
            srcRect.Width = width;
            srcRect.Height = height;

            TextureManager.Instance.Draw(spriteBatch, id, new Vector2(x, y), srcRect, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, layerID);
        }
    }
}
