using Core;
using ECS.Core;
using ECS.System;
using UnityEngine;
using Utils;
using World.Component;
using World.Events;

namespace World.System
{
    /// <summary>
    /// Располагает границы мира относительно камеры
    /// </summary>
    public class SpaceBordersPlacerSystem : IECSSystem, IECSFixedUpdateSystem
    {
        private readonly ECSFilter<SpaceBorderComponent, CameraSEvent> bordersByCameraEventFilter = null;


        public void OnCreate()
        {
            
        }

        public void OnDestroy()
        {
            
        }

        public void FixedUpdate()
        {
            if(!bordersByCameraEventFilter.IsEmpty())
                CheckBordersPositions();
        }
        
        private void CheckBordersPositions()
        {
            Bounds bounds = Game.MainCamera.GetWorldBounds();
            foreach (var border in bordersByCameraEventFilter.GetEntities())
            {
                UpdateBorderPosition(border.Get<SpaceBorderComponent>(), bounds);
                border.DeferredRemove<CameraSEvent>();
            }
        }

        private void UpdateBorderPosition(SpaceBorderComponent borderView, Bounds bounds)
        {
            borderView.Collider.size = new Vector2(100, 100);
            
            switch (borderView.Anchor)
            {
                case SpaceBorderAnchor.cameraDown:
                    borderView.Collider.transform.localPosition = bounds.max.Set(y: - bounds.size.y / 2);
                    borderView.Collider.offset = bounds.max + new Vector3(0, - borderView.Collider.size.y / 2);
                    break;
                case SpaceBorderAnchor.cameraUp:
                    borderView.Collider.transform.localPosition = bounds.max.Set(y: bounds.size.y / 2);
                    borderView.Collider.offset = bounds.max + new Vector3(0, borderView.Collider.size.y / 2);
                    break;
                case SpaceBorderAnchor.cameraLeft:
                    borderView.Collider.transform.localPosition = bounds.max.Set( x: - bounds.size.x / 2);
                    borderView.Collider.offset = bounds.max + new Vector3(- borderView.Collider.size.x / 2, 0);
                    break;
                case SpaceBorderAnchor.cameraRight:
                    borderView.Collider.transform.localPosition = bounds.max.Set( x: bounds.size.x / 2);
                    borderView.Collider.offset = bounds.max + new Vector3(borderView.Collider.size.x / 2, 0);
                    break;
                default:
                    borderView.Collider.offset = bounds.max;
                    break;
            }
        }
    }
}