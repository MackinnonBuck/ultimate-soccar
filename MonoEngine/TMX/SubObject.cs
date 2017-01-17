using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.TMX
{
    [Serializable]
    public class SubObject : Element
    {
        /// <summary>
        /// The ID of the SubObject.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The GID of the SubObject.
        /// </summary>
        public int GID { get; set; }

        /// <summary>
        /// The type of the SubObject.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The X position of the SubObject.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// The Y position of the SubObject.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// The Width of the SubObject.
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// The height of the SubObject.
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// The rotation of the SubObject in degrees.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Specifies what type of vertex data the SubObject contains.
        /// </summary>
        public string VertexDataType { get; set; }

        /// <summary>
        /// The vertex data of the SubObject.
        /// </summary>
        public string VertexData { get; set; }

        /// <summary>
        /// Instantiates a new instance of SubObject.
        /// </summary>
        public SubObject()
        {
            GID = -1;
            VertexDataType = string.Empty;
            VertexData = string.Empty;
        }
    }
}
