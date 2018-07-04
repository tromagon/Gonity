using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gonity
{
    public class ViewFactory : IViewFactory, IViewFactoryRegister
    {
        private class ViewItem
        {
            public UnityEngine.Object view;
            public bool needInstantiation;
        }

        private Dictionary<Type, ViewItem> _views;

        public ViewFactory()
        {
            _views = new Dictionary<Type, ViewItem>();
        }

        public void Register<T>(T prefab, bool needInstantiation = true) where T : UnityEngine.Object
        {
            Type viewType = typeof(T);
            _views.Add(viewType, new ViewItem { view = prefab, needInstantiation = needInstantiation });
        }

        public object GetView(Type viewType)
        {
            if (!_views.ContainsKey(viewType)) throw new Exception(string.Format("ViewFactory.GetView() : View type {0} hasn't been registered", viewType));

            ViewItem viewMap = _views[viewType];
            return viewMap.needInstantiation ? UnityEngine.Object.Instantiate(viewMap.view) : viewMap.view;
        }

        public void ReleaseView(object view)
        {
            GameObject gameObject = (view as MonoBehaviour).gameObject;
            UnityEngine.Object.Destroy(gameObject);
        }

        public void ClearAll()
        {
            _views.Clear();
        }
    }
}