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
        /// <summary>
        /// The parent Body of the PhysicsShape.
        /// </summary>
        protected Body ParentBody { get; private set; }

        /// <summary>
        /// The fixture associated with the PhysicsShape.
        /// </summary>
        protected Fixture Fixture { get; set; }

        /// <summary>
        /// The shape instance associated with the Fixture.
        /// </summary>
        protected T Shape
        {
            get
            {
                return Fixture == null ? null : (T)Fixture.Shape;
            }
        }

        /// <summary>
        /// Initializes the PhysicsShape.
        /// </summary>
        protected override void OnInitialize()
        {
            PhysicsBody physicsBody = Parent.GetComponent<PhysicsBody>();

            if (physicsBody == null)
            {
                Debug.Log("Cannot add a PhysicsShape to a GameObject without first adding a PhysicsBody.", Debug.LogSeverity.ERROR);
                Destroy();
                return;
            }

            ParentBody = physicsBody.Body;
        }

        /// <summary>
        /// Removes the PhysicsShape from the parent GameObject.
        /// </summary>
        protected override void OnDestroy()
        {
            PhysicsBody parentBody = Parent.GetComponent<PhysicsBody>();

            if (parentBody != null && Fixture != null)
                parentBody.Body.DestroyFixture(Fixture);
        }
    }
}
