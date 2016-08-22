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
            if (Parent.PhysicsBody == null)
                Debug.Log("Cannot add a PhysicsShape to a GameObject without first adding a PhysicsBody.", Debug.LogSeverity.ERROR);
            else
                fixture = CreateFixture();
        }

        protected override void OnDestroy()
        {
            if (Parent.PhysicsBody != null)
                Parent.PhysicsBody.Body.DestroyFixture(fixture);
        }
    }
}
