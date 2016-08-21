using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Core
{
    public class GameObject
    {
        private SafeList<Component> components;
        private PhysicsBody physicsBody;
        private Vector2 _position;
        private float _rotation;

        /// <summary>
        /// Called when the GameObject is initialized.
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <summary>
        /// Called when the GameObject is updated.
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void OnUpdate(GameTime gameTime) { }

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
        public Vector2 Position
        {
            get
            {
                return physicsBody == null ? _position : ConvertUnits.ToDisplayUnits(physicsBody.Body.Position);
            }
            set
            {
                _position = value;

                if (physicsBody != null)
                    physicsBody.Body.Position = ConvertUnits.ToSimUnits(value);
            }
        }

        /// <summary>
        /// Stores the rotation in degrees of the GameObject.
        /// </summary>
        public float Rotation
        {
            get
            {
                return physicsBody == null ? _rotation : physicsBody.Body.Rotation;
            }
            set
            {
                _rotation = value;

                if (physicsBody != null)
                    physicsBody.Body.Rotation = value;
            }
        }

        /// <summary>
        /// Stores the scale of the GameObject.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// Initializes the GameObject.
        /// </summary>
        public GameObject()
        {
            components = new SafeList<Component>();
            _position = Vector2.Zero;
            _rotation = 0f;
            
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

            components.Clear();

            App.Instance.Scene.RemoveGameObject(this);
            OnDestroy();
        }

        /// <summary>
        /// Finds and returns each Component of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetComponents<T>() where T : Component
        {
            return components.OfType<T>().ToList();
        }

        /// <summary>
        /// Finds and returns the first found GameObject of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : Component
        {
            List<T> comps = GetComponents<T>();

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

            if (component is PhysicsBody)
                physicsBody = component as PhysicsBody;

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

            if (component is PhysicsBody)
                physicsBody = null;

            components.Remove(component);
        }
    }
}
