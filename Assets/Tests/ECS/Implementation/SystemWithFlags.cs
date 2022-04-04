using ECS.System;

namespace Tests.Implementation
{
    internal class SystemWithFlags : IECSSystem, IECSUpdateSystem, IECSFixedUpdateSystem
    {
        public bool WasCreate;
        public bool WasDestroy;
        public int UpdateCounter;
        public int FixedUpdateCounter;
        
        public void OnCreate() => WasCreate = true;
        public void OnDestroy() => WasDestroy = true;
        public void Update() => UpdateCounter++;
        public void FixedUpdate() => FixedUpdateCounter++;
    }
}