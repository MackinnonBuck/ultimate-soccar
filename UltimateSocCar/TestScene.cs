using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UltimateSocCar.Engine;
using Microsoft.Xna.Framework.Input;

namespace UltimateSocCar
{
    class TestScene : Scene
    {
        protected override void OnInitialize()
        {
            new GameObject().AddComponent<TestComponent>();
        }

        protected override void OnQuit()
        {
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
                App.Instance.ChangeScene(new TestScene());

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                new TestObject(Mouse.GetState().X, Mouse.GetState().Y);

            if (Mouse.GetState().RightButton == ButtonState.Pressed)
                App.Instance.Scene.FindGameObject<TestObject>()?.Destroy();
        }

        protected override void OnPreDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            App.Instance.GraphicsDevice.Clear(Color.OrangeRed);

            spriteBatch.Begin();
        }

        protected override void OnPostDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.End();
        }
    }
}
