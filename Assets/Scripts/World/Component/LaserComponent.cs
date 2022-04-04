using ECS.Component;
using UnityEngine;

namespace World.Component
{
    /// <summary>
    /// Лазер, выстреливающий лучем, попадающий в астероиды и НЛО
    /// </summary>
    public class LaserComponent : IECSComponent
    {
        public Vector2 Direction;
    }
}