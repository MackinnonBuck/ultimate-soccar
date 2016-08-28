using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Core
{
    /// <summary>
    /// Used for creating overridable bindings for properties.
    /// </summary>
    internal class PropertyBinder
    {
        /// <summary>
        /// Represents the binding associated with a particular property.
        /// </summary>
        public class Binding
        {
            object parentObject;
            PropertyInfo propertyInfo;

            /// <summary>
            /// The default property for the binding.
            /// </summary>
            object DefaultValue { get; set; }

            /// <summary>
            /// The property linked to the Binding.
            /// </summary>
            public object Value
            {
                get
                {
                    return propertyInfo?.GetValue(parentObject);
                }
                set
                {
                    propertyInfo?.SetValue(parentObject, value);
                }
            }

            /// <summary>
            /// Creates a new Binding instance with a default property bound to it.
            /// </summary>
            public Binding()
            {
                SetBinding(this, "DefaultValue");
            }

            /// <summary>
            /// Sets the binding to a particular property from the given parent.
            /// </summary>
            /// <param name="parent"></param>
            /// <param name="propertyName"></param>
            public void SetBinding(object parent, string propertyName)
            {
                if (parent == null || propertyName == null)
                    return;

                parentObject = parent;
                propertyInfo = parent.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            }
        }

        /// <summary>
        /// The collection of Bindings.
        /// </summary>
        Dictionary<string, Binding> Bindings;

        /// <summary>
        /// Creates a new PropertyBinder instance.
        /// </summary>
        public PropertyBinder()
        {
            Bindings = new Dictionary<string, Binding>();
        }
        
        /// <summary>
        /// Used for accessing a binding owned by the PropertyBinder.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Binding this[string key]
        {
            get
            {
                return Bindings.ContainsKey(key) ? Bindings[key] : (Bindings[key] = new Binding());
            }
        }
    }
}
