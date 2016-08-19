using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateSocCar.Engine
{
    public class GameObject
    {
        /// <summary>
        /// Called when the GameObject is initialized.
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <summary>
        /// Called when the GameObject is updated.
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void OnUpdate(GameTime gameTime) {  }

        /// <summary>
        /// Called when the GameObject is Drawn.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="gameTime"></param>
        protected virtual void OnDraw(SpriteBatch spriteBatch, GameTime gameTime) { }

        /// <summary>
        /// Called when the GameObject is destroyed.
        /// </summary
        protected virtual void OnDestroy() { }

        private SafeList<Component> components;

        /// <summary>
        /// Initializes the GameObject.
        /// </summary>
        public GameObject()
        {
            components = new SafeList<Component>();

            App.Instance.Scene._AddGameObject(this);
            OnInitialize();
        }

        /// <summary>
        /// Updates the GameObject.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            foreach (Component c in components)
                c.Update(gameTime);

            OnUpdate(gameTime);
        }

        /// <summary>
        /// Draws the GameObject.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (Component c in components)
                c.Draw(spriteBatch, gameTime);

            OnDraw(spriteBatch, gameTime);
        }

        /// <summary>
        /// Destroys the GameObject.
        /// </summary>
        public void Destroy()
        {
            foreach (Component c in components)
                c.Destroy();

            App.Instance.Scene._RemoveGameObject(this);
            OnDestroy();
        }

        /// <summary>
        /// Adds a Component of the given type to the GameObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Component AddComponent<T>() where T : Component, new()
        {
            T component = new T();
            component.Parent = this;
            components.Add(component);

            return component;
        }

        /// <summary>
        /// Removes a Component from the GameObject (only to be called by core engine).
        /// </summary>
        /// <param name="component"></param>
        public void _RemoveComponent(Component component)
        {
            if (components.Contains(component))
                return;

            components.Remove(component);
        }
    }
}
