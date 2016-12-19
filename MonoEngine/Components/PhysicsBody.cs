using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Factories;
using FarseerPhysics;
using MonoEngine.Core;

namespace MonoEngine.Components
{
    public class PhysicsBody : Component
    {
        /// <summary>
        /// The Body associated with the PhysicsBody.
        /// </summary>
        internal Body Body { get; private set; }

        /// <summary>
        /// Describes if the PhysicsBody is dynamic, kinematic, or static.
        /// </summary>
        public BodyType BodyType
        {
            get
            {
                return Body.BodyType;
            }
            set
            {
                Body.BodyType = value;
            }
        }

        /// <summary>
        /// The mass of the PhysicsBody.
        /// </summary>
        public float Mass
        {
            get
            {
                return Body.Mass;
            }
            set
            {
                Body.Mass = value;
            }
        }

        /// <summary>
        /// The position of the PhysicsBody.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return ConvertUnits.ToDisplayUnits(Body.Position);
            }
            set
            {
                Body.Position = ConvertUnits.ToSimUnits(value);
                Body.Awake = true;
            }
        }

        /// <summary>
        /// The rotation of the PhysicsBody.
        /// </summary>
        public float Rotation
        {
            get
            {
                return Body.Rotation;
            }
            set
            {
                Body.Rotation = value;
                Body.Awake = true;
            }
        }

        /// <summary>
        /// The linear velocity of the PhysicsBody.
        /// </summary>
        public Vector2 LinearVelocity
        {
            get
            {
                return Body.LinearVelocity;
            }
            set
            {
                Body.LinearVelocity = value;
            }
        }

        /// <summary>
        /// The angular velocity of the PhysicsBody.
        /// </summary>
        public float AngularVelocity
        {
            get
            {
                return Body.AngularVelocity;
            }
            set
            {
                Body.AngularVelocity = value;
            }
        }

        /// <summary>
        /// The linear damping of the PhysicsBody.
        /// </summary>
        public float LinearDamping
        {
            get
            {
                return Body.LinearDamping;
            }
            set
            {
                Body.LinearDamping = value;
            }
        }

        /// <summary>
        /// The angular damping of the PhysicsBody.
        /// </summary>
        public float AngularDamping
        {
            get
            {
                return Body.AngularDamping;
            }
            set
            {
                Body.AngularDamping = value;
            }
        }

        /// <summary>
        /// Applies a force to the PhysicsBody.
        /// </summary>
        /// <param name="force"></param>
        /// <param name="point"></param>
        public void ApplyForce(Vector2 force)
        {
            Body.ApplyForce(force, Body.Position);
        }

        /// <summary>
        /// Applies a force to the PhysicsBody at a specified point.
        /// </summary>
        /// <param name="force"></param>
        /// <param name="point"></param>
        public void ApplyForce(Vector2 force, Vector2 point)
        {
            Body.ApplyForce(force, ConvertUnits.ToSimUnits(point));
        }

        /// <summary>
        /// Applies a torque to the PhysicsBody.
        /// </summary>
        /// <param name="torque"></param>
        public void ApplyTorque(float torque)
        {
            Body.ApplyTorque(torque);
        }

        /// <summary>
        /// Appleis a linear impulse to the PhysicsBody.
        /// </summary>
        /// <param name="impulse"></param>
        /// <param name="point"></param>
        public void ApplyLinearImpulse(Vector2 impulse)
        {
            Body.ApplyLinearImpulse(impulse, Body.Position);
        }

        /// <summary>
        /// Appleis a linear impulse to the PhysicsBody at a specified point.
        /// </summary>
        /// <param name="impulse"></param>
        /// <param name="point"></param>
        public void ApplyLinearImpulse(Vector2 impulse, Vector2 point)
        {
            Body.ApplyLinearImpulse(impulse, ConvertUnits.ToSimUnits(point));
        }

        /// <summary>
        /// Applies an angular impulse to the PhysicsBody.
        /// </summary>
        /// <param name="impulse"></param>
        public void ApplyAngularImpulse(float impulse)
        {
            Body.ApplyAngularImpulse(impulse);
        }

        protected override void OnInitialize()
        {
            if (Parent.GetComponents<PhysicsBody>().Count > 1)
            {
                Debug.Log("Cannot add more than one PhysicsBody to a GameObject.", Debug.LogSeverity.ERROR);
                Destroy();
                return;
            }
            
            Body = BodyFactory.CreateBody(App.Instance.ActiveScene.PhysicsWorld, ConvertUnits.ToSimUnits(Parent.Position.X, Parent.Position.Y), Parent.Rotation);
            Body.UserData = this;
            Body.BodyType = BodyType.Dynamic;

            Parent.PropertyBinder["Position"].SetBinding(this, "Position");
            Parent.PropertyBinder["Rotation"].SetBinding(this, "Rotation");
        }

        protected override void OnUpdate(GameTime gameTime)
        {
        }

        protected override void OnDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
        }

        protected override void OnDestroy()
        {
            if (Body != null)
                App.Instance.ActiveScene.PhysicsWorld.RemoveBody(Body);
        }
    }
}
