using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoEngine.Core;
using MonoEngine.Components;
using Microsoft.Xna.Framework;
using MonoEngine.Utilities;
using FarseerPhysics.Common;
using FarseerPhysics;

namespace MonoEngine.TMX
{
    public class DefaultDefinition : GameObjectDefinition
    {
        /// <summary>
        /// Initializes a new DefaultDefinition.
        /// </summary>
        public DefaultDefinition() : base("DefaultDefinition")
        {
        }

        /// <summary>
        /// Creates a kinematic, physics-based GameObject from the given BaseObject.
        /// </summary>
        /// <param name="baseObject"></param>
        /// <returns></returns>
        public override GameObject Create(SubObject baseObject)
        {
            GameObject gameObject = GameObject.Create();
            gameObject.Position = new Vector2(baseObject.X, baseObject.Y);

            PhysicsBody body = gameObject.AddComponent<PhysicsBody>();
            body.BodyType = FarseerPhysics.Dynamics.BodyType.Kinematic;

            switch (baseObject.VertexDataType)
            {
                case "polygon":
                    gameObject.AddComponent<PolygonShape>().ParseVertexString(baseObject.VertexData);
                    break;
                case "polyline":
                    gameObject.AddComponent<ChainShape>().ParseVertexString(baseObject.VertexData);
                    break;
                case "ellipse":
                    if (baseObject.Width != baseObject.Height)
                        Debug.Log("MonoEngine does not support ellipse shapes. Generating a circle from the ellipse's width.",
                            Debug.LogSeverity.WARNING);

                    CircleShape cc = gameObject.AddComponent<CircleShape>();

                    cc.Radius = ConvertUnits.ToSimUnits(baseObject.Width) * 0.5f;

                    body.Position += new Vector2(cc.Radius);
                    break;
                default:
                    RectangleShape rc = gameObject.AddComponent<RectangleShape>();

                    rc.Width = ConvertUnits.ToSimUnits(baseObject.Width);
                    rc.Height = ConvertUnits.ToSimUnits(baseObject.Height);

                    body.Position += new Vector2(rc.Width * 0.5f, rc.Height * 0.5f);
                    break;
            }

            return gameObject;
        }
    }
}
