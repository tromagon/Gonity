using System;
using System.Collections.Generic;

namespace Gonity
{
    public struct ListReadOnly<T>
    {
        private List<T> _list;

        public int Count
        {
            get { return _list == null ? 0 : _list.Count; }
        }

        public List<T> Clone() 
        {
            return _list == null ? null : new List<T>(_list);
        }

        public ListReadOnly(List<T> list)
        {
            _list = list;
        }

        public T this[int key] { get { return _list[key]; }    }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public T Find(Predicate<T> match)
        {
            return _list.Find(match);
        }
    }
}
