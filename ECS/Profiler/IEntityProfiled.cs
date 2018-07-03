using System;
using System.Collections.Generic;

namespace Gonity.Profiler
{
    public interface IEntityProfiled
    {
        Dictionary<Type, ECSComponent> components { get; }
        List<Enum> tags { get; }
    }
}