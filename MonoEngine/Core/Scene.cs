using FarseerPhysics;
using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Components;
using MonoEngine.Utilities;
using MonoEngine.ResourceManagement;
using MonoEngine.TMX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Core
{
    public class Scene
    {
        /// <summary>
        /// The path associated with the .tmx file to be loaded.
        /// </summary>
        private string tmxPath;

        /// <summary>
        /// The debug drawer of the physics world.
        /// </summary>
        DebugViewXNA debugView;

        /// <summary>
        /// The container for all of the scene's child entities.
        /// </summary>
        internal Container<Entity> Children { get; private set; }

        /// <summary>
        /// The physics world of the scene.
        /// </summary>
        public World PhysicsWorld { get; private set; }

        /// <summary>
        /// Called when the scene is loaded from a .tmx file.
        /// </summary>
        protected virtual void OnLoad() { }

        /// <summary>
        /// Called when the Scene is initialized.
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <summary>
        /// Called when the Scene is updated.
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void OnUpdate(GameTime gameTime) { }

        /// <summary>
        /// Called before the Scene is drawn.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="gameTime"></param>
        protected virtual void OnPreDraw(SpriteBatch spriteBatch, GameTime gameTime) { }

        /// <summary>
        /// Called after the Scene is drawn.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="gameTime"></param>
        protected virtual void OnPostDraw(SpriteBatch spriteBatch, GameTime gameTime) {  }

        /// <summary>
        /// Called when the Scene is quitting.
        /// </summary>
        protected virtual void OnDestroy() { }

        /// <summary>
        /// The TMX Map associated with the scene.
        /// </summary>
        public Map Map { get; private set; }

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
        /// Gets the Camera associated with the scene.
        /// </summary>
        public Camera Camera { get; private set; }

        /// <summary>
        /// Creates a new Scene instance.
        /// </summary>
        public Scene(string mapPath = null)
        {
            tmxPath = mapPath;
        }

        /// <summary>
        /// Loads the given Map into the scene.
        /// </summary>
        /// <param name="map"></param>
        private void LoadMap()
        {
            Map = App.Instance.Content.Load<Map>(tmxPath);

            foreach (KeyValuePair<string, string> property in Map.Properties)
            {
                switch (property.Key)
                {
                    case "gravity":
                        PhysicsWorld.Gravity = Parsing.TryParseVector2(property.Value) ?? PhysicsWorld.Gravity;
                        break;
                }
            }

            foreach (Tileset tileset in Map.Tilesets)
            {
                TextureManager.Instance.Load(tileset.Source, tileset.Source);
            }

            for (int i = 0; i < Map.Layers.Count; i++)
            {
                if (Map.Layers[i] is TMX.TileLayer)
                {
                    Children.Add(new TileLayer((TMX.TileLayer)Map.Layers[i]));
                }
                else if (Map.Layers[i] is ObjectGroup)
                {
                    ObjectGroup group = (ObjectGroup)Map.Layers[i];
                    
                    foreach (SubObject sub in group.Children)
                        GameObjectFactory.Instance.Create(sub.Type ?? "DefaultDefinition", sub);
                }
                else if (Map.Layers[i] is TMX.ImageLayer)
                {
                    Children.Add(new ImageLayer((TMX.ImageLayer)Map.Layers[i]));
                }
            }
            
            OnLoad();
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

            Children = new Container<Entity>();
            Camera = Camera.Create();

            if (tmxPath != null)
                LoadMap();

            OnInitialize();
        }

        /// <summary>
        /// Updates the Scene.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            PhysicsWorld.Step(App.Instance.TargetElapsedTime.Milliseconds * 0.001f);

            Children.Update(gameTime);
            OnUpdate(gameTime);
        }

        /// <summary>
        /// Draws the Scene.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(transformMatrix: Camera.ViewMatrix, samplerState: SamplerState.PointClamp);

            OnPreDraw(spriteBatch, gameTime);
            Children.Draw(spriteBatch, gameTime);
            OnPostDraw(spriteBatch, gameTime);

            spriteBatch.End();
            
            if (DebugDrawEnabled)
            {
                Matrix proj = Matrix.CreateOrthographicOffCenter(0f, ConvertUnits.ToSimUnits(App.Instance.GraphicsDevice.Viewport.Width), ConvertUnits.ToSimUnits(App.Instance.GraphicsDevice.Viewport.Height), 0f, 0f, 1f);
                debugView.RenderDebugData(proj, Camera.SimViewMatrix);
            }
        }

        /// <summary>
        /// Quits the Scene.
        /// </summary>
        public void Destroy()
        {
            Children.Destroy();
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
        public List<BodyComponent> GetBodiesAt(Vector2 point)
        {
            return PhysicsWorld.TestPointAll(ConvertUnits.ToSimUnits(point)).ConvertAll(fixture => (BodyComponent)fixture.Body.UserData);
        }

        /// <summary>
        /// Returns the first PhysicsBody located at the given point. If no PhysicsBody is found, null is returned.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>the first PhysicsBody located at the given point, or null of no PhysicsBody is found.</returns>
        public BodyComponent GetBodyAt(Vector2 point)
        {
            Fixture fixture = PhysicsWorld.TestPoint(ConvertUnits.ToSimUnits(point));

            if (fixture != null)
                return (BodyComponent)fixture.Body.UserData;

            return null;
        }
    }
}
