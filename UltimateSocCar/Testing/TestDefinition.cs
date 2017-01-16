using Microsoft.Xna.Framework;
using MonoEngine.Components;
using MonoEngine.Core;
using MonoEngine.TMX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateSocCar.Testing
{
    public class TestDefinition : GameObjectDefinition
    {
        public TestDefinition() : base("test")
        {
        }

        public override GameObject Create(SubObject baseObject)
        {
            GameObject gameObject = GameObject.Create();

            PhysicsBody body = gameObject.AddComponent<PhysicsBody>();
            body.Mass = 1.0f;

            gameObject.Position = Vector2.Zero;

            RectangleShape shape = gameObject.AddComponent<RectangleShape>();
            shape.Width = 2.0f;
            shape.Height = 2.0f;

            return gameObject;
        }
    }
}
