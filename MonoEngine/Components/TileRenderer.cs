using MonoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.TMX;
using MonoEngine.ResourceManagement;

namespace MonoEngine.Components
{
    public class TileRenderer : Component
    {
        private Tileset tileset;

        private int _gid;

        /// <summary>
        /// The GID to define which tile should be rendered.
        /// </summary>
        public int GID
        {
            get
            {
                return _gid;
            }
            set
            {
                _gid = value;
                tileset = App.Instance.Scene.Map.GetTilesetByID(_gid);
            }
        }

        /// <summary>
        /// Draws the tile to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        protected override void OnDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (tileset == null)
                return;

            TextureManager.Instance.DrawTile(spriteBatch, tileset, _gid,
                (int)Parent.Position.X, (int)Parent.Position.Y,
                new Vector2(tileset.TileWidth / 2, tileset.TileHeight / 2), Parent.Rotation);
        }
    }
}
