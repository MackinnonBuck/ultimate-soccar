using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateSocCar.Components
{
    public class StatusIndicator : Component
    {
        Texture2D rect;

        int _width;
        int _height;

        /// <summary>
        /// The width of the indicator.
        /// </summary>
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                ResetRect();
            }
        }

        /// <summary>
        /// The height of the indicator.
        /// </summary>
        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                ResetRect();
            }
        }

        /// <summary>
        /// The vertical positional offset.
        /// </summary>
        public int VerticalOffset { get; set; }

        /// <summary>
        /// The horizontal positional offset.
        /// </summary>
        public int HorizontalOffset { get; set; }

        /// <summary>
        /// Sets or gets if the indicator is visible.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// The color of the indicator.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Initializes the StatusIndicator.
        /// </summary>
        protected override void OnInitialize()
        {
            Visible = true;

            _width = 32;
            _height = 8;

            Color = Color.White;

            ResetRect();
        }

        /// <summary>
        /// Draws the indicator if visible.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        protected override void OnDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Visible)
                spriteBatch.Draw(rect, Parent.Position + new Vector2(HorizontalOffset - Width / 2, VerticalOffset - Height / 2), Color);
        }

        /// <summary>
        /// Resets the size of the indicator.
        /// </summary>
        private void ResetRect()
        {
            rect = new Texture2D(App.Instance.GraphicsDevice, _width, _height);

            Color[] data = new Color[_width * _height];

            for (int i = 0; i < data.Length; i++)
                data[i] = Color.White;

            rect.SetData(data);

            Color = Color.Yellow;
        }
    }
}
