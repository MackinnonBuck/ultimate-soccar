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
        /// Describes if the body is dynamic, kinematic, or static.
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

        protected override void OnInitialize()
        {
            Body = BodyFactory.CreateBody(App.Instance.Scene.PhysicsWorld, ConvertUnits.ToSimUnits(Parent.Position.X, Parent.Position.Y), Parent.Rotation);
            Body.UserData = this;
            BodyType = BodyType.Dynamic;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
        }

        protected override void OnDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {        
        }

        protected override void OnDestroy()
        {   
        }
    }
}
