using System;
using ECS.Component;
using UnityEngine;

namespace World.Component
{
    /// <summary>
    /// Границы космоса
    /// </summary>
    [Serializable]
    public class SpaceBorderComponent : IECSComponent
    {
        public SpaceBorderAnchor Anchor;
        public BoxCollider2D Collider;
    }

    public enum SpaceBorderBehavior
    {
        portalToOtherSide,
        rebound,
        destroy,
    }

    public enum SpaceBorderAnchor
    {
        cameraLeft = 0,
        cameraRight = 1,
        cameraUp = 2,
        cameraDown = 3,
    }
}