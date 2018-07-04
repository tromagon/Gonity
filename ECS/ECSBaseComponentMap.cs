using System;
using System.Collections.Generic;

namespace Gonity
{
    public abstract class ECSBaseComponentMap<T> : ECSComponent, IEntityCollection
    {
        private Dictionary<T, Entity> _entityReferences = new Dictionary<T, Entity>();

        private Dictionary<T, DelegateHandler<Entity>> _onAddMap = new Dictionary<T, DelegateHandler<Entity>>();
        private Dictionary<T, DelegateHandler<Entity>> _onRemoveMap = new Dictionary<T, DelegateHandler<Entity>>();

        public IDelegateHandler<Entity> onAdd(T key)
        {
            if (!_onAddMap.ContainsKey(key))
            {
                _onAddMap[key] = new DelegateHandler<Entity>();
            }

            return _onAddMap[key];
        }

        public IDelegateHandler<Entity> onRemove(T key)
        {
            if (!_onRemoveMap.ContainsKey(key))
            {
                _onRemoveMap[key] = new DelegateHandler<Entity>();
            }

            return _onRemoveMap[key];
        }

        public bool ContainsKey(T key)
        {
            return _entityReferences.ContainsKey(key);
        }

        public Entity this[T key]
        {
            get
            {
                return _entityReferences[key];
            }

            set
            {
                _entityReferences[key] = value;

                if (_onAddMap.ContainsKey(key))
                {
                    _onAddMap[key].Invoke(value);
                }
            }
        }

        public void Add(T key, Entity entity)
        {
            if (_entityReferences.ContainsKey(key))
            {
                if (_entityReferences[key] == entity)
                {
                    throw new Exception(string.Format("ECSComponent.AddReferenceEntity() : {0} already exists", key));
                }

                _entityReferences[key].onDestroy.Remove(OnEntityDestroyed);
                _entityReferences[key] = entity;
            }
            else
            {
                _entityReferences.Add(key, entity);
            }

            _entityReferences[key].onDestroy.Add(OnEntityDestroyed);

            if (_onAddMap.ContainsKey(key))
            {
                _onAddMap[key].Invoke(entity);
            }
        }

        public void Remove(T key)
        {
            if (!_entityReferences.ContainsKey(key))
            {
                throw new Exception(string.Format("ECSComponent.RemoveReferenceEntity() : {0} doesn't exist", key));
            }

            Entity entity = _entityReferences[key];
            entity.onDestroy.Remove(OnEntityDestroyed);

            if (_onRemoveMap.ContainsKey(key))
            {
                _onRemoveMap[key].Invoke(entity);
            }

            _entityReferences.Remove(key);
        }

        public void RemoveAll()
        {
            if (_entityReferences == null) return;

            var enumerator = _entityReferences.GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumerator.Current.Value.onDestroy.Remove(OnEntityDestroyed);

                if (_onRemoveMap.ContainsKey(enumerator.Current.Key))
                {
                    _onRemoveMap[enumerator.Current.Key].Invoke(enumerator.Current.Value);
                }
            }

            _entityReferences.Clear();
        }

        private void OnEntityDestroyed(Entity entity)
        {
            entity.onDestroy.Remove(OnEntityDestroyed);

            var enumerator = _entityReferences.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Value != entity) continue;

                if (_onRemoveMap.ContainsKey(enumerator.Current.Key))
                {
                    _onRemoveMap[enumerator.Current.Key].Invoke(enumerator.Current.Value);
                }

                _entityReferences.Remove(enumerator.Current.Key);

                break;
            }
        }
    }
}
