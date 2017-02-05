using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using MonoEngine.Components;
using MonoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Controllers;

namespace UltimateSocCar.Components
{
    public class Wheel : Component
    {
        // Constants
        const float Density = 0.25f;
        const float Friction = 1000.0f;
        const float SuspensionFrequency = 15.0f;
        const float SuspentionDampingRatio = 2.0f;

        private List<KeyValuePair<Fixture, Contact>> contacts;
        private Body parentBody;

        public float Radius { get; private set; }
        public Body Body { get; private set; }
        public Fixture Fixture { get; private set; }
        public WheelJoint WheelJoint { get; private set; }

        /// <summary>
        /// Returns true if contact with the ground is being made.
        /// </summary>
        public bool Grounded
        {
            get
            {
                return contacts.Count > 0;
            }
        }

        /// <summary>
        /// Gets the ground contact normal or the relative down vector if no contact is present.
        /// </summary>
        public Vector2 GroundNormal { get; private set; }

        /// <summary>
        /// Returns true if the wheel can stick to the current surface.
        /// </summary>
        public bool Sticky { get; private set; }

        /// <summary>
        /// Creates a wheel from the given radius and position.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="position"></param>
        public void Create(float radius, Vector2 position)
        {
            Radius = radius;

            contacts = new List<KeyValuePair<Fixture, Contact>>();
            parentBody = Parent.Parent.GetComponent<BodyComponent>().Body;

            Body = Parent.AddComponent<BodyComponent>().Body;
            Body.BodyType = BodyType.Dynamic;

            Fixture = Parent.AddComponent<FixtureComponent>().Fixture = FixtureFactory.AttachCircle(Radius, Density, Body, this);
            Fixture.OnCollision += OnCollision;
            Fixture.OnSeparation += OnSeparation;

            Body.Friction = Friction;

            WheelJoint = (WheelJoint)(Parent.AddComponent<JointComponent>().Joint =
                JointFactory.CreateWheelJoint(App.Instance.Scene.PhysicsWorld, parentBody, Body, Vector2.UnitY));
            WheelJoint.LocalAnchorA = position;
            WheelJoint.LocalAnchorB = Vector2.Zero;
            WheelJoint.MotorEnabled = true;
            WheelJoint.MaxMotorTorque = 0.0f;
            WheelJoint.Frequency = SuspensionFrequency;
            WheelJoint.DampingRatio = SuspentionDampingRatio;

            Parent.AddComponent<TextureRenderer>().TextureID = "Wheel";
        }

        /// <summary>
        /// Updates values of the Wheel.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void OnUpdate(GameTime gameTime)
        {
            // Calculate ground normal
            if (contacts.Count == 0)
            {
                GroundNormal = Vector2.Transform(-Vector2.UnitY, Matrix.CreateRotationZ(parentBody.Rotation));
                Sticky = false;
            }
            else
            {
                Vector2 normalAvg = new Vector2();

                foreach (Vector2 normal in (from x in contacts where x.Value.IsTouching == true select x.Value.Manifold.LocalNormal))
                    normalAvg += normal;

                normalAvg /= contacts.Count;

                GroundNormal = normalAvg;

                Sticky = GroundNormal.Y <= 0 || GroundNormal.Y <= Math.Abs(GroundNormal.X);
            }
        }

        /// <summary>
        /// Registers a ground collision.
        /// </summary>
        /// <param name="fixtureA"></param>
        /// <param name="fixtureB"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        private bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (!contact.IsTouching)
                return true;

            FixtureComponent ground = fixtureB.UserData as FixtureComponent;

            if (ground == null)
                return true;

            if (ground.Parent.Name.Equals("Ground"))
                contacts.Add(new KeyValuePair<Fixture, Contact>(fixtureB, contact));

            return true;
        }

        /// <summary>
        /// Unregisters a ground collision.
        /// </summary>
        /// <param name="fixtureA"></param>
        /// <param name="fixtureB"></param>
        private void OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            for (int i = 0; i < contacts.Count; i++)
            {
                if (contacts[i].Key == fixtureB)
                    contacts.RemoveAt(i);
            }
        }
    }
}
