﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;

namespace MonoEngine.Core
{
    public class Camera : IEntity
    {
        bool destroyed;

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

        public Camera()
        {
            destroyed = false;

            Origin = new Vector2(Viewport.Width * 0.5f, Viewport.Height * 0.5f);
            Position = Vector2.Zero;
            Rotation = 0f;
            Scale = Vector2.One;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
        }

        public void Destroy()
        {
            destroyed = true;
        }

        public bool IsDestroyed()
        {
            return destroyed;
        }
    }
}
