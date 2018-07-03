using System;
using System.Collections.Generic;

namespace Gonity
{
    public abstract class ECSComponentList : ECSComponent, IEntityCollection
    {
        private List<Entity> _entityReferences = new List<Entity>();

        private DelegateHandler<Entity> _onAdd = new DelegateHandler<Entity>();
        private DelegateHandler<Entity> _onRemove = new DelegateHandler<Entity>();

        public IDelegateHandler<Entity> onAdd { get { return _onAdd; } }
        public IDelegateHandler<Entity> onRemove { get { return _onRemove; } }

        public int Count
        {
            get
            {
                return _entityReferences.Count;
            }
        }

        public Entity this[int key]
        {
            get
            {
                return _entityReferences[key];
            }
        }

        public List<Entity> Clone()
        {
            return new List<Entity>(_entityReferences);
        }

        public List<Entity>.Enumerator GetEnumerator()
        {
            return _entityReferences.GetEnumerator();
        }

        public void Add(Entity entity)
        {
            entity.onDestroy.Add(OnEntityDestroyed);

            _entityReferences.Add(entity);

            _onAdd.Invoke(entity);
        }

        public void Insert(int index, Entity entity)
        {
            entity.onDestroy.Add(OnEntityDestroyed);

            _entityReferences.Insert(index, entity);

            _onAdd.Invoke(entity);
        }

        public void Remove(Entity entity)
        {
            if (!HasEntity(entity))
            {
                throw new Exception(string.Format("ECSComponent.RemoveReferenceEntity() : {0} is not a reference", entity));
            }

            entity.onDestroy.Remove(OnEntityDestroyed);

            _onRemove.Invoke(entity);

            _entityReferences.Remove(entity);
        }

        public Entity Find(Predicate<Entity> match)
        {
            return _entityReferences.Find(match);
        }

        public void RemoveAt(int index)
        {
            Remove(_entityReferences[index]);
        }

        public void RemoveAll()
        {
            if (_entityReferences.Count == 0) return;

            var enumerator = _entityReferences.GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumerator.Current.onDestroy.Remove(OnEntityDestroyed);

                _onRemove.Invoke(enumerator.Current);
            }

            _entityReferences.Clear();
        }

        public bool HasEntity(Entity entity)
        {
            return _entityReferences.Find(x => x == entity) != null;
        }

        private void OnEntityDestroyed(Entity entity)
        {
            entity.onDestroy.Remove(OnEntityDestroyed);

            var enumerator = _entityReferences.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current != entity) continue;

                _onRemove.Invoke(enumerator.Current);

                _entityReferences.Remove(enumerator.Current);
                
                break;
            }
        }
    }
}
