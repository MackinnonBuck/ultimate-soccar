﻿using MonoEngine.Core;
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
        public string TextureID { get; set; }

        /// <summary>
        /// Draws the texture to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        protected override void OnDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (TextureID != null)
                TextureManager.Instance.Draw(spriteBatch, TextureID, Parent.Position, null, Color.White, Parent.Rotation, null, Parent.Scale, SpriteEffects.None, 1);
        }
    }
}
