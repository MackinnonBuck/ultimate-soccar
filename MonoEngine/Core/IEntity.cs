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
    public interface IEntity
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        void Destroy();
        bool IsDestroyed();
    }
}
