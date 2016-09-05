using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Extensions
{
    public static class Parsing
    {
        public static bool TryParse(this Vector2 vector2, string s)
        {
            string[] values = s.Split(',');

            if (values.Length != 2)
                return false;

            float x, y;

            if (!float.TryParse(values[0], out x) || !float.TryParse(values[1], out y))
                return false;

            vector2.X = x;
            vector2.Y = y;

            return true;
        }
    }
}
