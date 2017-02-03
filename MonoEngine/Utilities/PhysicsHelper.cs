using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Utilities
{
    public class PhysicsHelper
    {
        /// <summary>
        /// Returns the center of mass of multiple bodies.
        /// </summary>
        /// <param name="bodies"></param>
        /// <returns></returns>
        public static Vector2 CenterOfMass(params Body[] bodies)
        {
            Vector2 center = new Vector2();
            float totalMass = 0f;

            foreach (Body body in bodies)
            {
                center += body.WorldCenter * body.Mass;
                totalMass += body.Mass;
            }

            center /= totalMass;

            return center;
        }
    }
}
