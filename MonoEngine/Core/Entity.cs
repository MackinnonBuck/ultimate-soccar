using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Core
{
    /// <summary>
    /// Defines basic behavior for Scenes, GameObjects, Components, etc.
    /// </summary>
    public abstract class Entity
    {
        public bool Destroyed { get; private set; }

        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        protected abstract void OnDestroy();
        public void Destroy()
        {
            Destroyed = true;
            OnDestroy();
        }
    }
}
