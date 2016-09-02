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
        PhysicsBody movingBody;

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
                    App.Instance.ChangeScene(new TestScene());
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
                        GameObject parent = GetChild<GameObject>();

                        while (parent != null && parent.GetChild<GameObject>() != null)
                            parent = parent.GetChild<GameObject>();

                        GameObject newObject = new GameObject(parent);
                        newObject.Position = Input.Instance.SceneMousePosition;
                        movingBody = newObject.AddComponent<PhysicsBody>();

                        if (Input.Instance.IsMouseButtonDown(Input.MouseButtons.LEFT))
                        {
                            RectangleShape rectangleShape = newObject.AddComponent<RectangleShape>();
                            rectangleShape.Width = rectangleShape.Height = 0.5f;
                            newObject.AddComponent<TextureRenderer>().TextureID = "Pin";
                            newObject.Scale = new Vector2(0.2f);
                        }
                        else
                        {
                            CircleShape circleShape = newObject.AddComponent<CircleShape>();
                            circleShape.Radius = 0.5f;
                            newObject.AddComponent<TextureRenderer>().TextureID = "Earth";
                            newObject.Scale = new Vector2(0.04f);
                        }

                        if (newObject.Parent != null)
                        {
                            newObject.AddComponent<RevoluteJoint>();
                        }
                    }

                    movingBody.BodyType = BodyType.Dynamic;
                    movingBody.Mass = 1f;
                    movingBody.LinearDamping = movingBody.AngularDamping = 25f;
                }

                movingBody.ApplyForce((Input.Instance.SceneMousePosition - movingBody.Position) * 25f);
            }
            else
            {
                if (movingBody != null)
                {
                    movingBody.LinearDamping = movingBody.AngularDamping = 0f;

                    if (movingBody.Parent.GetComponent<RectangleShape>() != null)
                        movingBody.BodyType = BodyType.Static;

                    movingBody = null;
                }

                if (Input.Instance.IsMouseButtonDown(Input.MouseButtons.MIDDLE))
                {
                    PhysicsBody body = GetBodyAt(Input.Instance.SceneMousePosition);

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
