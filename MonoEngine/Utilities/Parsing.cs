using FarseerPhysics;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using MonoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Utilities
{
    public static class Parsing
    {
        /// <summary>
        /// Returns a Vector2 from the given string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector2? TryParseVector2(string s)
        {
            string[] values = s.Split(',');

            if (values.Length != 2)
                return null;

            float x, y;

            if (!float.TryParse(values[0], out x) || !float.TryParse(values[1], out y))
                return null;

            return new Vector2(x, y);
        }

        /// <summary>
        /// Returns vertices from the given string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vertices TryParseVertices(string s)
        {
            Vertices verts = new Vertices();

            foreach (string point in s.Split(' '))
            {
                Vector2? vert = TryParseVector2(point);

                if (!vert.HasValue)
                    return null;

                verts.Add(ConvertUnits.ToSimUnits(vert.Value));
            }

            return verts;
        }
    }
}
