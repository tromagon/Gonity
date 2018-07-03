using System;

namespace Gonity.Core
{
    public interface IEntityDatabaseCallback
    {
        void OnComponentAdded<T>(T component, Type componentType) where T : ECSComponent;
        void OnComponentRemoved(ECSComponent component, Type componentType);
        void OnTagAdded(Entity entity, Enum tag);
        void OnTagRemoved(Entity entity, Enum tag);
    }
}