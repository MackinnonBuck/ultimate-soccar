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
    public class ImageLayer : Entity
    {
        TMX.ImageLayer layer;

        /// <summary>
        /// Initializes a new ImageLayer.
        /// </summary>
        /// <param name="imageLayer"></param>
        public ImageLayer(TMX.ImageLayer imageLayer)
        {
            layer = imageLayer;

            TextureManager.Instance.Load(layer.Source, layer.Source);
        }

        /// <summary>
        /// Draws the ImageLayer to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            TextureManager.Instance.Draw(spriteBatch, layer.Source, new Vector2(layer.OffsetX, layer.OffsetY),
                null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
