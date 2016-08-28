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
        }

        protected override void OnDestroy()
        {
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.R))
                App.Instance.ChangeScene(new TestScene());

            if (Keyboard.GetState().IsKeyDown(Keys.E))
                DebugDrawEnabled = true;
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
                DebugDrawEnabled = false;

            if (Mouse.GetState().LeftButton == ButtonState.Pressed || Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                if (movingBody == null)
                {
                    movingBody = GetBodyAt(Mouse.GetState().Position.ToVector2());

                    if (movingBody == null)
                    {
                        GameObject parent = GetChild<GameObject>();

                        while (parent != null && parent.GetChild<GameObject>() != null)
                            parent = parent.GetChild<GameObject>();

                        GameObject newObject = new GameObject(parent);
                        newObject.Position = Mouse.GetState().Position.ToVector2();
                        movingBody = newObject.AddComponent<PhysicsBody>();

                        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
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

                movingBody.ApplyForce((Mouse.GetState().Position.ToVector2() - movingBody.Position) * 25f);
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

                if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
                {
                    PhysicsBody body = GetBodyAt(Mouse.GetState().Position.ToVector2());

                    if (body != null)
                        body.Parent.Destroy();
                }
            }
        }

        protected override void OnPreDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            App.Instance.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
        }

        protected override void OnPostDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.End();
        }
    }
}
