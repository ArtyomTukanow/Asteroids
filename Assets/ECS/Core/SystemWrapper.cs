using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using ECS.System;

namespace ECS.Core
{
    internal class SystemWrapper
    {
        public IECSSystem System { get; }

        private FieldInfo[] filterFields;
        private ECSWorld world;

        public SystemWrapper(IECSSystem system, ECSWorld world)
        {
            System = system;
            this.world = world;
            RegisterFields();
            AddExistEntitiesToFilter();
        }

        internal void Update()
        {
            (System as IECSUpdateSystem).Update();
        }

        internal void FixedUpdate()
        {
            (System as IECSFixedUpdateSystem).FixedUpdate();
        }

        public void OnDestroy()
        {
            System.OnDestroy();
            
            foreach (var field in filterFields)
                field.SetValue(System, null);
        }
        
        private void RegisterFields()
        {
            if(filterFields != null)
                throw new Exception("Once registration only");

            var filterType = typeof(ECSFilter);
            filterFields = System.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(f => f.FieldType.IsSubclassOf(filterType))
                .ToArray();
        }

        private void AddExistEntitiesToFilter()
        {
            foreach (var entity in world.Entities.GetEntities())
                TryAddEntityToFilter(entity);
        }

        internal void TryAddEntityToFilter(ECSEntity entity)
        {
            foreach (var field in filterFields)
            {
                ECSFilter filter;
                if (field.GetValue(System) == null)
                {
                    filter = (ECSFilter) Activator.CreateInstance (field.FieldType, 
                        BindingFlags.NonPublic | BindingFlags.Instance, 
                        null, null, CultureInfo.InvariantCulture);
                    field.SetValue(System, filter);
                }
                else
                {
                    filter = (ECSFilter) field.GetValue(System);
                }

                if (filter.IsMatch(entity))
                    filter.AddEntityOnce(entity);
            }
        }

        internal void RemoveEntityFromFields(ECSEntity entity)
        {
            foreach (var field in filterFields)
                if (field.GetValue(System) is ECSFilter filter)
                    filter.RemoveEntity(entity);
        }
    }
}