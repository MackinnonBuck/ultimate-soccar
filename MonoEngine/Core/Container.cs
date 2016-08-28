using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace MonoEngine.Core
{
    public class Container<T> : IEnumerable<T> where T : IEntity
    {
        /// <summary>
        /// The list of child IEntities.
        /// </summary>
        protected List<T> Children { get; private set; }

        /// <summary>
        /// Initializes a new IEntity.
        /// </summary>
        public Container()
        {
            Children = new List<T>();
        }

        /// <summary>
        /// Finds and returns each IEntity of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>each GameObject of the given type.</returns>
        public List<U> GetChildren<U>() where U : T
        {
            return Children.OfType<U>().ToList();
        }

        /// <summary>
        /// Finds and returns the first found IEntity of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>the first found GameObject of the given type.</returns>
        public U GetChild<U>() where U : T
        {
            List<U> objects = GetChildren<U>();

            if (objects.Count > 0)
                return objects[0];

            return default(U);
        }

        /// <summary>
        /// Adds an IEntity to the Scene.
        /// </summary>
        /// <param name="entity"></param>
        internal void AddChild(T entity)
        {
            if (Children.Contains(entity))
                return;

            Children.Add(entity);
        }

        /// <summary>
        /// Removes an IEntity from the Scene.
        /// </summary>
        /// <param name="entity"></param>
        internal void RemoveChild(T entity)
        {
            if (!Children.Contains(entity))
                return;

            Children.Remove(entity);
        }

        /// <summary>
        /// Enumerates safely through each child IEntity.s
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> Enumerate()
        {
            foreach (T t in Children.ToList())
                if (!t.IsDestroyed())
                    yield return t;
        }

        /// <summary>
        /// Gets the enumerator from the specified type.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return Enumerate().GetEnumerator();
        }

        /// <summary>
        /// Gets a generic enumerator.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Updates each child IEntity.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            foreach (T t in this)
                t.Update(gameTime);
        }

        /// <summary>
        /// Draws each child GameObject.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (T t in this)
                t.Draw(spriteBatch, gameTime);
        }

        /// <summary>
        /// Destroys each child GameObject.
        /// </summary>
        public virtual void Destroy()
        {
            foreach (T t in this)
                t.Destroy();
        }
    }
}
