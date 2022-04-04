using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ECS.System;

namespace World.System
{
    /// <summary>
    /// Выдает список систем в пространстве имен World.System
    /// </summary>
    public static class AllSystems
    {
        public static IEnumerable<Type> GetAll()
        {
            var nameSpace = typeof(AllSystems).Namespace;
            return typeof(AllSystems)
                .GetTypeInfo().Assembly
                .GetTypes()
                .Where(t => typeof(IECSSystem).IsAssignableFrom(t) && string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal));
        }
    }
}