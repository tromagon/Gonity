using System;
using System.Collections.Generic;

namespace Gonity
{
    public class PropertyList<T>
    {
        private DelegateHandler _onChanged = new DelegateHandler();
        public IDelegateHandler onChanged { get { return _onChanged; } }

        private DelegateHandler<T> _onAdd = new DelegateHandler<T>();
        public IDelegateHandler<T> onAdd { get { return _onAdd; } }

        private DelegateHandler<T> _onRemove = new DelegateHandler<T>();
        public IDelegateHandler<T> onRemove { get { return _onRemove; } }

        private List<T> _list = new List<T>();

        public void Add(T value)
        {
            _list.Add(value);
            _onChanged.Invoke();
            _onAdd.Invoke(value);
        }

        public void Remove(T value)
        {
            _list.Remove(value);
            _onChanged.Invoke();
            _onRemove.Invoke(value);
        }

        public T this[int key] { get { return _list[key]; } }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public T Find(Predicate<T> match)
        {
            return _list.Find(match);
        }

        public void Sort(IComparer<T> comparer)
        {
            _list.Sort(comparer);
        }

        public void Sort(Comparison<T> comparison)
        {
            _list.Sort(comparison);
        }
    }
}