using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.TMX
{
    [Serializable]
    public abstract class Element
    {
        /// <summary>
        /// The custom properties associated with the Map.
        /// </summary>
        public Dictionary<string, string> Properties { get; protected set; }

        /// <summary>
        /// Initializes a new Element instance.
        /// </summary>
        public Element()
        {
            Properties = new Dictionary<string, string>();
        }
    }
}
