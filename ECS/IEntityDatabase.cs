using System;

namespace Gonity
{
    public interface IEntityDatabase
    {
        Entity CreateEntity();
        void DestroyEntity(Entity entity);
        void DestroyAllEntities();
        T QueryType<T>() where T : ECSComponent;
        ListReadOnly<T> QueryTypes<T>() where T : ECSComponent;
        Entity QueryEntity<T>() where T : ECSComponent;
        Entity QueryEntity(Enum tag);
        ListReadOnly<Entity> QueryEntities(Enum tag);
    }
}