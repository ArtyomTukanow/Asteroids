using System;
using ECS.Component;
using UnityEngine;

namespace World.Component
{
    /// <summary>
    /// Объект в космосе со скоростью и углом вращения.
    /// </summary>
    [Serializable]
    public class SpaceObjectComponent : IECSComponent
    {
        public float Angle;
        public Vector3 Speed;
        public float Braking;

        public Collider2D Collider;
        public GameObject GameObject;
        
        public SpaceBorderBehavior Behavior = SpaceBorderBehavior.portalToOtherSide;

        public Vector2 Direction
        {
            get
            {
                var rotate = GameObject.transform.rotation.eulerAngles.z / 180 * Mathf.PI;
                return new Vector2(Mathf.Cos(rotate), Mathf.Sin(rotate));
            }
        }
    }
}