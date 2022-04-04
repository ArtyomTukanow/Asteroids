using ECS.Core;
using ECS.System;
using UnityEngine;
using World.Component;
using World.Events;

namespace World.System
{
    /// <summary>
    /// Вызывает событие об изменении размера камеры
    /// </summary>
    public class CameraEventSystem : IECSSystem, IECSUpdateSystem
    {
        private ECSFilter<SpaceBorderComponent> bordersFilter = null;
        
        private int lastWidth;
        private int lastHeight;

        public CameraEventSystem()
        {
            
        }

        public void OnCreate()
        {
            TryInvokeEvent();
        }

        public void OnDestroy()
        {
            
        }

        public void Update()
        {
            TryInvokeEvent();
        }
        
        private void TryInvokeEvent()
        {
            if(lastWidth != Screen.width || lastHeight != Screen.height)
                InvokeEvent();
        }

        private void InvokeEvent()
        {
            lastWidth = Screen.width;
            lastHeight = Screen.height;

            foreach (var border in bordersFilter.GetEntities())
                border.Get<CameraSEvent>();
        }
    }
}