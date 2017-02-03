using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MonoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Components
{
    public class BodyComponent : Component
    {
        /// <summary>
        /// The Farseer Physics Body associated with the PhysicsBody.
        /// </summary>
        public Body Body { get; private set; }

        /// <summary>
        /// The position of the Body in display units.
        /// </summary>
        public Vector2 DisplayPosition
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
        /// Initializes the PhysicsBody.
        /// </summary>
        protected override void OnInitialize()
        {
            if (Parent.GetComponents<BodyComponent>().Count > 1)
            {
                Debug.Log("Cannot add more than one PhysicsBody to a GameObject.", Debug.LogSeverity.ERROR);
                Destroy();
                return;
            }

            Body = new Body(App.Instance.Scene.PhysicsWorld, ConvertUnits.ToSimUnits(Parent.Position), Parent.Rotation, this);

            Parent.PropertyBinder["Position"].SetBinding(this, "DisplayPosition");
            Parent.PropertyBinder["Rotation"].SetBinding(Body, "Rotation");
        }

        /// <summary>
        /// Removes the Body from the world when the PhysicsBody is destroyed.
        /// </summary>
        protected override void OnDestroy()
        {
            if (Body != null)
                App.Instance.Scene.PhysicsWorld.RemoveBody(Body);
        }
    }
}
