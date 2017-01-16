using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
using MonoEngine.Core;

namespace MonoEngine.Components
{
    public class RectangleShape : PhysicsShape<FarseerPhysics.Collision.Shapes.PolygonShape>
    {
        float width;
        float height;
        float density;

        /// <summary>
        /// The width of the RectangleShape.
        /// </summary>
        public float Width
        {
            get
            {
                return width;
            }
            set
            {
                if (value == 0)
                {
                    Debug.Log("Cannot set width of a RectangleShape to 0.", Debug.LogSeverity.WARNING);
                    return;
                }

                width = value;
                UpdateVertices();
            }
        }

        /// <summary>
        /// The height of the RectangleShape.
        /// </summary>
        public float Height
        {
            get
            {
                return height;
            }
            set
            {
                if (value == 0)
                {
                    Debug.Log("Cannot set height of a RectangleShape to 0.", Debug.LogSeverity.WARNING);
                    return;
                }

                height = value;
                UpdateVertices();
            }
        }

        /// <summary>
        /// The density of the RectangleShape.
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
                UpdateVertices();
            }
        }

        /// <summary>
        /// Creates a RectangleShape with default values.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            width = 1f;
            height = 1f;
            density = 1f;
            
            Fixture = FixtureFactory.AttachRectangle(width, height, density, Vector2.Zero, Parent.GetComponent<PhysicsBody>().Body);
        }

        protected override void OnDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
        }

        protected override void OnUpdate(GameTime gameTime)
        {
        }

        private void UpdateVertices()
        {
            Shape.Vertices = PolygonTools.CreateRectangle(width * 0.5f, height * 0.5f);
        }
    }
}
