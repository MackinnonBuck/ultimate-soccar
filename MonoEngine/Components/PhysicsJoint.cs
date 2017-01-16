using MonoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;

namespace MonoEngine.Components
{
    public abstract class PhysicsJoint<T> : Component where T : Joint
    {
        Joint joint;

        /// <summary>
        /// The joint instance associated with the PhysicsJoint.
        /// </summary>
        protected T Joint
        {
            get
            {
                return joint == null ? null : (T)joint;
            }
        }

        /// <summary>
        /// Used to specify how the joint is created.
        /// </summary>
        /// <returns></returns>
        protected abstract Joint CreateJoint();

        protected override void OnInitialize()
        {
            PhysicsBody parentBody = Parent.GetComponent<PhysicsBody>();

            if (parentBody == null || parentBody.Body.FixtureList.Count == 0)
            {
                Debug.Log("Cannot add a PhysicsJoint to a GameObject without first adding a PhysicsBody and PhysicsShape.", Debug.LogSeverity.ERROR);
                Destroy();
                return;
            }

            if (Parent.Parent == null)
            {
                Debug.Log("Joints can only be attached to GameObjects with parents.", Debug.LogSeverity.ERROR);
                Destroy();
                return;
            }

            PhysicsBody grandparentBody = Parent.Parent.GetComponent<PhysicsBody>();

            if (grandparentBody == null || grandparentBody.Body.FixtureList.Count == 0)
            {
                Debug.Log("The Joint's associated GameObject's parent must have PhysicsBody and PhysicsShape Components.", Debug.LogSeverity.ERROR);
                Destroy();
                return;
            }
            
            joint = CreateJoint();
        }

        /// <summary>
        /// Removes the PhysicsJoint from the scene.
        /// </summary>
        protected override void OnDestroy()
        {
            if (joint != null)
                App.Instance.ActiveScene.PhysicsWorld.RemoveJoint(joint);
        }
    }
}
