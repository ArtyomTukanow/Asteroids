using System;
using System.Linq;
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
    /// Проверяет столкновение объектов
    /// </summary>
    public class SpaceCollisionSystem : IECSSystem, IECSFixedUpdateSystem
    {
        private ECSFilter<SpaceObjectComponent> spaceObjects;
        private ECSFilter<SpaceBorderComponent> borders;
        private ECSFilter<BulletComponent> bullets;
        private ECSFilter<PlayerComponent> players;
        private ECSFilter<LaserComponent> lasers;
        
        public void OnCreate()
        {
            
        }

        public void OnDestroy()
        {
            
        }

        public void FixedUpdate()
        {
            CheckDestroy();
        }

        public void CheckDestroy()
        {
            CheckByBorders();
            CheckByBullets();
            CheckByLasers();
            CheckByPlayer();
        }

        private void CheckByPlayer()
        {
            if(players.IsEmpty())
                return;

            foreach (var playerEntity in players.GetEntities())
            {
                var playerCollider = playerEntity.Get<SpaceObjectComponent>().Collider;
                foreach (var spaceEntity in spaceObjects.GetEntities())
                {
                    if(IsPlayerWalkThrow(spaceEntity))
                        continue;

                    if (spaceEntity.Get<SpaceObjectComponent>().Collider.IsTouching(playerCollider))
                    {
                        playerEntity.Get<DestroySEvent>();
                        spaceEntity.Get<DestroySEvent>();
                    }
                }
            }
        }

        private void CheckByBullets()
        {
            if(bullets.IsEmpty())
                return;

            foreach (var bulletEntity in bullets.GetEntities())
            {
                var bulletCollider = bulletEntity.Get<SpaceObjectComponent>().Collider;
                foreach (var spaceEntity in spaceObjects.GetEntities())
                {
                    if(IsShootThrow(spaceEntity))
                        continue;

                    if (spaceEntity.Get<SpaceObjectComponent>().Collider.IsTouching(bulletCollider))
                    {
                        bulletEntity.Get<DestroySEvent>();
                        spaceEntity.Get<DestroySEvent>().Score += Settings.BULLET_SCORE;
                    }
                }
            }
        }

        private void CheckByLasers()
        {
            if(lasers.IsEmpty())
                return;

            foreach (var laserEntity in lasers.GetEntities())
            {
                var laserDirection = laserEntity.Get<LaserComponent>().Direction;
                var laserGameObject = laserEntity.Get<SpaceObjectComponent>().GameObject;
                
                var hits = Physics2D.RaycastAll(laserGameObject.transform.position, laserDirection);
                
                foreach (var spaceEntity in spaceObjects.GetEntities())
                {
                    if(IsShootThrow(spaceEntity))
                        continue;

                    var spaceObjCollider = spaceEntity.Get<SpaceObjectComponent>().Collider;
                    if (hits.Any(h => h.collider == spaceObjCollider))
                        spaceEntity.Get<DestroySEvent>().Score += Settings.BULLET_SCORE;
                }
                
                laserEntity.Get<DestroySEvent>();
            }
        }
        
        private void CheckByBorders()
        {
            foreach (var spaceEntity in spaceObjects.GetEntities())
            {
                var spaceObject = spaceEntity.Get<SpaceObjectComponent>();
                foreach (var borderEntity in borders.GetEntities())
                {
                    var border = borderEntity.Get<SpaceBorderComponent>();

                    var spaceObjPos = spaceObject.Collider.gameObject.transform.position;
                    if (border.Collider.bounds.Contains(new Vector3(spaceObjPos.x, spaceObjPos.y, border.Collider.transform.position.z)))
                        InteractWithBorder(border, spaceObject, spaceEntity);
                }
            }
        }

        private void InteractWithBorder(SpaceBorderComponent border, SpaceObjectComponent spaceObject, ECSEntity entity)
        {
            switch (spaceObject.Behavior)
            {
                case SpaceBorderBehavior.portalToOtherSide:
                    PortalByCollision(border, spaceObject);
                    break;
                case SpaceBorderBehavior.rebound:
                    ReboundByCollision(border, spaceObject);
                    break;
                case SpaceBorderBehavior.destroy:
                    entity.Get<DestroySEvent>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown border behavior: {spaceObject.Behavior}");
            }
        }

        private void PortalByCollision(SpaceBorderComponent border, SpaceObjectComponent spaceObject)
        {
            var bounds = Game.MainCamera.GetWorldBounds();

            Vector3 portalDirection = border.Anchor switch
            {
                SpaceBorderAnchor.cameraLeft => new Vector3(bounds.size.x, 0, 0),
                SpaceBorderAnchor.cameraRight => new Vector3(-bounds.size.x, 0, 0),
                SpaceBorderAnchor.cameraUp => new Vector3(0, -bounds.size.y, 0),
                SpaceBorderAnchor.cameraDown => new Vector3(0, bounds.size.y, 0),
                _ => default
            };

            spaceObject.GameObject.transform.localPosition += portalDirection;
        }

        private void ReboundByCollision(SpaceBorderComponent border, SpaceObjectComponent spaceObject)
        {
            spaceObject.Speed = border.Anchor switch
            {
                SpaceBorderAnchor.cameraLeft => new Vector3(-spaceObject.Speed.x, spaceObject.Speed.y),
                SpaceBorderAnchor.cameraRight => new Vector3(-spaceObject.Speed.x, spaceObject.Speed.y),
                SpaceBorderAnchor.cameraUp => new Vector3(spaceObject.Speed.x, -spaceObject.Speed.y),
                SpaceBorderAnchor.cameraDown => new Vector3(spaceObject.Speed.x, -spaceObject.Speed.y),
                _ => spaceObject.Speed
            };
        }
        
        private bool IsShootThrow(ECSEntity spaceEntity)
        {
            return spaceEntity.Contains<LaserComponent>()
                   || spaceEntity.Contains<BulletComponent>()
                   || spaceEntity.Contains<PlayerComponent>();
        }

        private bool IsPlayerWalkThrow(ECSEntity spaceEntity)
        {
            return spaceEntity.Contains<LaserComponent>()
                   || spaceEntity.Contains<BulletComponent>()
                   || spaceEntity.Contains<PlayerComponent>();
        }
    }
}