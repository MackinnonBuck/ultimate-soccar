using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using MonoEngine.Core;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;

namespace MonoEngine.Components
{
    public abstract class PhysicsShape<T> : Component where T : Shape
    {
        protected Body parentBody;
        protected Fixture fixture;

        /// <summary>
        /// The shape instance associated with the Fixture.
        /// </summary>
        protected T Shape
        {
            get
            {
                return fixture == null ? null : (T)fixture.Shape;
            }
        }

        /// <summary>
        /// Called when the Fixture is created.
        /// </summary>
        /// <returns></returns>
        protected abstract Fixture CreateFixture();

        protected override void OnInitialize()
        {
            List<PhysicsBody> physicsBodies = Parent.FindComponents<PhysicsBody>();

            if (physicsBodies.Count == 1)
            {
                parentBody = physicsBodies[0].Body;
                fixture = CreateFixture();
            }
            else
            {
                if (physicsBodies.Count == 0)
                    Debug.Log("Cannot add a PhysicsShape to a GameObject without first adding a PhysicsBody.", Debug.LogSeverity.ERROR);
                else
                    Debug.Log("Cannot add a PhysicsShape to a GameObject with multiple PhysicsBodies.", Debug.LogSeverity.ERROR);

                Destroy();
            }
        }

        protected override void OnDestroy()
        {
            parentBody?.DestroyFixture(fixture);
        }
    }
}
