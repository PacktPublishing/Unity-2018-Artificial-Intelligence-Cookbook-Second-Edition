using System;
using System.Collections.Generic;

namespace GPWiki
{
    /// <summary>
    /// A binary heap, useful for sorting data and priority queues.
    /// </summary>
    /// <typeparam name="T"><![CDATA[IComparable<T> type of item in the heap]]>.</typeparam>
    public class BinaryHeap<T> : ICollection<T> where T : IComparable<T>
    {
        // Constants
        private const int DEFAULT_SIZE = 4;
        // Fields
        private T[] _data = new T[DEFAULT_SIZE];
        private int _count = 0;
        private int _capacity = DEFAULT_SIZE;
        private bool _sorted;

        // Properties
        /// <summary>
        /// Gets the number of values in the heap. 
        /// </summary>
        public int Count
        {
            get { return _count; }
        }
        /// <summary>
        /// Gets or sets the capacity of the heap.
        /// </summary>
        public int Capacity
        {
            get { return _capacity; }
            set
            {
                int previousCapacity = _capacity;
                _capacity = Math.Max(value, _count);
                if (_capacity != previousCapacity)
                {
                    T[] temp = new T[_capacity];
                    Array.Copy(_data, temp, _count);
                    _data = temp;
                }
            }
        }
        // Methods
        /// <summary>
        /// Creates a new binary heap.
        /// </summary>
        public BinaryHeap()
        {
        }
        private BinaryHeap(T[] data, int count)
        {
            Capacity = count;
            _count = count;
            Array.Copy(data, _data, count);
        }
        /// <summary>
        /// Gets the first value in the heap without removing it.
        /// </summary>
        /// <returns>The lowest value of type TValue.</returns>
        public T Peek()
        {
            return _data[0];
        }

        /// <summary>
        /// Removes all items from the heap.
        /// </summary>
        public void Clear()
        {
            this._count = 0;
            _data = new T[_capacity];
        }
        /// <summary>
        /// Adds a key and value to the heap.
        /// </summary>
        /// <param name="item">The item to add to the heap.</param>
        public void Add(T item)
        {
            if (_count == _capacity)
            {
                Capacity *= 2;
            }
            _data[_count] = item;
            UpHeap();
            _count++;
        }

        /// <summary>
        /// Removes and returns the first item in the heap.
        /// </summary>
        /// <returns>The next value in the heap.</returns>
        public T Remove()
        {
            if (this._count == 0)
            {
                throw new InvalidOperationException("Cannot remove item, heap is empty.");
            }
            T v = _data[0];
            _count--;
            _data[0] = _data[_count];
            _data[_count] = default(T); //Clears the Last Node
            DownHeap();
            return v;
        }
        private void UpHeap()
        //helper function that performs up-heap bubbling
        {
            _sorted = false;
            int p = _count;
            T item = _data[p];
            int par = Parent(p);
            while (par > -1 && item.CompareTo(_data[par]) < 0)
            {
                _data[p] = _data[par]; //Swap nodes
                p = par;
                par = Parent(p);
            }
            _data[p] = item;
        }
        private void DownHeap()
        //helper function that performs down-heap bubbling
        {
            _sorted = false;
            int n;
            int p = 0;
            T item = _data[p];
            while (true)
            {
                int ch1 = Child1(p);
                if (ch1 >= _count) break;
                int ch2 = Child2(p);
                if (ch2 >= _count)
                {
                    n = ch1;
                }
                else
                {
                    n = _data[ch1].CompareTo(_data[ch2]) < 0 ? ch1 : ch2;
                }
                if (item.CompareTo(_data[n]) > 0)
                {
                    _data[p] = _data[n]; //Swap nodes
                    p = n;
                }
                else
                {
                    break;
                }
            }
            _data[p] = item;
        }
        private void EnsureSort()
        {
            if (_sorted) return;
            Array.Sort(_data, 0, _count);
            _sorted = true;
        }
        private static int Parent(int index)
        //helper function that calculates the parent of a node
        {
            return (index - 1) >> 1;
        }
        private static int Child1(int index)
        //helper function that calculates the first child of a node
        {
            return (index << 1) + 1;
        }
        private static int Child2(int index)
        //helper function that calculates the second child of a node
        {
            return (index << 1) + 2;
        }

        /// <summary>
        /// Creates a new instance of an identical binary heap.
        /// </summary>
        /// <returns>A BinaryHeap.</returns>
        public BinaryHeap<T> Copy()
        {
            return new BinaryHeap<T>(_data, _count);
        }

        /// <summary>
        /// Gets an enumerator for the binary heap.
        /// </summary>
        /// <returns>An IEnumerator of type T.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            EnsureSort();
            for (int i = 0; i < _count; i++)
            {
                yield return _data[i];
            }
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Checks to see if the binary heap contains the specified item.
        /// </summary>
        /// <param name="item">The item to search the binary heap for.</param>
        /// <returns>A boolean, true if binary heap contains item.</returns>
        public bool Contains(T item)
        {
            EnsureSort();
            return Array.BinarySearch<T>(_data, 0, _count, item) >= 0;
        }
        /// <summary>
        /// Copies the binary heap to an array at the specified index.
        /// </summary>
        /// <param name="array">One dimensional array that is the destination of the copied elements.</param>
        /// <param name="arrayIndex">The zero-based index at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            EnsureSort();
            Array.Copy(_data, array, _count);
        }
        /// <summary>
        /// Gets whether or not the binary heap is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        /// <summary>
        /// Removes an item from the binary heap. This utilizes the type T's Comparer and will not remove duplicates.
        /// </summary>
        /// <param name="item">The item to be removed.</param>
        /// <returns>Boolean true if the item was removed.</returns>
        public bool Remove(T item)
        {
            EnsureSort();
            int i = Array.BinarySearch<T>(_data, 0, _count, item);
            if (i < 0) return false;
            Array.Copy(_data, i + 1, _data, i, _count - i);
            _data[_count] = default(T);
            _count--;
            return true;
        }
    }
}