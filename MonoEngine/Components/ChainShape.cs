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
using MonoEngine.Utilities;

namespace MonoEngine.Components
{
    public class ChainShape : PhysicsShape<FarseerPhysics.Collision.Shapes.ChainShape>
    {
        private Vertices _vertices;

        /// <summary>
        /// The vertices of the ChainShape.
        /// </summary>
        public Vertices Vertices
        {
            get
            {
                return _vertices;
            }
            set
            {
                if (Fixture != null)
                    return;

                _vertices = value;
                Fixture = FixtureFactory.AttachChainShape(_vertices, ParentBody);
            }
        }
    }
}
