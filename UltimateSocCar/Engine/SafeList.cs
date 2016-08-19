using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateSocCar.Engine
{
    public class SafeList<T> : List<T>, IEnumerable<T>
    {
        /// <summary>
        /// Used for safely enumerating the SafeList.
        /// </summary>
        class SafeEnumerator : IEnumerator<T>
        {
            /// <summary>
            /// The indices removed between iterations.
            /// </summary>
            public List<int> RemovedIndices { get; private set; }

            SafeList<T> safeList;      
            int currentIndex;

            /// <summary>
            /// Creates a new SafeEnumerator instance.
            /// </summary>
            /// <param name="list"></param>
            public SafeEnumerator(SafeList<T> list)
            {
                RemovedIndices = new List<int>();

                safeList = list;
                currentIndex = -1;
            }

            /// <summary>
            /// Gets the current value.
            /// </summary>
            public T Current
            {
                get
                {
                    return safeList[currentIndex];
                }
            }

            /// <summary>
            /// Gets a boxed reference to the current value.
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            /// <summary>
            /// Disposes the SafeEnumerator.
            /// </summary>
            public void Dispose()
            {
                safeList.RemoveEnumerator(this);
            }

            /// <summary>
            /// Safely moves to the next element.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                foreach (int i in RemovedIndices)
                    if (i <= currentIndex)
                        currentIndex--;

                RemovedIndices.Clear();

                currentIndex++;

                return currentIndex < safeList.Count;
            }

            /// <summary>
            /// Resets the SafeEnumerator.
            /// </summary>
            public void Reset()
            {
                currentIndex = -1;
            }
        }

        List<SafeEnumerator> activeEnumerators;

        /// <summary>
        /// Initializes an empty SafeList.
        /// </summary>
        public SafeList()
        {
            activeEnumerators = new List<SafeEnumerator>();
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

            foreach (SafeEnumerator e in activeEnumerators)
                e.RemovedIndices.Add(IndexOf(value));

            base.Remove(value);
        }

        /// <summary>
        /// Safely removes each values matching the range from the SafeList.
        /// </summary>
        /// <param name="range"></param>
        public new void RemoveAll(Predicate<T> range)
        {
            List<int> removedIndices = new List<int>();

            foreach (T value in FindAll(range))
                removedIndices.Add(IndexOf(value));

            foreach (SafeEnumerator e in activeEnumerators)
                e.RemovedIndices.AddRange(removedIndices);

            base.RemoveAll(range);
        }

        /// <summary>
        /// Safely clears the SafeList.
        /// </summary>
        public new void Clear()
        {
            int[] removedIndices = new int[Count];

            for (int i = 0; i < removedIndices.Length; i++)
                removedIndices[i] = i;

            foreach (SafeEnumerator e in activeEnumerators)
                e.RemovedIndices.AddRange(removedIndices);

            base.Clear();
        }

        /// <summary>
        /// Returns a safe IEnumerator that can handle multiple values being removed during the enumeration.
        /// </summary>
        /// <returns>a safe IEnumerator that can handle multiple values being removed during the enumeration.</returns>
        public new IEnumerator<T> GetEnumerator()
        {
            SafeEnumerator enumerator = new SafeEnumerator(this);
            activeEnumerators.Add(enumerator);
            return enumerator;
        }

        private void RemoveEnumerator(SafeEnumerator enumerator)
        {
            if (activeEnumerators.Contains(enumerator))
                activeEnumerators.Remove(enumerator);
        }
    }
}
