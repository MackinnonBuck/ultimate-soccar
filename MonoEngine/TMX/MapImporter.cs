using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MonoEngine.TMX
{
    [ContentImporter(".tmx", DefaultProcessor = "MapProcessor", DisplayName = "MapImporter - MonoEngine")]
    public class MapImporter : ContentImporter<XmlReader>
    {
        /// <summary>
        /// Opens and returns an XmlReader to the given file.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override XmlReader Import(string filename, ContentImporterContext context)
        {
            context.Logger.LogMessage("Importing Map...");
            return XmlReader.Create(filename);
        }
    }
}
