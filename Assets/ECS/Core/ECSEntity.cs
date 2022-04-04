using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using ECS.Component;
using ECS.Exceptions;

namespace ECS.Core
{
    public class ECSEntity
    {
        private ECSEntities entities;
        
        private Dictionary<Type, IECSComponent> components = new Dictionary<Type, IECSComponent>();
        
        public int Count => components.Count;
        public bool Destroyed { get; private set; }

        internal ECSEntity(ECSEntities entities)
        {
            this.entities = entities;
        }

        public bool Contains<T>() where T : IECSComponent => Contains(typeof(T));
        public bool Contains(Type t) => components.ContainsKey(t);

        public IECSComponent Get(Type compType)
        {
            if (Destroyed)
                throw new DestroyException(typeof(ECSEntity));
            
            if (!components.ContainsKey(compType))
            {
                var component = (IECSComponent)Activator.CreateInstance(compType, 
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null,
                    null, CultureInfo.InvariantCulture);
                components[compType] = component;
                entities.OnAddComponent(this);
            }
            return components[compType];
        }
        
        public T Get<T>() where T: IECSComponent, new ()
        {
            if (Destroyed)
                throw new DestroyException(typeof(ECSEntity));
            
            var compType = typeof(T);
            if (!components.ContainsKey(compType))
            {
                var component = new T();
                components[compType] = component;
                entities.OnAddComponent(this);
            }
            
            return (T)components[compType];
        }

        public void Remove(Type compType)
        {
            if (components.ContainsKey(compType))
            {
                components.Remove(compType);
                entities.OnRemoveComponent(this);
            }
        }

        public void Remove<T>() where T : IECSComponent => Remove(typeof(T));

        public void DeferredRemove<T>() where T : IECSComponent => entities.AddToDeferredRemove(this, typeof(T));

        public ECSEntity Replace(IECSComponent component)
        {
            var compType = component.GetType();
            var wasComponent = components.Remove(compType);

            components[compType] = component;
            
            if(!wasComponent)
                entities.OnAddComponent(this);

            return this;
        }

        public void Destroy()
        {
            if(Destroyed)
                return;
            
            Destroyed = true;
            
            components.Clear();
            entities.OnRemoveComponent(this);
        }
    }
}