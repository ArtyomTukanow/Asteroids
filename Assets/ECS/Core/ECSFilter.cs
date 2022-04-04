using System;
using System.Collections.Generic;
using System.Linq;
using ECS.Component;

namespace ECS.Core
{
    public static class ECSFilterUtils
    {
        private static IEnumerable<ECSEntity> emptyEntities = new List<ECSEntity>();
        
        public static bool IsEmpty(this ECSFilter filter) => filter == null || filter.Count == 0;
        public static IEnumerable<ECSEntity> GetEntities(this ECSFilter filter) => filter != null ? filter.entities : emptyEntities;
    }
    
    public abstract class ECSFilter
    {
        protected Type[] ComponentTypes;
        
        internal readonly List<ECSEntity> entities = new List<ECSEntity>();
        
        protected ECSFilter()
        {
        }

        public int Count => entities.Count;
        
        public ECSEntity Get(int index) => entities[index];

        internal void AddEntityOnce(ECSEntity entity)
        {
            if(!entities.Contains(entity))
                entities.Add(entity);
        }

        internal void RemoveEntity(ECSEntity entity) => entities.Remove(entity);
        
        internal bool IsMatch(ECSEntity entity) => ComponentTypes.All(entity.Contains);
    }


    #region IMPLEMENTATION
    
    public class ECSFilter<T> : ECSFilter where T: IECSComponent
    {
        protected ECSFilter() : base()
        {
            ComponentTypes = new[]
            {
                typeof(T)
            };
        }
    }

    public class ECSFilter<T1, T2> : ECSFilter where T1 : IECSComponent where T2: IECSComponent
    {
        protected ECSFilter() : base()
        {
            ComponentTypes = new[]
            {
                typeof(T1),
                typeof(T2),
            };
        }
    }

    public class ECSFilter<T1, T2, T3> : ECSFilter where T1 : IECSComponent where T2: IECSComponent where T3: IECSComponent
    {
        protected ECSFilter() : base()
        {
            ComponentTypes = new[]
            {
                typeof(T1),
                typeof(T2),
                typeof(T3),
            };
        }
    }

    #endregion

}