using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using MonoEngine.Core;

namespace MonoEngine.Components
{
    public class RevoluteJoint : PhysicsJoint<FarseerPhysics.Dynamics.Joints.RevoluteJoint>
    {
        /// <summary>
        /// Specifies the local A anchor of the RevoluteJoint.
        /// </summary>
        public Vector2 LocalAnchorA
        {
            set
            {
                Joint.LocalAnchorA = value;
            }
            get
            {
                return Joint.LocalAnchorA;
            }
        }

        /// <summary>
        /// Specifies the local B anchor of the RevoluteJoint.
        /// </summary>
        public Vector2 LocalAnchorB
        {
            set
            {
                Joint.LocalAnchorB = value;
            }
            get
            {
                return Joint.LocalAnchorB;
            }
        }

        /// <summary>
        /// Specifies the world A anchor of the RevoluteJoint.
        /// </summary>
        public Vector2 WorldAnchorA
        {
            set
            {
                Joint.WorldAnchorA = value;
            }
            get
            {
                return Joint.LocalAnchorA;
            }
        }

        /// <summary>
        /// Specifies the world B anchor of the RevoluteJoint.
        /// </summary>
        public Vector2 WorldAnchorB
        {
            set
            {
                Joint.WorldAnchorB = value;
            }
            get
            {
                return Joint.WorldAnchorB;
            }
        }

        protected override Joint CreateJoint()
        {
            PhysicsBody parentBody = Parent.GetComponent<PhysicsBody>();

            return JointFactory.CreateRevoluteJoint(App.Instance.ActiveScene.PhysicsWorld, Parent.GetComponent<PhysicsBody>().Body,
                Parent.Parent.GetComponent<PhysicsBody>().Body, Vector2.Zero);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
        }

        protected override void OnDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
        }
    }
}
