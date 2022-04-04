using System;
using System.Collections.Generic;
using ECS.Component;
using ECS.Exceptions;

namespace ECS.Core
{
    public class ECSEntities
    {
        private readonly List<(ECSEntity entity, Type type)> deferredComponentsToRemove = new List<(ECSEntity, Type)>();
        private readonly List<ECSEntity> entities = new List<ECSEntity>();
        
        public ECSWorld World { get; }

        internal ECSEntities(ECSWorld world)
        {
            World = world;
        }
        

        public void Update()
        {
            RemoveDeferredComponents();
        }

        public void FixedUpdate()
        {
            RemoveDeferredComponents();
        }

        internal void OnDestroy()
        {
            var wasCount = entities.Count + 1;
            while (entities.Count > 0)
            {
                if(wasCount == entities.Count)
                    throw new Exception("Double remove ECSEntity!");
                wasCount = entities.Count;
                entities.RemoveAt(entities.Count - 1);
            }
        }
        
        

        public IEnumerable<ECSEntity> GetEntities() => entities;

        public ECSEntity AddEntity(params Type[] components)
        {
            if(World.Destroyed)
                throw new DestroyException(typeof(ECSEntities)); 
            
            if(components.Length == 0)
                throw new TypeException(typeof(IECSComponent), null);
            
            var entity = new ECSEntity(this);
            entities.Add(entity);

            foreach (var compType in components)
            {
                if(entity.Contains(compType))
                    continue;
                
                if (!typeof(IECSComponent).IsAssignableFrom(compType))
                    throw new TypeException(typeof(IECSComponent), compType);
                
                entity.Get(compType);
            }

            return entity;
        }

        internal void OnAddComponent(ECSEntity entity)
        {
            foreach (var system in World.Systems.GetSystemWrappers())
                system.TryAddEntityToFilter(entity);
        }

        internal void OnRemoveComponent(ECSEntity entity)
        {
            foreach (var system in World.Systems.GetSystemWrappers())
                system.RemoveEntityFromFields(entity);

            if (entity.Count > 0)
            {
                foreach (var system in World.Systems.GetSystemWrappers())
                    system.TryAddEntityToFilter(entity);
            }
            else
            {
                entities.Remove(entity);
                entity.Destroy();
            }
        }

        internal void AddToDeferredRemove(ECSEntity entity, Type type)
        {
            deferredComponentsToRemove.Add((entity, type));
        }

        private void RemoveDeferredComponents()
        {
            if (deferredComponentsToRemove.Count > 0)
            {
                foreach (var pair in deferredComponentsToRemove)
                    pair.entity.Remove(pair.type);
                deferredComponentsToRemove.Clear();
            }
        }
    }
}