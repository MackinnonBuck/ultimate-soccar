using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateSocCar.Engine
{
    public class SafeList<T> : List<T>, IEnumerable<T>
    {
        bool isEnumerating;
        List<int> removedIndices;

        /// <summary>
        /// Initializes an empty SafeList.
        /// </summary>
        public SafeList()
        {
            isEnumerating = false;
            removedIndices = new List<int>();
        }

        /// <summary>
        /// Initializes a SafeList from the given IEnumerable.
        /// </summary>
        /// <param name="collection"></param>
        public SafeList(IEnumerable<T> collection)
            : this()
        {
            AddRange(collection);
        }

        /// <summary>
        /// Safely removes a value from the SafeList.
        /// </summary>
        /// <param name="value"></param>
        public new void Remove(T value)
        {
            if (!Contains(value))
                return;

            if (isEnumerating)
                removedIndices.Add(IndexOf(value));

            base.Remove(value);
        }

        /// <summary>
        /// Safely removes each values matching the range from the SafeList.
        /// </summary>
        /// <param name="range"></param>
        public new void RemoveAll(Predicate<T> range)
        {
            foreach (T value in FindAll(range))
                removedIndices.Add(IndexOf(value));

            base.RemoveAll(range);
        }

        /// <summary>
        /// Safely clears the SafeList.
        /// </summary>
        public new void Clear()
        {
            if (isEnumerating)
                for (int i = 0; i < Count; i++)
                    removedIndices.Add(i);

            base.Clear();
        }

        /// <summary>
        /// Returns a safe IEnumerator that can handle multiple values being removed during the enumeration.
        /// </summary>
        /// <returns>a safe IEnumerator that can handle multiple values being removed during the enumeration.</returns>
        public new IEnumerator<T> GetEnumerator()
        {
            return SafeEnumerate().GetEnumerator();
        }

        private IEnumerable<T> SafeEnumerate()
        {
            isEnumerating = true;

            for (int i = 0; i < Count; i++)
            {
                yield return this[i];

                foreach (int j in removedIndices)
                    if (j <= i)
                        i--;

                removedIndices.Clear();
            }

            isEnumerating = false;
        }
    }
}
