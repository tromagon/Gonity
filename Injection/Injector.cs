using System;
using System.Reflection;
using System.Collections.Generic;
using Gonity.Core;

namespace Gonity
{
    public class Injector : IInjector, IInjectorCore
    {
        private Dictionary<Type, Injection> _injected;
        private List<Dependency> _unresolved;

        private class Injection
        {
            public object instance;
            public bool persistent;
        }

        private class Dependency
        {
            public object instance;
            public FieldInfo fInfo;
            public Type fieldType;
        }

        public Injector()
        {
            _injected = new Dictionary<Type, Injection>();
            _unresolved = new List<Dependency>();
        }

        public void Inject<T>(T instance, bool persistent = false)
        {
            Type instanceType = typeof(T);
            ResolveDepedencies(instanceType, instance);
            _injected.Add(instanceType, new Injection { instance = instance, persistent = persistent });
        }

        public void Inject(object instance, InjectionMode mode, bool persistent = false)
        {
            Type instanceType = instance.GetType();
            Inject(instanceType, instance, mode, persistent);
        }

        public T GetInjected<T>() where T : class
        {
            Type instanceType = typeof(T);
            object instance = null;

            var enumerator = _injected.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Key == instanceType)
                {
                    instance = enumerator.Current.Value.instance;
                    break;
                }
            }

            return instance as T;
        }

        public void ClearAll(bool persistent = false)
        {
            List<Type> types = new List<Type>();
            var enumerator = _injected.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Value.persistent && !persistent) continue;
                types.Add(enumerator.Current.Key);
            }

            int l = types.Count;
            for (int i = 0; i < l; ++i)
            {
                _injected.Remove(types[i]);
            }
        }

        void IInjectorCore.ClearInject()
        {
            List<Type> types = new List<Type>();
            var enumerator = _injected.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Value.persistent) continue;
                types.Add(enumerator.Current.Key);
            }

            int l = types.Count;
            for (int i = 0; i < l; ++i)
            {
                _injected.Remove(types[i]);
            }

            _unresolved.Clear();
        }

        private void Inject(Type instanceType, object instance, InjectionMode mode, bool persistent = false)
        {
            if (mode == InjectionMode.InjectAndResolve || mode == InjectionMode.ResolveOnly)
            {
                ResolveDepedencies(instanceType, instance);
            }

            if (mode == InjectionMode.InjectAndResolve || mode == InjectionMode.InjectOnly)
            {
                _injected.Add(instanceType, new Injection { instance = instance, persistent = persistent });
            }
        }

        private void ResolveDepedencies(Type type, object instance)
        {
            List<FieldInfo> attributes = ReflectionUtils.GetCustomAttributeFields<Injected>(type);
            int l = attributes.Count;
            for (int i = 0; i < l; i++)
            {
                FieldInfo fInfo = attributes[i];
                if (_injected.ContainsKey(fInfo.FieldType))
                {
                    fInfo.SetValue(instance, _injected[fInfo.FieldType].instance);
                }
                else
                {
                    _unresolved.Add(new Dependency { instance = instance, fInfo = fInfo, fieldType = fInfo.FieldType });
                }
            }

            // Then solves previous unresolved dependencies
            l = _unresolved.Count;
            for (int i = l - 1; i >= 0; --i)
            {
                Dependency dependency = _unresolved[i];
                if (dependency.fieldType == type)
                {
                    dependency.fInfo.SetValue(dependency.instance, instance);
                    _unresolved.RemoveAt(i);
                }
            }
        }
    }

    public class Injected : Attribute { }
}

