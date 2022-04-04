using System.Collections.Generic;
using Core;
using ECS.Core;
using ECS.System;
using UnityEngine;
using Utils;
using World.Component;
using World.Components;
using World.Events;

namespace World.System
{
    /// <summary>
    /// Перехватывает объекты с DestroySEvent и удаляет их с поля
    /// </summary>
    public class DestroySystem : IECSSystem, IECSFixedUpdateSystem
    {
        private ECSFilter<SpaceObjectComponent, DestroySEvent> objectsToDestroy;
        
        public void OnCreate()
        {
            
        }

        public void OnDestroy()
        {
            
        }

        public void FixedUpdate()
        {
            if(objectsToDestroy.IsEmpty())
                return;
            
            var entities = new List<ECSEntity>(objectsToDestroy.GetEntities());
            foreach (var entity in entities)
            {
                AddScore(entity);
                
                if (entity.Contains<LaserComponent>())
                    DestroyLaser(entity);
                if (entity.Contains<PlayerComponent>())
                    DestroyPlayer(entity);
                else if (entity.Contains<AsteroidComponent>())
                    DestroyAsteroid(entity);
                else if (entity.Contains<SpaceObjectComponent>())
                    DestroySpaceObject(entity);
            }
        }

        private void AddScore(ECSEntity entity)
        {
            Game.User.Player.Score += entity.Get<DestroySEvent>().Score;
        }

        private void DestroySpaceObject(ECSEntity entity)
        {
            Object.Destroy(entity.Get<SpaceObjectComponent>().GameObject);
            entity.Destroy();
        }

        private void DestroyAsteroid(ECSEntity entity)
        {
            var nextLevel = entity.Get<AsteroidComponent>().Level - 1;
            var wasPosition = entity.Get<SpaceObjectComponent>().GameObject.transform.position;
            
            if (nextLevel != AsteroidLevel.none)
            {
                for (var i = 0; i < Settings.ASTEROID_SPLIT_COUNT; i++)
                {
                    var newAsteroid = Game.Main.WorldFactory.CreateAsteroid(nextLevel);
                    newAsteroid.Get<SpaceObjectComponent>().GameObject.transform.position = wasPosition;
                }
            }
            
            Object.Destroy(entity.Get<SpaceObjectComponent>().GameObject);
            entity.Destroy();
        }

        private void DestroyPlayer(ECSEntity entity)
        {
            if (entity.Contains<SpaceObjectComponent>())
                DestroySpaceObject(entity);
        }

        private void DestroyLaser(ECSEntity entity)
        {
            var go = entity.Get<SpaceObjectComponent>().GameObject;
            Game.Timer.DelayedCall(0.2f, () =>
            {
                if (go)
                    Object.Destroy(go);
            });
            entity.Destroy();
        }
    }
}