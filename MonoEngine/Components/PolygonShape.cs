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
using FarseerPhysics;
using MonoEngine.Utilities;

namespace MonoEngine.Components
{
    public class PolygonShape : PhysicsShape<FarseerPhysics.Collision.Shapes.PolygonShape>
    {
        private Vertices _vertices;

        /// <summary>
        /// The vertices of the PolygonShape.
        /// </summary>
        public Vertices Vertices
        {
            get
            {
                return _vertices;
            }
            set
            {
                _vertices = value;

                if (Fixture == null)
                {
                    Fixture = FixtureFactory.AttachPolygon(_vertices, _density, ParentBody);
                    return;
                }

                Shape.Vertices = _vertices;
                ParentBody.ResetMassData();
            }
        }

        private float _density;

        /// <summary>
        /// The density of the PolygonShape.
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
        /// Initializes the PolygonShape.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            _density = 1.0f;
        }
    }
}
