using Gonity.Core;
using System;
using System.Collections.Generic;

namespace Gonity.Profiler
{
    public interface IEntityDatabaseProfiled
    {
        int numEntities { get; }
        Dictionary<Type, IComponentList> componentList { get; }
        List<Entity> entities { get; }
    }
}
