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

            if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
            {
                PhysicsBody body = App.Instance.Scene.GetBodyAt(Mouse.GetState().Position.ToVector2());

                if (body != null)
                    body.Parent.Destroy();
            }
            else
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    GameObject sphereObject = new GameObject();
                    sphereObject.Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                    sphereObject.AddComponent<PhysicsBody>();
                    sphereObject.AddComponent<CircleShape>().Radius = 0.5f;
                }
                else if (Mouse.GetState().RightButton == ButtonState.Pressed &&
                    App.Instance.Scene.GetBodiesAt(Mouse.GetState().Position.ToVector2()).Count == 0)
                {
                    GameObject boxObject = new GameObject();
                    boxObject.Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                    PhysicsBody body = boxObject.AddComponent<PhysicsBody>();
                    body.BodyType = BodyType.Static;
                    RectangleShape shape = boxObject.AddComponent<RectangleShape>();
                    shape.Width = 0.25f;
                    shape.Height = 0.25f;
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
