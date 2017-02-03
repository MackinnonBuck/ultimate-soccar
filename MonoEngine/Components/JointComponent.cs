using FarseerPhysics.Dynamics.Joints;
using MonoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Components
{
    public class JointComponent : Component
    {
        private Joint _joint;

        /// <summary>
        /// The Joint associated with this PhysicsJoint.
        /// </summary>
        public Joint Joint
        {
            get
            {
                return _joint;
            }
            set
            {
                if (_joint != null)
                    App.Instance.Scene.PhysicsWorld.RemoveJoint(_joint);

                _joint = value;
            }
        }

        /// <summary>
        /// Destroys the PhysicsJoint and its child Joint.
        /// </summary>
        protected override void OnDestroy()
        {
            if (_joint != null)
                App.Instance.Scene.PhysicsWorld.RemoveJoint(_joint);
        }
    }
}
