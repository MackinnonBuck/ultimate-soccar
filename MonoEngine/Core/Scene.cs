using FarseerPhysics;
using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Components;
using MonoEngine.ResourceManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Core
{
    public abstract class Scene : Container<GameObject>
    {
        /// <summary>
        /// The physics world of the scene.
        /// </summary>
        internal World PhysicsWorld { get; private set; }

        /// <summary>
        /// The debug drawer of the physics world.
        /// </summary>
        DebugViewXNA debugView;

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
        protected abstract void OnDestroy();

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
        public override void Update(GameTime gameTime)
        {
            PhysicsWorld.Step(App.Instance.TargetElapsedTime.Milliseconds * 0.001f);

            base.Update(gameTime);
            OnUpdate(gameTime);
        }

        /// <summary>
        /// Draws the Scene.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="gameTime"></param>
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            OnPreDraw(spriteBatch, gameTime);
            base.Draw(spriteBatch, gameTime);
            OnPostDraw(spriteBatch, gameTime);

            Matrix proj = Matrix.CreateOrthographicOffCenter(0f, ConvertUnits.ToSimUnits(App.Instance.GraphicsDevice.Viewport.Width), ConvertUnits.ToSimUnits(App.Instance.GraphicsDevice.Viewport.Height), 0f, 0f, 1f);
            debugView.RenderDebugData(ref proj);
        }

        /// <summary>
        /// Quits the Scene.
        /// </summary>
        public override void Destroy()
        {
            base.Destroy();
            PhysicsWorld.Clear();
            debugView.Dispose();

            OnDestroy();

            TextureManager.Instance.Clear();
            App.Instance.Content.Unload();
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
    }
}
