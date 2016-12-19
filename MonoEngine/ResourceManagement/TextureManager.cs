using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.ResourceManagement
{
    public class TextureManager
    {
        Dictionary<string, Texture2D> textures;

        static TextureManager _instance;

        /// <summary>
        /// The global TextureManager instance.
        /// </summary>
        public static TextureManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TextureManager();

                return _instance;
            }
        }

        /// <summary>
        /// Initializes the TextureManager instance.
        /// </summary>
        private TextureManager()
        {
            textures = new Dictionary<string, Texture2D>();
        }

        /// <summary>
        /// Loads a Texture2D from the given asset name.
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="id"></param>
        public bool Load(string assetName, string id)
        {
            try
            {
                textures.Add(id, App.Instance.Content.Load<Texture2D>(assetName));
                return true;
            }
            catch (ContentLoadException)
            {
                return false;
            }
        }

        /// <summary>
        /// Removes all references to existing textures.
        /// </summary>
        public void Clear()
        {
            textures.Clear();
        }

        /// <summary>
        /// Returns the texture with the given ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Texture2D GetTexture(string id)
        {
            if (textures.ContainsKey(id))
                return textures[id];

            return null;
        }

        /// <summary>
        /// Draws an loaded texture.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="id"></param>
        /// <param name="position"></param>
        /// <param name="sourceRectangle"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        /// <param name="scale"></param>
        /// <param name="effects"></param>
        /// <param name="layerDepth"></param>
        public void Draw(SpriteBatch spriteBatch, string id, Vector2 position, Rectangle? sourceRectangle, Color color,
            float rotation, Vector2? origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            if (textures.ContainsKey(id))
            {
                Texture2D texture = textures[id];
                Vector2 textureOrigin = origin ?? new Vector2(texture.Width / 2, texture.Height / 2);
                spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, textureOrigin, scale, effects, layerDepth);
            }            
        }
    }
}
