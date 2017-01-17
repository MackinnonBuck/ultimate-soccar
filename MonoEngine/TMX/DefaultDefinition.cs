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
using FarseerPhysics.Collision;

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
        /// Creates a static, physics-based GameObject from the given SubObject.
        /// </summary>
        /// <param name="baseObject"></param>
        /// <returns></returns>
        public override GameObject Create(SubObject baseObject)
        {
            GameObject gameObject = GameObject.Create();
            gameObject.Position = new Vector2(baseObject.X, baseObject.Y);

            PhysicsBody body = gameObject.AddComponent<PhysicsBody>();
            body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            switch (baseObject.VertexDataType)
            {
                case "polygon":
                    {
                        Vertices verts = Parsing.TryParseVertices(baseObject.VertexData);

                        verts.Rotate(MathHelper.ToRadians(baseObject.Rotation));
                        body.Position += verts.GetCentroid();
                        verts.Translate(-verts.GetCentroid());

                        gameObject.AddComponent<PolygonShape>().Vertices = verts;
                    }
                    break;
                case "polyline":
                    {
                        Vertices verts = Parsing.TryParseVertices(baseObject.VertexData);

                        verts.Rotate(MathHelper.ToRadians(baseObject.Rotation));
                        gameObject.AddComponent<ChainShape>().Vertices = verts;
                    }
                    break;
                case "ellipse":
                    {
                        if (baseObject.Width == baseObject.Height)
                        {
                            CircleShape cc = gameObject.AddComponent<CircleShape>();

                            cc.Radius = ConvertUnits.ToSimUnits(baseObject.Width) * 0.5f;

                            body.Position += new Vector2(cc.Radius);
                            body.Rotation += MathHelper.ToRadians(baseObject.Rotation);
                        }
                        else
                        {
                            EllipseShape es = gameObject.AddComponent<EllipseShape>();

                            es.XRadius = ConvertUnits.ToSimUnits(baseObject.Width) * 0.5f;
                            es.YRadius = ConvertUnits.ToSimUnits(baseObject.Height) * 0.5f;
                            es.Translate(new Vector2(es.XRadius, es.YRadius));
                            es.Rotate(MathHelper.ToRadians(baseObject.Rotation));

                            Vector2 centroid = es.GetVertices().GetCentroid();

                            body.Position += centroid;
                            es.Translate(-centroid);

                            body.Rotation += MathHelper.ToRadians(baseObject.Rotation);
                            es.Rotate(-body.Rotation);
                        }
                    }
                    break;
                default:
                    {
                        RectangleShape rc = gameObject.AddComponent<RectangleShape>();

                        rc.Width = ConvertUnits.ToSimUnits(baseObject.Width);
                        rc.Height = ConvertUnits.ToSimUnits(baseObject.Height);
                        rc.Angle = MathHelper.ToRadians(baseObject.Rotation);

                        if (baseObject.GID == -1)
                        {
                            body.Position += rc.GetVertices()[1]; // top-left vertex;
                        }
                        else
                        {
                            body.Position += rc.GetVertices()[0];
                            TileRenderer tr = gameObject.AddComponent<TileRenderer>();
                            tr.GID = baseObject.GID;
                        }

                        body.Rotation += rc.Angle;
                        rc.Angle = 0;
                    }
                    break;
            }

            return gameObject;
        }
    }
}
