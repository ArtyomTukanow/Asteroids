using ECS.Core;
using ECS.System;
using UnityEngine;
using Utils;
using World.Component;

namespace World.System
{
    /// <summary>
    /// Передвигает объекты в пространстве
    /// </summary>
    public class SpaceObjectsMovementSystem : IECSSystem, IECSFixedUpdateSystem
    {
        private ECSFilter<SpaceObjectComponent> objectsFilter = null;
        
        public void FixedUpdate()
        {
            foreach (var entity in objectsFilter.GetEntities())
                MoveObject(entity.Get<SpaceObjectComponent>());
        }
        
        public void OnCreate()
        {
            
        }

        public void OnDestroy()
        {
            
        }

        private void MoveObject(SpaceObjectComponent spaceObject)
        {
            var newRotate = spaceObject.GameObject.transform.rotation.eulerAngles.Add(z: spaceObject.Angle);
            spaceObject.GameObject.transform.rotation = Quaternion.Euler(newRotate);
            spaceObject.GameObject.transform.localPosition += spaceObject.Speed;

            if (spaceObject.Braking != 0)
            {
                var newX = GetSpeedByBreaking(spaceObject.Speed.x, spaceObject.Braking);
                var newY = GetSpeedByBreaking(spaceObject.Speed.y, spaceObject.Braking);
                spaceObject.Speed = new Vector2(newX, newY);
            }
        }

        private float GetSpeedByBreaking(float speed, float breaking) => 
            Mathf.Abs(speed) > breaking ? Mathf.Sign(speed) * (Mathf.Abs(speed) - breaking) : 0;
    }
}