using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Components
{
    public class EllipseShape : PhysicsShape<FarseerPhysics.Collision.Shapes.PolygonShape>
    {
        private float _xRadius;

        /// <summary>
        /// The horizontal radius of the ellipse.
        /// </summary>
        public float XRadius
        {
            get
            {
                return _xRadius;
            }
            set
            {
                _xRadius = value;
                UpdateVertices();
            }
        }

        private float _yRadius;

        /// <summary>
        /// The vertical radius of the ellipse.
        /// </summary>
        public float YRadius
        {
            get
            {
                return _yRadius;
            }
            set
            {
                _yRadius = value;
                UpdateVertices();
            }
        }

        private int _numEdges;

        /// <summary>
        /// The number of edges in the ellipse polygon.
        /// </summary>
        public int NumEdges
        {
            get
            {
                return _numEdges;
            }
            set
            {
                if (value > Settings.MaxPolygonVertices)
                    return;
                
                _numEdges = value;
                UpdateVertices();
            }
        }

        private float _density;

        /// <summary>
        /// The density of the EllipseShape.
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

        /// <summary>
        /// Translates the ellipse by the given translation.
        /// </summary>
        /// <param name="translation"></param>
        public void Translate(Vector2 translation)
        {
            Vertices verts = new Vertices(Shape.Vertices);
            verts.Translate(translation);

            Shape.Vertices = verts;
            ParentBody.ResetMassData();
        }

        /// <summary>
        /// Rotates the ellipse by the given angle.
        /// </summary>
        /// <param name="angle"></param>
        public void Rotate(float angle)
        {
            Vertices verts = new Vertices(Shape.Vertices);
            verts.Rotate(angle);

            Shape.Vertices = verts;
            ParentBody.ResetMassData();
        }

        /// <summary>
        /// Returns the vertices that make up the ellipse.
        /// </summary>
        /// <returns></returns>
        public Vertices GetVertices()
        {
            return new Vertices(Shape.Vertices);
        }

        /// <summary>
        /// Initializes the EllipseShape with default values.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            _xRadius = 1.0f;
            _yRadius = 1.0f;
            _numEdges = 16;
            _density = 1.0f;
            
            Fixture = FixtureFactory.AttachEllipse(_xRadius, _yRadius, _numEdges, _density, ParentBody);
        }

        /// <summary>
        /// Updates the vertices of the ellipse.
        /// </summary>
        private void UpdateVertices()
        {
            Shape.Vertices = PolygonTools.CreateEllipse(_xRadius, _yRadius, _numEdges);
            ParentBody.ResetMassData();
        }
    }
}
