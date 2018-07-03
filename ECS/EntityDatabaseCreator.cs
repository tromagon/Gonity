using Gonity.Core;
using Gonity.Profiler;
using UnityEngine;

namespace Gonity
{
    public static class EntityDatabaseCreator
    {
        public static IEntityDatabase CreateDatabase()
        {
            EntityDatabase entityDatabase = new EntityDatabase();
            ActivateProfile(entityDatabase);
            return entityDatabase;
        }

        public static T CreateDatabase<T>() where T : IEntityDatabase, IEntityDatabaseProfiled, new()
        {
            T entityDatabase = new T();
            ActivateProfile(entityDatabase);
            return entityDatabase;
        }

        private static void ActivateProfile(IEntityDatabaseProfiled entityDatabase)
        {
#if ECS_PROFILER_ENABLED && UNITY_EDITOR
            GameObject debugObject = new GameObject("ECSProfiler");
            ECSProfiler profiler = debugObject.AddComponent<ECSProfiler>();
            profiler.entityDatabase = entityDatabase;
            GameObject.DontDestroyOnLoad(debugObject);
#endif
        }
    }
}
