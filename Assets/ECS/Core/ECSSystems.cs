using System;
using System.Collections.Generic;
using System.Linq;
using ECS.System;
using JetBrains.Annotations;
using UnityEngine;

namespace ECS.Core
{
    public class ECSSystems
    {
        private readonly Dictionary<Type, SystemWrapper> systems = new Dictionary<Type, SystemWrapper>();
        private readonly Dictionary<Type, SystemWrapper> updateSystems = new Dictionary<Type, SystemWrapper>();
        private readonly Dictionary<Type, SystemWrapper> fixedUpdateSystems = new Dictionary<Type, SystemWrapper>();
        
        internal ECSWorld World { get; }
        
        public double Count => systems.Count;

        public ECSSystems(ECSWorld world)
        {
            World = world;
        }

        internal void OnDestroy()
        {
            var types = systems.Keys.ToList();
            foreach (var type in types)
                RemoveSystem(type);
        }

        internal void Update()
        {
            foreach (var wrapper in updateSystems.Values)
                wrapper.Update();
        }

        internal void FixedUpdate()
        {
            foreach (var wrapper in fixedUpdateSystems.Values)
                wrapper.FixedUpdate();
        }


        public T GetSystem<T>() where T: IECSSystem, new()
        {
            var type = typeof(T);
            if (systems.ContainsKey(type))
                return (T)systems[type].System;
            
            var system = new T();
            var wrapper = new SystemWrapper(system, World);
            
            systems[type] = wrapper;

            if (wrapper.System is IECSUpdateSystem)
                updateSystems[type] = wrapper;

            if (wrapper.System is IECSFixedUpdateSystem)
                fixedUpdateSystems[type] = wrapper;

            system.OnCreate();
            
#if DEBUG
            Debug.Log($"[WorldSystems] Add system: {typeof(T).Name}() success");
#endif
            
            return system;
        }

        [CanBeNull]
        public T TryGetSystem<T>() where T: IECSSystem
        {
            var type = typeof(T);
            if (systems.ContainsKey(type))
                return (T)systems[type].System;
            return default;
        }

        public void RemoveSystem<T>() where T : IECSSystem => RemoveSystem(typeof(T));

        public void RemoveSystem([NotNull]Type type)
        {
            if (systems.TryGetValue(type, out var wrapper))
            {
                systems.Remove(type);
                updateSystems.Remove(type);
                fixedUpdateSystems.Remove(type);
                
                wrapper.OnDestroy();
            }
            
#if DEBUG
            Debug.Log($"[WorldSystems] Remove system: {type.Name}() success");
#endif
        }

        internal IEnumerable<SystemWrapper> GetSystemWrappers() => systems.Values;
    }
}