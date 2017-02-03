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
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

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
            GameObject gameObject = GameObject.Create(baseObject.Name ?? string.Empty);
            gameObject.Position = new Vector2(baseObject.X, baseObject.Y);

            Body body = gameObject.AddComponent<BodyComponent>().Body;

            switch (baseObject.VertexDataType)
            {
                case "polygon":
                    {
                        Vertices verts = Parsing.TryParseVertices(baseObject.VertexData);

                        verts.Rotate(MathHelper.ToRadians(baseObject.Rotation));
                        body.Position += verts.GetCentroid();
                        verts.Translate(-verts.GetCentroid());

                        gameObject.AddComponent<FixtureComponent>().Fixture = FixtureFactory.AttachPolygon(verts, 1.0f, body);
                    }
                    break;
                case "polyline":
                    {
                        Vertices verts = Parsing.TryParseVertices(baseObject.VertexData);

                        if (verts.Count < 2)
                            break;

                        verts.Rotate(MathHelper.ToRadians(baseObject.Rotation));
                        gameObject.AddComponent<FixtureComponent>().Fixture = verts.Count > 2 ? FixtureFactory.AttachChainShape(verts, body) :
                            FixtureFactory.AttachEdge(verts[0], verts[1], body);
                    }
                    break;
                case "ellipse":
                    {
                        if (baseObject.Width == baseObject.Height)
                        {
                            Fixture fixture = gameObject.AddComponent<FixtureComponent>().Fixture = FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(baseObject.Width) * 0.5f, 1.0f, body);

                            body.Position += new Vector2(fixture.Shape.Radius);
                            body.Rotation += MathHelper.ToRadians(baseObject.Rotation);
                        }
                        else
                        {
                            float xRadius = ConvertUnits.ToSimUnits(baseObject.Width) * 0.5f;
                            float yRadius = ConvertUnits.ToSimUnits(baseObject.Height) * 0.5f;

                            Fixture fixture = gameObject.AddComponent<FixtureComponent>().Fixture = FixtureFactory.AttachEllipse(
                                xRadius, yRadius, 16, 1.0f, body);

                            FarseerPhysics.Collision.Shapes.PolygonShape shape = (FarseerPhysics.Collision.Shapes.PolygonShape)fixture.Shape;
                            shape.Vertices.Translate(new Vector2(xRadius, yRadius));
                            shape.Vertices.Rotate(MathHelper.ToRadians(baseObject.Rotation));

                            Vector2 centroid = shape.Vertices.GetCentroid();

                            body.Position += centroid;
                            shape.Vertices.Translate(-centroid);

                            body.Rotation += MathHelper.ToRadians(baseObject.Rotation);
                            shape.Vertices.Rotate(-body.Rotation);
                        }
                    }
                    break;
                default:
                    {
                        Fixture fixture = gameObject.AddComponent<FixtureComponent>().Fixture = FixtureFactory.AttachRectangle(
                            ConvertUnits.ToSimUnits(baseObject.Width), ConvertUnits.ToSimUnits(baseObject.Height), 1.0f, Vector2.Zero, body);

                        FarseerPhysics.Collision.Shapes.PolygonShape shape = (FarseerPhysics.Collision.Shapes.PolygonShape)fixture.Shape;
                        shape.Vertices.Rotate(MathHelper.ToRadians(baseObject.Rotation));

                        if (baseObject.GID == -1)
                        {
                            body.Position += shape.Vertices[1]; // top-left vertex;
                        }
                        else
                        {
                            body.Position += shape.Vertices[0];
                            TileRenderer tr = gameObject.AddComponent<TileRenderer>();
                            tr.GID = baseObject.GID;
                        }

                        body.Rotation = MathHelper.ToRadians(baseObject.Rotation);
                        shape.Vertices.Rotate(-body.Rotation);
                    }
                    break;
            }

            return gameObject;
        }
    }
}
