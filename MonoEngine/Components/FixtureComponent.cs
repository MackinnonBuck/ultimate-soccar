using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using MonoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Components
{
    public class FixtureComponent : Component
    {
        private Fixture _fixture;

        /// <summary>
        /// The Fixture associated with this PhysicsFixture.
        /// </summary>
        public Fixture Fixture
        {
            get
            {
                return _fixture;
            }
            set
            {
                _fixture?.Dispose();

                if (value.Body != ParentBody.Body)
                {
                    Debug.Log("The Fixture's associated body must be its parent GameObject's Body.", Debug.LogSeverity.ERROR);
                    return;
                }

                _fixture = value;
                _fixture.UserData = this;
            }
        }

        /// <summary>
        /// The parent PhysicsBody of this PhysicsFixture.
        /// </summary>
        public BodyComponent ParentBody
        {
            get
            {
                return Parent.GetComponent<BodyComponent>();
            }
        }

        /// <summary>
        /// Initializes the PhysicsFixture.
        /// </summary>
        protected override void OnInitialize()
        {
            if (ParentBody == null)
            {
                Debug.Log("Cannot add a PhysicsFixture to a GameObject without first adding a PhysicsBody.", Debug.LogSeverity.ERROR);
                Destroy();
                return;
            }
        }

        /// <summary>
        /// Destroys the PhysicsFixture and its child Fixture.
        /// </summary>
        protected override void OnDestroy()
        {
            _fixture?.Dispose();
        }
    }
}
