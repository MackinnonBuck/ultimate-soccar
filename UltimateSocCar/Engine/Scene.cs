using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateSocCar.Engine
{
    public abstract class Scene
    {
        /// <summary>
        /// Called when the Scene is initialized.
        /// </summary>
        protected abstract void OnInitialize();

        /// <summary>
        /// Called when the Scene is updated.
        /// </summary>
        /// <param name="gameTime"></param>
        protected abstract void OnUpdate(GameTime gameTime);

        /// <summary>
        /// Called before the Scene is drawn.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="gameTime"></param>
        protected abstract void OnPreDraw(SpriteBatch spriteBatch, GameTime gameTime);

        /// <summary>
        /// Called after the Scene is drawn.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="gameTime"></param>
        protected abstract void OnPostDraw(SpriteBatch spriteBatch, GameTime gameTime);

        /// <summary>
        /// Called when the Scene is quitting.
        /// </summary>
        protected abstract void OnQuit();
        
        private SafeList<GameObject> gameObjects = new SafeList<GameObject>();

        /// <summary>
        /// Initializes the Scene.
        /// </summary>
        public void Initialize()
        {
            OnInitialize();
        }

        /// <summary>
        /// Updates the Scene.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            foreach (GameObject g in gameObjects)
                g.Update(gameTime);

            OnUpdate(gameTime);
        }

        /// <summary>
        /// Draws the Scene.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            OnPreDraw(spriteBatch, gameTime);

            foreach (GameObject g in gameObjects)
                g.Draw(spriteBatch, gameTime);

            OnPostDraw(spriteBatch, gameTime);
        }

        /// <summary>
        /// Quits the Scene.
        /// </summary>
        public void Quit()
        {
            foreach (GameObject g in gameObjects)
                g.Destroy();

            OnQuit();
        }
        
        /// <summary>
        /// Finds and returns each GameObject of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>each GameObject of the given type.</returns>
        public List<T> FindGameObjects<T>() where T : GameObject
        {
            return new List<T>(gameObjects.OfType<T>());
        }

        /// <summary>
        /// Finds and returns the first found GameObject of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>the first found GameObject of the given type.</returns>
        public T FindGameObject<T>() where T : GameObject
        {
            List<T> objects = FindGameObjects<T>();

            if (objects.Count > 0)
                return objects[0];

            return null;
        }

        /// <summary>
        /// Adds a GameObject to the Scene (only to be called by core engine).
        /// </summary>
        /// <param name="gameObject"></param>
        public void _AddGameObject(GameObject gameObject)
        {
            if (gameObjects.Contains(gameObject))
                return;

            gameObjects.Add(gameObject);
        }

        /// <summary>
        /// Removes a GameObject from the Scene (only to be called by core engine).
        /// </summary>
        /// <param name="gameObject"></param>
        public void _RemoveGameObject(GameObject gameObject)
        {
            if (!gameObjects.Contains(gameObject))
                return;

            gameObjects.Remove(gameObject);
        }
    }
}
