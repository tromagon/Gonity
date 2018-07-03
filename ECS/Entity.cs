using System;
using System.Collections.Generic;
using Gonity.Core;
using Gonity.Profiler;

namespace Gonity
{
    public sealed class Entity : IEntityProfiled, IEntityCallback
    {
        private Dictionary<Type, ECSComponent> _components;
        private List<Enum> _tags;
        private DelegateHandler<Entity> _destroyHandler;
        private IEntityDatabaseCallback _databaseCallback;

        public IDelegateHandler<Entity> onDestroy { get { return _destroyHandler; } }

        public Entity(IEntityDatabaseCallback databaseCallback)
        {
            _components = new Dictionary<Type, ECSComponent>();
            _tags = new List<Enum>();
            _destroyHandler = new DelegateHandler<Entity>();
            _databaseCallback = databaseCallback;
        }

        public T AddComponent<T>() where T : ECSComponent, new()
        {
            Type componentType = typeof(T);
            if (_components.ContainsKey(componentType)) throw new Exception(string.Format("There is already a component of type {0} on this entity", componentType));

            T component = new T();
            (component as IComponent).SetEntity(this);
            _components.Add(componentType, component);

            _databaseCallback.OnComponentAdded(component, componentType);

            return component;
        }

        public void RemoveComponent<T>() where T : ECSComponent
        {
            Type componentType = ValidateComponentType<T>();
            _databaseCallback.OnComponentRemoved(_components[componentType], componentType);

            _components.Remove(componentType);
        }

        public void RemoveAllComponents()
        {
            var enumerator = _components.GetEnumerator();
            while (enumerator.MoveNext())
            {
                _databaseCallback.OnComponentRemoved(enumerator.Current.Value, enumerator.Current.Key);
            }

            _components.Clear();
        }

        public void AddTag(Enum tag)
        {
            if (HasTag(tag)) throw new Exception(string.Format("Tag {0} already exists on this entity", tag));
            _tags.Add(tag);
            _databaseCallback.OnTagAdded(this, tag);
        }

        public bool HasTag(Enum tag)
        {
            return _tags.Contains(tag);
        }

        public void RemoveTag(Enum tag)
        {
            if (!_tags.Contains(tag)) throw new Exception(string.Format("Tag {0} not found on this entity", tag));
            _tags.Remove(tag);
            _databaseCallback.OnTagRemoved(this, tag);
        }

        public void RemoveAllTags()
        {
            var enumerator = _tags.GetEnumerator();
            while (enumerator.MoveNext())
            {
                _databaseCallback.OnTagRemoved(this, enumerator.Current);
            }

            _tags.Clear();
        }

        public T GetComponent<T>() where T : ECSComponent
        {
            Type componentType = typeof(T);
            if (!_components.ContainsKey(componentType))
            {
                return null;
            }

            return (T)_components[componentType];
        }

        public bool HasComponent<T>() where T : ECSComponent
        {
            return _components.ContainsKey(typeof(T));
        }

        private Type ValidateComponentType<T>()
        {
            Type type = typeof(T);
            if (!_components.ContainsKey(type)) throw new Exception(string.Format("There is no component of type {0} on this entity", type));
            return type;
        }

        public static implicit operator bool(Entity entity)
        {
            return !ReferenceEquals(entity, null);
        }

        void IEntityCallback.Destroy()
        {
            _destroyHandler.Invoke(this);
        }

        // For Profiling Purpose
        Dictionary<Type, ECSComponent> IEntityProfiled.components { get { return _components; } }
        List<Enum> IEntityProfiled.tags { get { return _tags; } }
    }
}