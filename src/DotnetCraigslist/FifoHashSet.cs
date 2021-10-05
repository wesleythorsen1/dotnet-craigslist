using System;
using System.Collections.Generic;

namespace DotnetCraigslist
{
    internal class FifoHashSet<T>
    {
        private int _capacity;
        private int _i = 0;
        private T[] _queue;
        private HashSet<T> _lookup;

        public FifoHashSet(int capacity)
        {
            if (capacity < 1)
                throw new ArgumentException("Value must be greater than 0.", nameof(capacity));

            _capacity = capacity;
            _queue = new T[_capacity];
            _lookup = new HashSet<T>(_capacity);
        }

        public bool Contains(T item)
            => _lookup.Contains(item);

        public void Add(T item)
        {
            _lookup.Remove(_queue[_i]);
            _queue[_i] = item;
            _lookup.Add(item);

            _i = ++_i % _capacity;
        }
    }
}