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
    public class Container<T> : IEnumerable<T> where T : Entity
    {
        /// <summary>
        /// The list of child Entities.
        /// </summary>
        protected List<T> Children { get; private set; }

        /// <summary>
        /// Initializes a new Entity.
        /// </summary>
        public Container()
        {
            Children = new List<T>();
        }

        /// <summary>
        /// Finds and returns each Entity of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>each GameObject of the given type.</returns>
        public List<U> GetAll<U>() where U : T
        {
            return Children.OfType<U>().ToList();
        }

        /// <summary>
        /// Finds and returns the first found Entity of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>the first found GameObject of the given type.</returns>
        public U Get<U>() where U : T
        {
            List<U> objects = GetAll<U>();

            if (objects.Count > 0)
                return objects[0];

            return null;
        }

        /// <summary>
        /// Adds the given entity to the list of children.
        /// </summary>
        /// <param name="entity"></param>
        public U Add<U>(U entity) where U : T
        {
            Children.Add(entity);
            entity.Initialize();

            return entity;
        }

        /// <summary>
        /// Removes an Entity from the Container.
        /// </summary>
        /// <param name="entity"></param>
        internal void Remove(T entity)
        {
            if (!Children.Contains(entity))
                return;

            Children.Remove(entity);
        }

        /// <summary>
        /// Enumerates safely through each child Entity.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> Enumerate()
        {
            foreach (T t in Children.ToList())
                if (!t.Destroyed)
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
        /// Updates each child Entity.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            foreach (T t in this)
                t.Update(gameTime);
        }

        /// <summary>
        /// Draws each child Entity.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (T t in this)
                t.Draw(spriteBatch, gameTime);
        }

        /// <summary>
        /// Destroys each child Entity.
        /// </summary>
        public virtual void Destroy()
        {
            foreach (T t in this)
                t.Destroy();
        }
    }
}
