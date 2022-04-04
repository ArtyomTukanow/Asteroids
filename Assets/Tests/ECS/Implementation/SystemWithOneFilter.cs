using ECS.Core;
using ECS.System;

namespace Tests.Implementation
{
    public class SystemWithOneFilter: IECSSystem
    {
        private ECSFilter<SampleComponent> privateFilter;
        public ECSFilter<SampleComponent> publicFilter;
        private static ECSFilter<SampleComponent> staticFilter;

        public ECSFilter<SampleComponent> PrivateFilter => privateFilter;
        public ECSFilter<SampleComponent> PublicFilter => publicFilter;
        public ECSFilter<SampleComponent> StaticFilter => staticFilter;
        
        public void OnCreate()
        {
            
        }

        public void OnDestroy()
        {
            
        }
    }
}