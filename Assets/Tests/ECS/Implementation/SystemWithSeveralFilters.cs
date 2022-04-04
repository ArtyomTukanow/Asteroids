using ECS.Core;
using ECS.System;

namespace Tests.Implementation
{
    public class SystemWithSeveralFilters: IECSSystem
    {
        private ECSFilter<SampleComponent> sample1Filter;
        private ECSFilter<SampleComponent2> sample2Filter;
        private ECSFilter<SampleComponent, SampleComponent2> sample1AndSample2Filter;

        public ECSFilter<SampleComponent> Sample1Filter => sample1Filter;
        public ECSFilter<SampleComponent2> Sample2Filter => sample2Filter;
        public ECSFilter<SampleComponent, SampleComponent2> Sample1AndSample2Filter => sample1AndSample2Filter;
        
        public void OnCreate()
        {
            
        }

        public void OnDestroy()
        {
            
        }
    }
}