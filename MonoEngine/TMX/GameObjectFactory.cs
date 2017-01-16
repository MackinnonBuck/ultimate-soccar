using MonoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.TMX
{
    public class GameObjectFactory
    {
        private static GameObjectFactory _instance;

        /// <summary>
        /// The GameObjectFactory instance.
        /// </summary>
        public static GameObjectFactory Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameObjectFactory();

                return _instance;
            }
        }

        /// <summary>
        /// The registered definitions of the GameObjectFactory.
        /// </summary>
        private Dictionary<string, GameObjectDefinition> definitions;

        /// <summary>
        /// Initializes a new GameObjectFactory instance.
        /// </summary>
        private GameObjectFactory()
        {
            definitions = new Dictionary<string, GameObjectDefinition>();

            RegisterDefinition<DefaultDefinition>();
        }

        /// <summary>
        /// Registers a new definition of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RegisterDefinition<T>() where T : GameObjectDefinition, new()
        {
            T definition = new T();

            if (definitions.ContainsKey(definition.Name))
                return;

            definitions.Add(definition.Name, definition);
        }

        /// <summary>
        /// Instantiates a new GameObject from the given registered type and a SubObject with the object information.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="baseObject"></param>
        /// <returns></returns>
        public GameObject Create(string type, SubObject baseObject)
        {
            if (!definitions.ContainsKey(type))
            {
                Debug.Log("Invalid definition name " + type + ".", Debug.LogSeverity.ERROR);
                return null;
            }

            return definitions[type].Create(baseObject);
        }
    }
}
