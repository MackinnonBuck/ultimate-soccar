using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Factories;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using MonoEngine.Core;
using MonoEngine.Components;
using MonoEngine.ResourceManagement;

namespace UltimateSocCar.Testing
{
    public class TestScene : Scene
    {
        BodyComponent movingBody;
        string map;

        public TestScene(string mapPath)
            : base(mapPath)
        {
            map = mapPath;
        }

        protected override void OnInitialize()
        {
            DebugDrawEnabled = true;
            TextureManager.Instance.Load("Textures/Pin", "Pin");
            TextureManager.Instance.Load("Textures/Earth", "Earth");

            Input.Instance.OnKeyPressed += OnKeyPressed;
        }

        private void OnKeyPressed(object sender, Input.KeyboardInputEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.R:
                    App.Instance.ChangeScene(new TestScene(map));
                    break;
                case Keys.D:
                    DebugDrawEnabled = !DebugDrawEnabled;
                    break;
            }
        }

        protected override void OnDestroy()
        {
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (Input.Instance.IsKeyDown(Keys.LeftControl))
                Camera.Position -= Input.Instance.MouseSpeed;

            if (Input.Instance.IsMouseButtonDown(Input.MouseButtons.LEFT) || Input.Instance.IsMouseButtonDown(Input.MouseButtons.RIGHT))
            {
                if (movingBody == null)
                {
                    movingBody = GetBodyAt(Input.Instance.SceneMousePosition);

                    if (movingBody == null)
                    {
                        GameObject parent = GameObject.Get();

                        while (parent != null && GameObject.Get(parent) != null)
                            parent = GameObject.Get(parent);

                        GameObject newObject = GameObject.Create(parent);
                        newObject.Position = Input.Instance.SceneMousePosition;
                        movingBody = newObject.AddComponent<BodyComponent>();

                        if (Input.Instance.IsMouseButtonDown(Input.MouseButtons.LEFT))
                        {
                            Fixture fixture = newObject.AddComponent<FixtureComponent>().Fixture = FixtureFactory.AttachRectangle(
                                0.5f, 0.5f, 1.0f, Vector2.Zero, movingBody.Body);
                            newObject.AddComponent<TextureRenderer>().TextureID = "Pin";
                            newObject.Scale = new Vector2(0.2f);
                        }
                        else
                        {
                            Fixture fixture = newObject.AddComponent<FixtureComponent>().Fixture = FixtureFactory.AttachCircle(
                                0.5f, 1.0f, movingBody.Body);
                            newObject.AddComponent<TextureRenderer>().TextureID = "Earth";
                            newObject.Scale = new Vector2(0.04f);
                        }

                        if (newObject.Parent != null)
                        {
                            newObject.AddComponent<JointComponent>().Joint = JointFactory.CreateRevoluteJoint(PhysicsWorld, movingBody.Body,
                                parent.GetComponent<BodyComponent>().Body, Vector2.Zero);
                        }
                    }

                    movingBody.Body.BodyType = BodyType.Dynamic;
                    movingBody.Body.Mass = 1f;
                    movingBody.Body.LinearDamping = movingBody.Body.AngularDamping = 25f;
                }

                movingBody.Body.ApplyForce((Input.Instance.SceneMousePosition - movingBody.DisplayPosition) * 25f);
            }
            else
            {
                if (movingBody != null)
                {
                    movingBody.Body.LinearDamping = movingBody.Body.AngularDamping = 0f;

                    if (movingBody.Parent.GetComponent<FixtureComponent>().Fixture.Shape.ShapeType != FarseerPhysics.Collision.Shapes.ShapeType.Circle)
                        movingBody.Body.BodyType = BodyType.Static;

                    movingBody = null;
                }

                if (Input.Instance.IsMouseButtonDown(Input.MouseButtons.MIDDLE))
                {
                    BodyComponent body = GetBodyAt(Input.Instance.SceneMousePosition);

                    if (body != null)
                        body.Parent.Destroy();
                }
            }

            Camera.Scale *= 1 + Input.Instance.WheelSpeed * 0.00025f;
        }

        protected override void OnPreDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            App.Instance.GraphicsDevice.Clear(Color.Black);
        }

        protected override void OnPostDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
        }
    }
}
