using MonoEngine.TMX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoEngine.Core;
using MonoEngine.Components;
using Microsoft.Xna.Framework;
using UltimateSocCar.Components;

namespace UltimateSocCar.Definitions
{
    public class CarDefinition : GameObjectDefinition
    {
        public CarDefinition() : base("Car")
        {
        }

        public override GameObject Create(SubObject baseObject)
        {
            GameObject carBody = GameObject.Create(baseObject.Name);
            carBody.Position = new Vector2(baseObject.X + baseObject.Width / 2, baseObject.Y + baseObject.Height / 2);
            carBody.AddComponent<Car>();

            return carBody;
        }
    }
}
