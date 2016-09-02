using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoEngine.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Core
{
    public class GameObject : Container<GameObject>, IEntity
    {
        bool isDestroyed;

        /// <summary>
        /// Manages rebindable properties of the GameObject.
        /// </summary>
        internal PropertyBinder PropertyBinder { get; private set; }

        /// <summary>
        /// The components of this GameObject.
        /// </summary>
        Container<Component> components;

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
        /// The parent of this GameObject.
        /// </summary>
        public GameObject Parent { get; private set; }

        public Vector2 Position
        {
            get
            {
                return (Vector2)PropertyBinder["Position"].Value;
            }
            set
            {
                PropertyBinder["Position"].Value = value;
            }
        }

        /// <summary>
        /// Stores the rotation in degrees of the GameObject.
        /// </summary>
        public float Rotation
        {
            get
            {
                return (float)PropertyBinder["Rotation"].Value;
            }
            set
            {
                PropertyBinder["Rotation"].Value = value;
            }
        }

        /// <summary>
        /// Stores the scale of the GameObject.
        /// </summary>
        public Vector2 Scale
        {
            get
            {
                return (Vector2)PropertyBinder["Scale"].Value;
            }
            set
            {
                PropertyBinder["Scale"].Value = value;
            }
        }

        /// <summary>
        /// Creates the GameObject with a parent.
        /// </summary>
        public GameObject(Container<GameObject> parentObject)
        {
            PropertyBinder = new PropertyBinder();

            isDestroyed = false;

            components = new Container<Component>();
            Position = Vector2.Zero;
            Rotation = 0f;
            Scale = Vector2.One;

            if (parentObject == null || !(parentObject is GameObject))
            {
                App.Instance.Scene.AddChild(this);
            }
            else
            {
                Parent = (GameObject)parentObject;
                parentObject.AddChild(this);
            }

            OnInitialize();
        }

        /// <summary>
        /// Creates the GameObject without a parent.
        /// </summary>
        public GameObject() : this(null)
        {
        }

        /// <summary>
        /// Updates the GameObject.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            components.Update(gameTime);
            base.Update(gameTime);

            OnUpdate(gameTime);
        }

        /// <summary>
        /// Draws the GameObject.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="gameTime"></param>
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            components.Draw(spriteBatch, gameTime);
            base.Draw(spriteBatch, gameTime);
            OnDraw(spriteBatch, gameTime);
        }

        /// <summary>
        /// Destroys the GameObject.
        /// </summary>
        public override void Destroy()
        {
            base.Destroy();
            components.Destroy();

            OnDestroy();

            if (Parent == null)
                App.Instance.Scene.RemoveChild(this);
            else
                Parent.RemoveChild(this);

            isDestroyed = true;
        }

        /// <summary>
        /// Used for determining if the GameObject has been destroyed.
        /// </summary>
        /// <returns></returns>
        bool IEntity.IsDestroyed()
        {
            return isDestroyed;
        }

        /// <summary>
        /// Adds a Component to the GameObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T AddComponent<T>() where T : Component, new()
        {
            T component = new T();
            components.AddChild(component);
            component.Parent = this;
            component.Initialize();

            return component;
        }

        /// <summary>
        /// Gets all Components of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetComponents<T>() where T : Component
        {
            return components.GetChildren<T>();
        }

        /// <summary>
        /// Gets the first Component of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : Component
        {
            List<T> components = GetComponents<T>();

            if (components.Count > 0)
                return components[0];

            return null;
        }

        /// <summary>
        /// Removes the component from this GameObject.
        /// </summary>
        /// <param name="component"></param>
        internal void RemoveComponent(Component component)
        {
            components.RemoveChild(component);
        }
    }
}
