using System;
using System.Collections.Generic;
using System.Reflection;

namespace Gonity
{
    public class MediatorMap : IMediatorMap
    {
        private class MediatorItem
        {
            public Type mediatorType;
            public Type viewType;
            public FieldInfo viewField;
            public FieldInfo argumentField;
            public MethodInfo openMethod;
            public MethodInfo closeMethod;
        }

        private class ViewItem
        {
            public object mediator;
            public object view;
        }

        private IViewFactory _viewFactory;
        private IInjector _injector;
        private Dictionary<object, MediatorItem> _items;
        private Dictionary<object, ViewItem> _opened;

        public MediatorMap(IViewFactory viewFactory, IInjector injector = null)
        {
            _viewFactory = viewFactory;
            _injector = injector;
            _items = new Dictionary<object, MediatorItem>();
            _opened = new Dictionary<object, ViewItem>();
        }

        public void Map<T>(object viewKey)
        {
            if (_items.ContainsKey(viewKey)) throw new Exception(string.Format("{0} is already mapped", viewKey));

            Type mediatorType = typeof(T);
            FieldInfo viewField = ReflectionUtils.GetCustomAttributeField<ViewElement>(mediatorType);

            if (viewField == null) throw new Exception(string.Format("Could not find any [View] attribute on {0}", mediatorType));

            _items.Add(viewKey,
                new MediatorItem
                {
                    mediatorType = mediatorType,
                    viewType = viewField.FieldType,
                    viewField = viewField,
                    argumentField = ReflectionUtils.GetCustomAttributeField<ViewArgument>(mediatorType),
                    openMethod = ReflectionUtils.GetCustomAttributeMethod<ViewOpened>(mediatorType),
                    closeMethod = ReflectionUtils.GetCustomAttributeMethod<ViewClosed>(mediatorType)
                });
        }

        public void Open(object viewKey, Argument argument = null)
        {
            if (!_items.ContainsKey(viewKey)) throw new Exception(string.Format("{0} has not been mapped", viewKey));
            if (_opened.ContainsKey(viewKey)) throw new Exception(string.Format("{0} is already opened", viewKey));
 
            MediatorItem item = _items[viewKey];

            var mediator = Activator.CreateInstance(item.mediatorType);
            object view = _viewFactory.GetView(item.viewType);
            _opened.Add(viewKey, new ViewItem { mediator = mediator, view = view });

            item.viewField.SetValue(mediator, view);

            if (_injector != null)
            {
                _injector.Inject(mediator, InjectionMode.ResolveOnly);
            }
            
            if (item.argumentField != null)
            {
                item.argumentField.SetValue(mediator, argument);
            }

            if (item.openMethod != null)
            {
                item.openMethod.Invoke(mediator, null);
            }
        }

        public void Close(object viewKey)
        {
            if (!_items.ContainsKey(viewKey)) throw new Exception(string.Format("{0} has not been mapped", viewKey));
            if (!_opened.ContainsKey(viewKey)) throw new Exception(string.Format("{0} is not opened", viewKey));

            ViewItem item = _opened[viewKey];
            MediatorItem mediatorItem = _items[viewKey];
            if (mediatorItem.closeMethod != null)
            {
                mediatorItem.closeMethod.Invoke(item.mediator, null);
            }

            _viewFactory.ReleaseView(item.view);
            _opened.Remove(viewKey);
        }

        public void CloseAll()
        {
            var enumerator = _opened.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ViewItem item = enumerator.Current.Value;
                MediatorItem mediatorItem = _items[enumerator.Current.Key];
                if (mediatorItem.closeMethod != null)
                {
                    mediatorItem.closeMethod.Invoke(item.mediator, null);
                }

                _viewFactory.ReleaseView(item.view);
            }

            _opened.Clear();
        }
    }

    public class ViewElement : Attribute { }
    public class ViewOpened : Attribute { }
    public class ViewClosed : Attribute { }
    public class ViewArgument : Attribute { }
}