using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Runtime.Serialization.Formatters.Binary;

namespace MonoEngine.TMX
{
    [ContentTypeWriter]
    public class MapWriter : ContentTypeWriter<Map>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "MonoEngine.TMX.MapReader, MonoEngine";
        }

        protected override void Write(ContentWriter output, Map value)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(output.BaseStream, value);
        }
    }
}
