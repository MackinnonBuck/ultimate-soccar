using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
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

            Body body = gameObject.AddComponent<BodyComponent>().Body;
            body.Mass = 1.0f;

            gameObject.Position = Vector2.Zero;

            gameObject.AddComponent<FixtureComponent>().Fixture = FixtureFactory.AttachRectangle(2.0f, 2.0f, 1.0f, Vector2.Zero, body);

            return gameObject;
        }
    }
}
