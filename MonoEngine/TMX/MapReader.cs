using Microsoft.Xna.Framework.Content;
using MonoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MonoEngine.TMX
{
    public class MapReader : ContentTypeReader<Map>
    {
        /// <summary>
        /// Deserializes the loaded map and returns the Map instance.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="existingInstance"></param>
        /// <returns></returns>
        protected override Map Read(ContentReader input, Map existingInstance)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (Map)formatter.Deserialize(input.BaseStream);
        }
    }
}
