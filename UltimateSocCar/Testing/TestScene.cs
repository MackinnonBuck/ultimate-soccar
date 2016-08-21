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

namespace UltimateSocCar.Testing
{
    public class TestScene : Scene
    {
        PhysicsBody movingBody;

        protected override void OnInitialize()
        {
            DebugDrawEnabled = true;
        }

        protected override void OnQuit()
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
                        GameObject newObject = new GameObject();
                        newObject.Position = Mouse.GetState().Position.ToVector2();
                        movingBody = newObject.AddComponent<PhysicsBody>();

                        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            RectangleShape rectangleShape = newObject.AddComponent<RectangleShape>();
                            rectangleShape.Width = rectangleShape.Height = 0.25f;
                        }
                        else
                        {
                            newObject.AddComponent<CircleShape>().Radius = 0.5f;
                        }
                    }

                    movingBody.BodyType = BodyType.Kinematic;
                }

                movingBody.Parent.Position = Mouse.GetState().Position.ToVector2();
                movingBody.LinearVelocity = Vector2.Zero;
            }
            else
            {
                if (movingBody != null)
                {
                    if (movingBody.Parent.GetComponent<CircleShape>() != null)
                        movingBody.BodyType = BodyType.Dynamic;

                    movingBody = null;
                }

                if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
                {
                    PhysicsBody body = App.Instance.Scene.GetBodyAt(Mouse.GetState().Position.ToVector2());

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
