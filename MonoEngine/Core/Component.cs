using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Components;
using MonoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Core
{
    public abstract class Component : Entity
    {
        /// <summary>
        /// Called when the Component is initialized.
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <summary>
        /// Called when the Component is updated.
        /// </summary>
        protected virtual void OnUpdate(GameTime gameTime) { }

        /// <summary>
        /// Called when the Component is drawn.
        /// </summary>
        protected virtual void OnDraw(SpriteBatch spriteBatch, GameTime gameTime) { }

        /// <summary>
        /// The Parent GameObject.
        /// </summary>
        public GameObject Parent { get; private set; }

        /// <summary>
        /// Creates a new instance of the given component.
        /// This method must be used to create components instead of calling the component's constructor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        internal static T Create<T>(GameObject parent) where T : Component, new()
        {
            T component = new T();
            component.Parent = parent;
            return component;
        }

        /// <summary>
        /// Initializes the Component.
        /// </summary>
        public override void Initialize()
        {
            OnInitialize();
        }

        /// <summary>
        /// Updates the Component.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            OnUpdate(gameTime);
        }

        /// <summary>
        /// Draws the Component.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            OnDraw(spriteBatch, gameTime);
        }
    }
}
