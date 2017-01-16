using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.TMX
{
    [Serializable]
    public class ObjectGroup : Layer
    {
        /// <summary>
        /// The child SubObjects of the ObjectGroup.
        /// </summary>
        public List<SubObject> Children { get; }

        /// <summary>
        /// Initializes a new ObjectGroup instance.
        /// </summary>
        public ObjectGroup()
        {
            Name = string.Empty;
            Children = new List<SubObject>();
        }
    }
}
