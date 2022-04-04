using System;
using ECS.Exceptions;
using ECS.System;
using JetBrains.Annotations;

namespace ECS.Core
{
    public class ECSWorld
    {
        internal bool Destroyed { get; private set; }
        
        public ECSSystems Systems { get; }
        public ECSEntities Entities { get; }

        public ECSWorld()
        {
            Entities = new ECSEntities(this);
            Systems = new ECSSystems(this);
        }

        public void OnDestroy()
        {
            Entities.OnDestroy();
            Systems.OnDestroy();

            Destroyed = true;
        }

        public void Update()
        {
            if(Destroyed)
                throw new DestroyException(typeof(ECSWorld));
            Entities.Update();
            Systems.Update();
        }

        public void FixedUpdate()
        {
            if(Destroyed)
                throw new DestroyException(typeof(ECSWorld));
            Entities.FixedUpdate();
            Systems.FixedUpdate();
        }


        #region CHAINING/WRAPPED_METHODS

        public ECSWorld WithEntity(params Type[] components)
        {
            Entities.AddEntity(components);
            return this;
        }

        public ECSWorld WithSystem<T>() where T: IECSSystem, new()
        {
            Systems.GetSystem<T>();
            return this;
        }

        public ECSWorld WithoutSystem<T>() where T : IECSSystem
        {
            Systems.RemoveSystem<T>();
            return this;
        }

        public ECSEntity AddEntity(params Type[] components) => Entities.AddEntity(components);
        
        public T GetSystem<T>() where T: IECSSystem, new() => Systems.GetSystem<T>();
        [CanBeNull]
        public T TryGetSystem<T>() where T: IECSSystem => Systems.TryGetSystem<T>();
        
        public void RemoveSystem<T>() where T: IECSSystem => Systems.RemoveSystem<T>();

        #endregion
    }
}