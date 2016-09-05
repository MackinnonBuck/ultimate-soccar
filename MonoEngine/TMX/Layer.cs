using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.TMX
{
    [Serializable]
    public class Layer : Element
    {
        /// <summary>
        /// The name of the Layer.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Creates an empty Layer.
        /// </summary>
        public Layer()
        {
            Name = string.Empty;
        }
    }
}
