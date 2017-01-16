using MonoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.TMX
{
    public abstract class GameObjectDefinition
    {
        /// <summary>
        /// The name of the GameObjectDefinition.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Called when a GameObject is to be created.
        /// </summary>
        /// <param name="baseObject"></param>
        /// <returns></returns>
        abstract public GameObject Create(SubObject baseObject);

        /// <summary>
        /// Instantiates a new GameObjectDefinition with the given name.
        /// </summary>
        /// <param name="name"></param>
        public GameObjectDefinition(string name)
        {
            Name = name;
        }
    }
}
