using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Core
{
    public abstract class Component
    {
        /// <summary>
        /// Called when the Component is initialized.
        /// </summary>
        protected abstract void OnInitialize();

        /// <summary>
        /// Called when the Component is updated.
        /// </summary>
        protected abstract void OnUpdate(GameTime gameTime);

        /// <summary>
        /// Called when the Component is drawn.
        /// </summary>
        protected abstract void OnDraw(SpriteBatch spriteBatch, GameTime gameTime);

        /// <summary>
        /// Called when the Component is destroyed.
        /// </summary>
        protected abstract void OnDestroy();

        /// <summary>
        /// Used for determining if the Component has been destroyed.
        /// </summary>
        public bool IsDestroyed { get; private set; }

        GameObject _parent;

        /// <summary>
        /// The parent GameObject.
        /// </summary>
        public GameObject Parent
        {
            get
            {
                return _parent;
            }
            internal set
            {
                if (_parent == null)
                    _parent = value;
                else
                    Debug.Log("Cannot change a component's parent after creation.", Debug.LogSeverity.ERROR);
            }
        }

        /// <summary>
        /// Initializes the Component.
        /// </summary>
        public void Initialize()
        {
            IsDestroyed = false;

            OnInitialize();
        }

        /// <summary>
        /// Updates the Component.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            OnUpdate(gameTime);
        }

        /// <summary>
        /// Draws the Component.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            OnDraw(spriteBatch, gameTime);
        }

        /// <summary>
        /// Destroys the Component.
        /// </summary>
        public void Destroy()
        {
            Parent.RemoveComponent(this);
            OnDestroy();

            IsDestroyed = true;
        }
    }
}
