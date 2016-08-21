using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Core;

namespace UltimateSocCar.Testing
{
    public class TestObject : GameObject
    {
        const int WIDTH = 32;
        const int HEIGHT = 32;

        Vector2 pos;
        Texture2D rect;

        public TestObject(int x, int y)
        {
            pos = new Vector2(x - WIDTH / 2, y - HEIGHT / 2);
            rect = new Texture2D(App.Instance.GraphicsDevice, WIDTH, HEIGHT);

            Color[] data = new Color[WIDTH * HEIGHT];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Yellow;
            rect.SetData(data);
        }

        protected override void OnInitialize()
        {
        }

        protected override void OnUpdate(GameTime gameTime)
        {
        }

        protected override void OnDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(rect, pos, Color.White);
        }

        protected override void OnDestroy()
        {
        }
    }
}
