using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.TMX
{
    [Serializable]
    public class ImageLayer : Layer
    {
        /// <summary>
        /// The opacity of the image to display.
        /// </summary>
        public float Opacity { get; set; }

        /// <summary>
        /// The horizontal offset of the image to display.
        /// </summary>
        public float OffsetX { get; set; }

        /// <summary>
        /// The vertical offset of the image to display.
        /// </summary>
        public float OffsetY { get; set; }
        
        /// <summary>
        /// The source of the image to display.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// The width of the image to display.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The height of the image to display.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Initializes a new ImageLayer instance.
        /// </summary>
        public ImageLayer()
        {
            Opacity = 1.0f;
        }
    }
}
