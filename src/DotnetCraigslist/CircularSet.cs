using System;
using System.Collections.Generic;

namespace DotnetCraigslist
{
    internal class CircularSet<T>
    {
        private readonly int _capacity;
        private readonly T[] _queue;
        private readonly HashSet<T> _lookup;
        private int _i = 0;

        public CircularSet(int capacity)
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