using Gonity.Core;
using Gonity.Profiler;
using System;
using System.Collections.Generic;

namespace Gonity
{
    internal sealed class EntityDatabase : IEntityDatabase, IEntityDatabaseCallback, IEntityDatabaseProfiled
    {
        private List<Entity> _entities;
        private Dictionary<Type, IComponentList> _componentMap;
        private Dictionary<Enum, List<Entity>> _tagMap;

        public EntityDatabase()
        {
            _entities = new List<Entity>();
            _componentMap = new Dictionary<Type, IComponentList>();
            _tagMap = new Dictionary<Enum, List<Entity>>();
        }

        public Entity CreateEntity()
        {
            Entity entity = new Entity(this);
            _entities.Add(entity);
            return entity;
        }

        public void DestroyEntity(Entity entity)
        {
            if (_entities.Find(e => e == entity) == null) throw new Exception("Could not find entity to remove in Database");

            (entity as IEntityCallback).Destroy();
            entity.RemoveAllComponents();
            entity.RemoveAllTags();

            _entities.Remove(entity);
        }

        public void DestroyAllEntities()
        {
            while (_entities.Count > 0)
            {
                DestroyEntity(_entities[0]);
            }
        }

        public ListReadOnly<T> QueryTypes<T>() where T : ECSComponent
        {
            Type componentType = typeof(T);
            if (_componentMap.ContainsKey(componentType))
            {
                return new ListReadOnly<T>((_componentMap[componentType] as ComponentList<T>).GetList());
            }

            return new ListReadOnly<T>();
        }

        public Entity QueryEntity<T>() where T : ECSComponent
        {
            Type componentType = typeof(T);
            if (_componentMap.ContainsKey(componentType))
            {
                return (_componentMap[componentType] as ComponentList<T>).GetList()[0].entity;
            }

            return null;
        }

        public T QueryType<T>() where T : ECSComponent
        {
            Type componentType = typeof(T);
            if (_componentMap.ContainsKey(componentType))
            {
                return (_componentMap[componentType] as ComponentList<T>).GetList()[0];
            }

            return null;
        }

        public ListReadOnly<Entity> QueryEntities(Enum tag)
        {
            if (_tagMap.ContainsKey(tag))
            {
                return new ListReadOnly<Entity>(_tagMap[tag]);
            }

            return new ListReadOnly<Entity>();
        }

        public Entity QueryEntity(Enum tag)
        {
            if (_tagMap.ContainsKey(tag))
            {
                return _tagMap[tag][0];
            }

            return null;
        }

        void IEntityDatabaseCallback.OnComponentAdded<T>(T component, Type componentType)
        {
            if (!_componentMap.ContainsKey(componentType))
            {
                _componentMap.Add(componentType, new ComponentList<T>());
            }

            (_componentMap[componentType] as ComponentList<T>).Add(component);
        }

        void IEntityDatabaseCallback.OnComponentRemoved(ECSComponent component, Type componentType)
        {
            IEntityCollection entityCollection = component as IEntityCollection;
            if (entityCollection != null)
            {
                entityCollection.RemoveAll();
            }

            (_componentMap[componentType] as ComponentList).Remove(component);

            if (_componentMap[componentType].Count == 0)
            {
                _componentMap.Remove(componentType);
            }
        }

        void IEntityDatabaseCallback.OnTagAdded(Entity entity, Enum tag)
        {
            if (!_tagMap.ContainsKey(tag))
            {
                _tagMap.Add(tag, new List<Entity>());
            }

            _tagMap[tag].Add(entity);
        }

        void IEntityDatabaseCallback.OnTagRemoved(Entity entity, Enum tag)
        {
            _tagMap[tag].Remove(entity);

            if (_tagMap[tag].Count == 0)
            {
                _tagMap.Remove(tag);
            }
        }

        private abstract class ComponentList : IComponentList
        {
            public abstract void Remove(ECSComponent component);
            public abstract int Count { get; }
        }

        private class ComponentList<T> : ComponentList where T : ECSComponent
        {
            private List<T> _list = new List<T>();

            public override int Count { get { return _list.Count;  } }

            public void Add(T component)
            {
                _list.Add(component);
            }

            public List<T> GetList()
            {
                return _list;
            }

            public override void Remove(ECSComponent component)
            {
                _list.Remove(component as T);
            }
        }

        // For profiling purpose only
        int IEntityDatabaseProfiled.numEntities { get { return _entities.Count;  } }
        Dictionary<Type, IComponentList> IEntityDatabaseProfiled.componentList { get { return _componentMap; } }
        List<Entity> IEntityDatabaseProfiled.entities { get { return _entities; } }
    }
}
