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
    public class GameObject : Entity
    {
        /// <summary>
        /// Manages rebindable properties of the GameObject.
        /// </summary>
        internal PropertyBinder PropertyBinder { get; private set; }

        /// <summary>
        /// The GameObject children of this GameObject.
        /// </summary>
        private Container<GameObject> children;

        /// <summary>
        /// The components of this GameObject.
        /// </summary>
        private Container<Component> components;

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
        /// The parent of this GameObject.
        /// </summary>
        public GameObject Parent { get; private set; }

        /// <summary>
        /// The position of the GameObject.
        /// </summary>
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
        private GameObject(GameObject parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Creates the GameObject without a parent.
        /// </summary>
        private GameObject() : this(null)
        {
        }

        /// <summary>
        /// Used for creating an instance of GameObject with the given parent.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static GameObject Create(GameObject parent)
        {
            return parent == null ? App.Instance.ActiveScene.Children.Add(new GameObject())
                : parent.children.Add(new GameObject(parent));
        }

        /// <summary>
        /// Used for creating an instance of GameObjet without a parent.
        /// </summary>
        /// <returns></returns>
        public static GameObject Create()
        {
            return Create(null);
        }

        /// <summary>
        /// Returns all GameObject children of the given parent.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static List<GameObject> GetAll(GameObject parent = null)
        {
            return parent == null ? App.Instance.ActiveScene.Children.GetAll<GameObject>() : parent.children.GetAll<GameObject>();
        }

        /// <summary>
        /// Returns the first child GameObject of the given parent.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static GameObject Get(GameObject parent = null)
        {
            return parent == null ? App.Instance.ActiveScene.Children.Get<GameObject>() : parent.children.Get<GameObject>();
        }

        /// <summary>
        /// Adds a component of the given type to the GameObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddComponent<T>() where T : Component, new()
        {
            T component = Component.Create<T>(this);
            components.Add(component);
            return component;
        }

        /// <summary>
        /// Returns all components of the given type associated with the GameObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetComponents<T>() where T : Component
        {
            return components.GetAll<T>();
        }

        /// <summary>
        /// Returns the first component of the given type associated with the GameObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : Component
        {
            return components.Get<T>();
        }

        /// <summary>
        /// Initializes the GameObject.
        /// </summary>
        public override void Initialize()
        {
            PropertyBinder = new PropertyBinder();

            components = new Container<Component>();
            children = new Container<GameObject>();
            Position = Vector2.Zero;
            Rotation = 0f;
            Scale = Vector2.One;

            OnInitialize();
        }

        /// <summary>
        /// Updates the GameObject.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            components.Update(gameTime);
            children.Update(gameTime);

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
            children.Draw(spriteBatch, gameTime);
            OnDraw(spriteBatch, gameTime);
        }

        /// <summary>
        /// Destroys the GameObject.
        /// </summary>
        protected override void OnDestroy()
        {
            children.Destroy();
            components.Destroy();

            if (Parent == null)
                App.Instance.ActiveScene.Children.Remove(this);
            else
                Parent.children.Remove(this);
        }
    }
}
