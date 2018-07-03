using System;
using System.Collections.Generic;

namespace Gonity
{
    public class Navigator : INavigator
    {
        private class NavigatorItem
        {
            public object viewType;
            public OpenMode openMode;
        }

        private IMediatorMap _mediatorMap;

        private Dictionary<object, NavigatorItem> _navigatorItems;
        private List<object> _stack;

        public Navigator(IMediatorMap mediatorMap)
        {
            _mediatorMap = mediatorMap;
            _navigatorItems = new Dictionary<object, NavigatorItem>();
            _stack = new List<object>();
        }

        public void Register<T>(object viewType, OpenMode openMode = 0)
        {
            _mediatorMap.Map<T>(viewType);
            _navigatorItems.Add(viewType, new NavigatorItem { viewType = viewType, openMode = openMode });
        }

        public void Open(object viewType, Argument argument = null)
        {
            if (!_navigatorItems.ContainsKey(viewType)) throw new Exception(string.Format("Navigator.Open() : {0} is not registered", viewType));

            NavigatorItem item = _navigatorItems[viewType];
            if (item.openMode.HasFlag(OpenMode.Unique))
            {
                int index = _stack.FindIndex(v => v == viewType);
                if (index != -1)
                {
                    _stack.RemoveAt(index);
                }
            }

            if (!item.openMode.HasFlag(OpenMode.Overlay) && _stack.Count > 0)
            {
                _mediatorMap.Close(_stack[_stack.Count - 1]);
            }

            _stack.Add(viewType);
            _mediatorMap.Open(viewType, argument);
        }

        public void GoBack()
        {
            if (_stack.Count <= 0) return;

            object viewType = _stack[_stack.Count - 1];
            NavigatorItem currentItem = _navigatorItems[viewType];

            _mediatorMap.Close(viewType);

            _stack.RemoveAt(_stack.Count - 1);

            if (!currentItem.openMode.HasFlag(OpenMode.Overlay) && _stack.Count > 0)
            {
                _mediatorMap.Open(_stack[_stack.Count - 1]);
            }
        }
    }
}