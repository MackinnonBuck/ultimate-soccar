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
        private float _width;

        /// <summary>
        /// The width of the RectangleShape.
        /// </summary>
        public float Width
        {
            get
            {
                return _width;
            }
            set
            {
                if (value == 0)
                {
                    Debug.Log("Cannot set width of a RectangleShape to 0.", Debug.LogSeverity.WARNING);
                    return;
                }

                _width = value;
                UpdateVertices();
            }
        }

        private float _height;

        /// <summary>
        /// The height of the RectangleShape.
        /// </summary>
        public float Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (value == 0)
                {
                    Debug.Log("Cannot set height of a RectangleShape to 0.", Debug.LogSeverity.WARNING);
                    return;
                }

                _height = value;
                UpdateVertices();
            }
        }

        private float _density;

        /// <summary>
        /// The density of the RectangleShape.
        /// </summary>
        public float Density
        {
            get
            {
                return _density;
            }
            set
            {
                _density = value;
                Shape.Density = _density;
            }
        }

        private Vector2 _offset;

        /// <summary>
        /// The center offset of the RectangleShape.
        /// </summary>
        public Vector2 Offset
        {
            get
            {
                return _offset;
            }
            set
            {
                _offset = value;
                UpdateVertices();
            }
        }

        private float _angle;

        /// <summary>
        /// The angle of the RectangleShape.
        /// </summary>
        public float Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                _angle = value;
                UpdateVertices();
            }
        }

        /// <summary>
        /// Returns a copy of the RectangleShape's vertices.
        /// </summary>
        /// <returns></returns>
        public Vertices GetVertices()
        {
            return new Vertices(Shape.Vertices);
        }

        /// <summary>
        /// Creates a RectangleShape with default values.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            _width = 1f;
            _height = 1f;
            _density = 1f;
            _offset = Vector2.Zero;
            _angle = 0f;
            
            Fixture = FixtureFactory.AttachRectangle(_width, _height, _density, _offset, ParentBody);
        }

        /// <summary>
        /// Resets the polygon's vertices and recalculates the body's mass data.
        /// </summary>
        private void UpdateVertices()
        {
            Shape.Vertices = PolygonTools.CreateRectangle(_width * 0.5f, _height * 0.5f, _offset, _angle);           
            ParentBody.ResetMassData();
        }
    }
}
