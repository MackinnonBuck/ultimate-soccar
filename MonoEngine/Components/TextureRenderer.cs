using MonoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.ResourceManagement;

namespace MonoEngine.Components
{
    public class TextureRenderer : Component
    {
        string _textureID;

        /// <summary>
        /// The ID of the associated Texture2D.
        /// </summary>
        public string TextureID
        {
            get
            {
                return _textureID;
            }
            set
            {
                _textureID = value;
                Texture = TextureManager.Instance.GetTexture(value);
            }
        }

        /// <summary>
        /// The texture associated with the TextureRenderer.
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// Gets the radius of the texture.
        /// </summary>
        public float Radius
        {
            get
            {
                return (float)Math.Sqrt(Texture.Width * Texture.Width + Texture.Height * Texture.Height);
            }
        }

        protected override void OnInitialize()
        {
        }

        protected override void OnUpdate(GameTime gameTime)
        {
        }

        protected override void OnDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Texture != null)
                spriteBatch.Draw(Texture, Parent.Position, null, Color.White, Parent.Rotation, new Vector2(Texture.Width / 2, Texture.Height / 2), Parent.Scale, SpriteEffects.None, 1);
        }

        protected override void OnDestroy()
        {
        }
    }
}
