using Gonity.Core;

namespace Gonity
{
    public abstract class ECSComponent : IComponent
    {
        private Entity _entity;

        public Entity entity { get { return _entity; } }

        public static implicit operator bool(ECSComponent component)
        {
            return !ReferenceEquals(component, null);
        }

        void IComponent.SetEntity(Entity entity)
        {
            _entity = entity;
        }
    }
}