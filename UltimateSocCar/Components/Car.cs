using MonoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoEngine.Components;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;
using MonoEngine.Utilities;

namespace UltimateSocCar.Components
{
    public class Car : Component
    {
        // Constants

        const float TopSpeed = 10f;
        const float SpeedLimit = 14f;

        const float WheelOffsetX = 0.4f;
        const float WheelOffsetY = 0.2f;
        const float WheelRadius = 0.15f;
        const float WheelTopSpeed = TopSpeed / WheelRadius;

        const float WheelStrongForce = 15.0f;
        const float WheelWeakForce = 5.0f;

        const float BoostForceX = 6f;
        const float BoostForceY = 8f;

        const float AccelerationTorque = 0.5f;
        const float AccelerationCurveThreshold = WheelTopSpeed * 0.75f;
        const float BrakingTorque = 0.5f;
        const float CoastingTorque = 0.1f;
        const float LimitingTorque = 2.0f;

        const float BodyWidth = 1.18f;
        const float BodyHeight = 0.36f;

        const float BodyDensity = 1.0f;
        const float BodyFriction = 0.1f;
        const float BodyAngularDamping = 10.0f;
        const float BodyAngularImpulse = 0.05f;

        const float ThrottleTolerance = 0.1f;

        // Physics components

        Body body;

        Wheel frontWheel;
        Wheel rearWheel;

        Gravity gravity;

        StatusIndicator throttleIndicator;
        StatusIndicator limitIndicator;

        /// <summary>
        /// Returns true if both wheels are touching the ground.
        /// </summary>
        bool Grounded
        {
            get
            {
                return frontWheel.Grounded && rearWheel.Grounded;
            }
        }

        /// <summary>
        /// Returns true if one of the two wheels are touching the ground.
        /// </summary>
        bool PartiallyGrounded
        {
            get
            {
                return frontWheel.Grounded || rearWheel.Grounded;
            }
        }

        /// <summary>
        /// Gets the average wheel joint speed or sets the target motor speed.
        /// </summary>
        float WheelSpeed
        {
            get
            {
                return (rearWheel.WheelJoint.JointSpeed + frontWheel.WheelJoint.JointSpeed) * 0.5f;
            }
            set
            {
                rearWheel.WheelJoint.MotorSpeed = frontWheel.WheelJoint.MotorSpeed = value;
            }
        }

        /// <summary>
        /// Sets the max motor torque of the wheels.
        /// </summary>
        float WheelTorque
        {
            set
            {
                rearWheel.WheelJoint.MaxMotorTorque = frontWheel.WheelJoint.MaxMotorTorque = value;
            }
        }

        /// <summary>
        /// Gets a gravity vector relative to the car's rotation.
        /// </summary>
        Vector2 RelativeGravity
        {
            get
            {
                return Vector2.Transform(App.Instance.Scene.PhysicsWorld.Gravity, Matrix.CreateRotationZ(body.Rotation));
            }
        }

        /// <summary>
        /// Initializes the car by creating the body and wheels.
        /// </summary>
        protected override void OnInitialize()
        {
            body = Parent.AddComponent<BodyComponent>().Body;
            body.BodyType = BodyType.Static;

            FixtureComponent parentFixture = Parent.AddComponent<FixtureComponent>();
            parentFixture.Fixture = FixtureFactory.AttachRectangle(BodyWidth, BodyHeight, BodyDensity, Vector2.Zero, body);

            body.Friction = BodyFriction;
            body.AngularDamping = BodyAngularDamping;

            rearWheel = CreateWheel(new Vector2(-WheelOffsetX, WheelOffsetY));
            frontWheel = CreateWheel(new Vector2(WheelOffsetX, WheelOffsetY));

            throttleIndicator = Parent.AddComponent<StatusIndicator>();
            throttleIndicator.VerticalOffset = -48;

            limitIndicator = Parent.AddComponent<StatusIndicator>();
            limitIndicator.VerticalOffset = -56;
            limitIndicator.Width = 32;
            limitIndicator.Height = 4;
            limitIndicator.Color = Color.Red;
            limitIndicator.Visible = false;

            gravity = Parent.AddComponent<Gravity>();
            gravity.AddBody(body);
            gravity.AddBody(rearWheel.Body);
            gravity.AddBody(frontWheel.Body);
        }

        /// <summary>
        /// Updates the car's physics.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void OnUpdate(GameTime gameTime)
        {
            if (body.BodyType != BodyType.Dynamic)
                body.BodyType = BodyType.Dynamic;

            bool boosting = Input.Instance.GamePads[0].Buttons.X == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            bool jumping = Input.Instance.GamePads[0].Buttons.A == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            float throttle = boosting ? 1.0f : Input.Instance.GamePads[0].Triggers.Right - Input.Instance.GamePads[0].Triggers.Left;
            float rotation = Input.Instance.GamePads[0].ThumbSticks.Left.X;

            // Car rotation and wheel magnets
            if (Grounded && !jumping)
            {
                // Apply strong force against the ground
                rearWheel.Body.ApplyForce(-rearWheel.GroundNormal * WheelStrongForce);
                frontWheel.Body.ApplyForce(-frontWheel.GroundNormal * WheelStrongForce);
            }
            else if (PartiallyGrounded && !jumping)
            {
                // Apply weak force toward the ground
                rearWheel.Body.ApplyForce(-rearWheel.GroundNormal * WheelWeakForce);
                frontWheel.Body.ApplyForce(-frontWheel.GroundNormal * WheelWeakForce);
            }
            else
            {
                // Car is free - rotate
                body.ApplyAngularImpulse(rotation * BodyAngularImpulse);
            }

            if (body.LinearVelocity.Length() > SpeedLimit)
            {
                // Car is supersonic - limit the velocity
                limitIndicator.Visible = true;

                Vector2 limitVelocity = body.LinearVelocity;
                limitVelocity.Normalize();
                limitVelocity *= SpeedLimit;

                Vector2 differenceVector = body.LinearVelocity - limitVelocity;
                body.LinearVelocity -= differenceVector;
                rearWheel.Body.LinearVelocity -= differenceVector;
                frontWheel.Body.LinearVelocity -= differenceVector;
            }
            else
            {
                limitIndicator.Visible = false;
            }

            if (boosting) // Apply the boosting force at the car's absolute center of mass
                body.ApplyForce(Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ(body.Rotation)) * new Vector2(BoostForceX, BoostForceY),
                    PhysicsHelper.CenterOfMass(body, rearWheel.Body, frontWheel.Body));

            if (!PartiallyGrounded || throttle == 0.0f)
            {
                if (Grounded) // Apply fake gravity force + fake real gravity force
                    gravity.Value = RelativeGravity + Vector2.Transform(App.Instance.Scene.PhysicsWorld.Gravity,
                        Matrix.CreateRotationZ(body.Rotation - (float)(Math.PI * 0.5))) * (float)Math.Sin(body.Rotation);
                else // Car is free - apply normal gravity
                    gravity.Value = App.Instance.Scene.PhysicsWorld.Gravity;

                throttleIndicator.Color = Color.Yellow;

                WheelTorque = CoastingTorque;
                WheelSpeed = 0.0f;
            }
            else
            {
                // Set gravity according to the rotation of the car
                gravity.Value = RelativeGravity;

                if (throttle * WheelSpeed >= -ThrottleTolerance)
                {
                    // Accelerating
                    throttleIndicator.Color = Color.Green;

                    float absSpeed = Math.Abs(WheelSpeed);

                    // Modulate acceleration curves
                    if (absSpeed > WheelTopSpeed)
                        WheelTorque = 0.0f;
                    else if (absSpeed > Math.Abs(throttle) * WheelTopSpeed - AccelerationCurveThreshold)
                        WheelTorque = Math.Max(0, AccelerationTorque * ((Math.Abs(throttle) * WheelTopSpeed - absSpeed) / AccelerationCurveThreshold));
                    else
                        WheelTorque = AccelerationTorque;

                    WheelSpeed = throttle * WheelTopSpeed;
                }
                else
                {
                    // Braking
                    throttleIndicator.Color = Color.Red;
                    WheelTorque = BrakingTorque;
                    WheelSpeed = 0.0f;
                }
            }
        }

        /// <summary>
        /// Creates a wheel from the given relative position to the car.
        /// </summary>
        /// <param name="relativePosition"></param>
        /// <returns></returns>
        private Wheel CreateWheel(Vector2 relativePosition)
        {
            GameObject wheelObject = GameObject.Create(Parent);
            wheelObject.Position = Parent.Position + relativePosition;

            Wheel wheel = wheelObject.AddComponent<Wheel>();
            wheel.Create(WheelRadius, relativePosition);

            return wheel;
        }
    }
}
