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
using FarseerPhysics;
using Microsoft.Xna.Framework.Input;
using UltimateSocCar.Utilities;

namespace UltimateSocCar.Components
{
    public class Car : Component
    {
        // Constants
        const float TopSpeed = 14.0f;
        const float SpeedLimit = 23.0f;
        const float SupersonicTolerance = 1.0f;

        const float WheelOffsetX = 0.4f;
        const float WheelOffsetY = 0.15f;
        const float WheelRadius = 0.15f;

        const float WheelStrongForce = 10.0f;
        const float WheelWeakForce = 5.0f;

        const float BoostForceX = 5f;
        const float BoostForceY = 8f;

        const float AccelerationTorque = 0.5f;
        const float AccelerationCurveThreshold = TopSpeed * 0.75f;
        const float BrakingTorque = 0.75f;
        const float CoastingTorque = 0.1f;

        const float BodyWidth = 1.18f;
        const float BodyHeight = 0.36f;

        const float BodyDensity = 1.0f;
        const float BodyFriction = 0.1f;
        const float BodyAngularDamping = 10.0f;
        const float BodyAngularImpulse = 0.05f;

        const float ThrottleTolerance = 0.1f;

        const float FirstJumpImpulse = 2.0f;
        const float FirstJumpForce = 6.0f;
        const float FirstJumpHoldTime = 0.5f;
        const float JumpCooldownThreshold = 0.25f;
        const float SecondJumpDelayTime = 1.5f;

        // Physics components

        Body body;

        Wheel frontWheel;
        Wheel rearWheel;

        Gravity gravity;

        StatusIndicator throttleIndicator;
        StatusIndicator limitIndicator;

        TimeKeeper timeKeeper;
        TimeKeeper.Timer jumpHoldTimer;
        TimeKeeper.Timer secondJumpTimer;

        Vector2 firstJumpDirection;

        bool tryJump;
        bool canDodge;

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
        /// Returns true if one of the wheels is sticky.
        /// </summary>
        bool Sticky
        {
            get
            {
                return frontWheel.Sticky || rearWheel.Sticky;
            }
        }

        /// <summary>
        /// Returns true if the car is supersonic.
        /// </summary>
        bool Supersonic
        {
            get
            {
                return body.LinearVelocity.Length() > SpeedLimit - SupersonicTolerance;
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
        /// Initializes the car by creating the body and wheels.
        /// </summary>
        protected override void OnInitialize()
        {
            Parent.AddComponent<TextureRenderer>().TextureID = "Car";

            Input.Instance.OnButtonStateChanged += ButtonStateChanged;

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

            timeKeeper = new TimeKeeper();
            jumpHoldTimer = new TimeKeeper.Timer();
            secondJumpTimer = new TimeKeeper.Timer();

            firstJumpDirection = Vector2.Zero;

            tryJump = false;
            canDodge = false;
        }

        /// <summary>
        /// Updates the car's physics.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void OnUpdate(GameTime gameTime)
        {
            timeKeeper.Update();

            if (body.BodyType != BodyType.Dynamic)
                body.BodyType = BodyType.Dynamic;

            bool boosting = Input.Instance.GamePads[0].Buttons.X == ButtonState.Pressed;
            float throttle = boosting ? 1.0f : Input.Instance.GamePads[0].Triggers.Right - Input.Instance.GamePads[0].Triggers.Left;
            float rotation = Input.Instance.GamePads[0].ThumbSticks.Left.X;

            if (Grounded)
                canDodge = true;

            if (tryJump)
            {
                if (Grounded)
                {
                    firstJumpDirection = Vector2.Transform(-Vector2.UnitY, Matrix.CreateRotationZ(body.Rotation));
                    body.ApplyLinearImpulse(firstJumpDirection * FirstJumpImpulse);
                    jumpHoldTimer = timeKeeper.StartTimer(FirstJumpHoldTime, null, () => Input.Instance.GamePads[0].IsButtonUp(Buttons.A));
                    secondJumpTimer = timeKeeper.StartTimer(SecondJumpDelayTime, () => canDodge = false, () => secondJumpTimer.SecondsRemaining < SecondJumpDelayTime - JumpCooldownThreshold && Grounded);
                }
                else
                {
                    if (canDodge)
                    {
                        canDodge = false;
                        body.ApplyLinearImpulse(Vector2.Transform(-Vector2.UnitY, Matrix.CreateRotationZ(body.Rotation)) * FirstJumpImpulse);
                        secondJumpTimer.Cancel();
                    }
                }
            }

            body.ApplyForce(firstJumpDirection * FirstJumpForce * (jumpHoldTimer.SecondsRemaining / FirstJumpHoldTime));

            // Car rotation and wheel magnets
            if (Sticky)
            {
                rearWheel.Body.ApplyForce(-rearWheel.GroundNormal * (Grounded ? WheelStrongForce : WheelWeakForce));
                frontWheel.Body.ApplyForce(-frontWheel.GroundNormal * (Grounded ? WheelStrongForce : WheelWeakForce));
            }
            else
            {
                // Car is free - rotate
                body.ApplyAngularImpulse(rotation * BodyAngularImpulse);
            }

            if (!Sticky || throttle == 0.0f)
            {
                if (Grounded) // Apply fake gravity force + fake real gravity force
                    gravity.Value = Vector2.Transform(App.Instance.Scene.PhysicsWorld.Gravity,
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
                gravity.Value = Vector2.Zero;

                if (throttle * WheelSpeed >= -ThrottleTolerance)
                {
                    // Accelerating
                    throttleIndicator.Color = Color.Green;

                    float carSpeed = body.LinearVelocity.Length();

                    if (carSpeed > TopSpeed)
                        WheelTorque = 0.0f;
                    else if (carSpeed > Math.Abs(throttle) * TopSpeed - AccelerationCurveThreshold)
                        WheelTorque = Math.Max(0.0f, AccelerationTorque * ((Math.Abs(throttle) * TopSpeed - carSpeed) / AccelerationCurveThreshold));
                    else
                        WheelTorque = AccelerationTorque;

                    WheelSpeed = throttle > 0 ? float.MaxValue : float.MinValue;
                }
                else
                {
                    // Braking
                    throttleIndicator.Color = Color.Red;
                    WheelTorque = BrakingTorque;
                    WheelSpeed = 0.0f;
                }
            }

            if (body.LinearVelocity.Length() > SpeedLimit)
            {
                // Car has exceeded the sped limit - limit the velocity
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
                body.ApplyForce(Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ(body.Rotation)) *
                    new Vector2(BoostForceX, Grounded ? BoostForceX : BoostForceY),
                    PhysicsHelper.CenterOfMass(body, rearWheel.Body, frontWheel.Body));

            tryJump = false;
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

        /// <summary>
        /// Removes the ButtonStateChanged event listener.
        /// </summary>
        protected override void OnDestroy()
        {
            Input.Instance.OnButtonStateChanged -= ButtonStateChanged;
        }

        /// <summary>
        /// Listens for button presses.
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="button"></param>
        /// <param name="state"></param>
        private void ButtonStateChanged(int playerID, Buttons button, ButtonState state)
        {
            if (button == Buttons.A && state == ButtonState.Pressed)
                tryJump = true;
        }
    }
}
