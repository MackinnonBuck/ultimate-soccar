﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;

namespace MonoEngine.Components
{
    public class CircleShape : PhysicsShape<FarseerPhysics.Collision.Shapes.CircleShape>
    {
        float radius;
        float density;

        /// <summary>
        /// The radius of the CircleShape.
        /// </summary>
        public float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
                Shape.Radius = value;
            }
        }

        /// <summary>
        /// The density of the CircleShape.
        /// </summary>
        public float Density
        {
            get
            {
                return density;
            }
            set
            {
                density = value;
                Shape.Density = value;
            }
        }

        protected override void OnInitialize()
        {
            radius = 1f;
            density = 1f;

            base.OnInitialize();
        }

        protected override Fixture CreateFixture()
        {
            return FixtureFactory.AttachCircle(Radius, Density, parentBody);
        }

        protected override void OnDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
        }

        protected override void OnUpdate(GameTime gameTime)
        {
        }
    }
}