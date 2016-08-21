using FarseerPhysics;
using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
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
    public abstract class Scene
    {
        /// <summary>
        /// The physics world of the scene.
        /// </summary>
        internal World PhysicsWorld { get; private set; }

        DebugViewXNA debugView;

        SafeList<GameObject> gameObjects;

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

        /// <summary>
        /// The gravity of the physics world.
        /// </summary>
        public Vector2 Gravity
        {
            get
            {
                return PhysicsWorld.Gravity;
            }
            set
            {
                PhysicsWorld.Gravity = Gravity;
            }
        }

        /// <summary>
        /// Determines if debug drawing is enabled.
        /// </summary>
        public bool DebugDrawEnabled
        {
            get
            {
                return debugView.Enabled;
            }
            set
            {
                debugView.Enabled = value;
            }
        }

        /// <summary>
        /// Creates a new Scene instance.
        /// </summary>
        public Scene()
        {
            gameObjects = new SafeList<GameObject>();
        }

        /// <summary>
        /// Initializes the Scene.
        /// </summary>
        public void Initialize()
        {
            PhysicsWorld = new World(new Vector2(0f, 9.81f));
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);

            debugView = new DebugViewXNA(PhysicsWorld);
            debugView.AppendFlags(DebugViewFlags.DebugPanel);
            debugView.AppendFlags(DebugViewFlags.ContactPoints);
            debugView.AppendFlags(DebugViewFlags.ContactNormals);
            debugView.LoadContent(App.Instance.GraphicsDevice, App.Instance.Content);
            debugView.Enabled = false;

            OnInitialize();
        }

        /// <summary>
        /// Updates the Scene.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            PhysicsWorld.Step(App.Instance.TargetElapsedTime.Milliseconds * 0.001f);

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

            Matrix proj = Matrix.CreateOrthographicOffCenter(0f, ConvertUnits.ToSimUnits(App.Instance.GraphicsDevice.Viewport.Width), ConvertUnits.ToSimUnits(App.Instance.GraphicsDevice.Viewport.Height), 0f, 0f, 1f);
            debugView.RenderDebugData(ref proj);
        }

        /// <summary>
        /// Quits the Scene.
        /// </summary>
        public void Quit()
        {
            foreach (GameObject g in gameObjects)
                g.Destroy();

            PhysicsWorld.Clear();
            gameObjects.Clear();
            debugView.Dispose();

            OnQuit();
        }

        /// <summary>
        /// Returns a list of PhysicsBodies located at the given point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>true if a body exists at the given point.</returns>
        public List<PhysicsBody> GetBodiesAt(Vector2 point)
        {
            return PhysicsWorld.TestPointAll(ConvertUnits.ToSimUnits(point)).ConvertAll(fixture => (PhysicsBody)fixture.Body.UserData);
        }

        /// <summary>
        /// Returns the first PhysicsBody located at the given point. If no PhysicsBody is found, null is returned.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>the first PhysicsBody located at the given point, or null of no PhysicsBody is found.</returns>
        public PhysicsBody GetBodyAt(Vector2 point)
        {
            Fixture fixture = PhysicsWorld.TestPoint(ConvertUnits.ToSimUnits(point));

            if (fixture != null)
                return (PhysicsBody)fixture.Body.UserData;

            return null;
        }
        
        /// <summary>
        /// Finds and returns each GameObject of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>each GameObject of the given type.</returns>
        public List<T> FindGameObjects<T>() where T : GameObject
        {
            return gameObjects.OfType<T>().ToList();
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
        /// Adds a GameObject to the Scene.
        /// </summary>
        /// <param name="gameObject"></param>
        internal void AddGameObject(GameObject gameObject)
        {
            if (gameObjects.Contains(gameObject))
                return;

            gameObjects.Add(gameObject);
        }

        /// <summary>
        /// Removes a GameObject from the Scene.
        /// </summary>
        /// <param name="gameObject"></param>
        internal void RemoveGameObject(GameObject gameObject)
        {
            if (!gameObjects.Contains(gameObject))
                return;

            gameObjects.Remove(gameObject);
        }
    }
}
