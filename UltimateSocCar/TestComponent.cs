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
    public class TestComponent : Component
    {
        const int WIDTH = 32;
        const int HEIGHT = 32;

        Texture2D rect;

        protected override void OnInitialize()
        {
            rect = new Texture2D(App.Instance.GraphicsDevice, WIDTH, HEIGHT);

            Color[] data = new Color[WIDTH * HEIGHT];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Yellow;
            rect.SetData(data);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            Parent.Position = new Vector2(Mouse.GetState().X - WIDTH / 2, Mouse.GetState().Y - HEIGHT / 2);
        }

        protected override void OnDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(rect, Parent.Position, Color.White);
        }

        protected override void OnDestroy()
        {           
        }
    }
}
