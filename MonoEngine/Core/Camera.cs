using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;

namespace MonoEngine.Core
{
    public class Camera : Entity
    {
        /// <summary>
        /// The Camera's Viewport.
        /// </summary>
        public Viewport Viewport
        {
            get
            {
                return App.Instance.GraphicsDevice.Viewport;
            }
        }

        public Vector2 Origin { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; }

        /// <summary>
        /// Gets the Camera's view matrix.
        /// </summary>
        public Matrix ViewMatrix
        {
            get
            {
                return
                    Matrix.CreateTranslation(-Position.X, -Position.Y, 0f) *
                    Matrix.CreateTranslation(-Origin.X, -Origin.Y, 0f) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(Scale.X, Scale.Y, 1f) *
                    Matrix.CreateTranslation(Origin.X, Origin.Y, 0f);
            }
        }

        /// <summary>
        /// Gets the Camera's view matrix in sim units.
        /// </summary>
        public Matrix SimViewMatrix
        {
            get
            {
                return
                    Matrix.CreateTranslation(ConvertUnits.ToSimUnits(-Position.X), ConvertUnits.ToSimUnits(-Position.Y), 0f) *
                    Matrix.CreateTranslation(ConvertUnits.ToSimUnits(-Origin.X), ConvertUnits.ToSimUnits(-Origin.Y), 0f) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(Scale.X, Scale.Y, 1f) *
                    Matrix.CreateTranslation(ConvertUnits.ToSimUnits(Origin.X), ConvertUnits.ToSimUnits(Origin.Y), 0f);
            }
        }

        private Camera()
        {
        }

        /// <summary>
        /// Creates a new Camera.
        /// </summary>
        /// <returns></returns>
        public static Camera Create()
        {
            return App.Instance.Scene.Children.Add(new Camera());
        }

        public override void Initialize()
        {
            Origin = new Vector2(Viewport.Width * 0.5f, Viewport.Height * 0.5f);
            Position = Vector2.Zero;
            Rotation = 0f;
            Scale = Vector2.One;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
        }

        protected override void OnDestroy()
        {
        }
    }
}
