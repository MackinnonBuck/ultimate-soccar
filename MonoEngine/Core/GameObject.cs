using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoEngine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Core
{
    public class GameObject : Container<GameObject>, IEntity
    {
        bool isDestroyed;

        /// <summary>
        /// The components of this GameObject.
        /// </summary>
        Container<Component> components;

        /// <summary>
        /// Used to quickly reference the GameObject's PhysicsBody (needed for internal performance).
        /// </summary>
        internal PhysicsBody PhysicsBody { get; private set; }

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

        Vector2 _position;

        /// <summary>
        /// Stores position of the GameObject.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return PhysicsBody == null ? _position : PhysicsBody.Position;
            }
            set
            {
                _position = value;

                if (PhysicsBody != null)
                    PhysicsBody.Position = value;
            }
        }

        float _rotation;

        /// <summary>
        /// Stores the rotation in degrees of the GameObject.
        /// </summary>
        public float Rotation
        {
            get
            {
                return PhysicsBody == null ? _rotation : PhysicsBody.Body.Rotation;
            }
            set
            {
                _rotation = value;

                if (PhysicsBody != null)
                    PhysicsBody.Body.Rotation = value;
            }
        }

        /// <summary>
        /// Stores the scale of the GameObject.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// Creates the GameObject with a parent.
        /// </summary>
        public GameObject(Container<GameObject> parentObject)
        {
            isDestroyed = false;

            components = new Container<Component>();
            _position = Vector2.Zero;
            _rotation = 0f;

            if (parentObject == null)
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
            components.Destroy();
            base.Destroy();

            App.Instance.Scene.RemoveChild(this);
            OnDestroy();

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

            if (component is PhysicsBody)
            {
                if (PhysicsBody == null)
                    PhysicsBody = component as PhysicsBody;
                else
                    Debug.Log("Cannot add more than one PhysicsBody to a GameObject.", Debug.LogSeverity.ERROR);
            }

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
