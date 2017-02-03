using FarseerPhysics.Dynamics;
using MonoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace UltimateSocCar.Components
{
    public class Gravity : Component
    {
        /// <summary>
        /// The bodies influenced by the Gravity component.
        /// </summary>
        private List<Body> bodies;

        /// <summary>
        /// The Gravity's value.
        /// </summary>
        public Vector2 Value { get; set; }

        /// <summary>
        /// Initializes a new Gravity instance.
        /// </summary>
        public Gravity()
        {
            bodies = new List<Body>();
            Value = Vector2.Zero;
        }

        /// <summary>
        /// Adds a Body to be influenced by gravity.
        /// </summary>
        /// <param name="body"></param>
        public void AddBody(Body body)
        {
            if (!bodies.Contains(body))
                bodies.Add(body);

            body.IgnoreGravity = true;
        }

        /// <summary>
        /// Removes a Body from the Gravity's influence.
        /// </summary>
        /// <param name="body"></param>
        public void RemoveBody(Body body)
        {
            if (bodies.Contains(body))
                bodies.Remove(body);

            body.IgnoreGravity = false;
        }

        /// <summary>
        /// Applies the artificial gravity force to the specified bodies.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (Body b in bodies)
                b.ApplyForce(Value * b.Mass);
        }
    }
}
