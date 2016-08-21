using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Core
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
        /// </summary>
        protected virtual void OnDestroy() { }

        /// <summary>
        /// Stores position of the GameObject.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Stores the scale of the GameObject.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// Stores the rotation in degrees of the GameObject.
        /// </summary>
        public float Rotation { get; set; }

        private SafeList<Component> components;

        /// <summary>
        /// Initializes the GameObject.
        /// </summary>
        public GameObject()
        {
            components = new SafeList<Component>();
            
            App.Instance.Scene.AddGameObject(this);
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

            App.Instance.Scene.RemoveGameObject(this);
            OnDestroy();
        }

        /// <summary>
        /// Finds and returns each Component of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> FindComponents<T>() where T : Component
        {
            return components.OfType<T>().ToList<T>();
        }

        /// <summary>
        /// Finds and returns the first found GameObject of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T FindComponent<T>() where T : Component
        {
            List<T> comps = FindComponents<T>();

            if (comps.Count > 0)
                return comps[0];

            return null;
        }

        /// <summary>
        /// Adds a Component of the given type to the GameObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddComponent<T>() where T : Component, new()
        {
            T component = new T();
            component.Parent = this;
            components.Add(component);
            component.Initialize();

            return component;
        }

        /// <summary>
        /// Removes a Component from the GameObject (only to be called by core engine).
        /// </summary>
        /// <param name="component"></param>
        internal void RemoveComponent(Component component)
        {
            if (components.Contains(component))
                return;

            components.Remove(component);
        }
    }
}
