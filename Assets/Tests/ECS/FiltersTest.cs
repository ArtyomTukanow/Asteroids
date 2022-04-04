using ECS.Core;
using NUnit.Framework;
using Tests.Implementation;

namespace Tests.ECS
{
    public class FiltersTest
    {
        /// <summary>
        /// При добавлении компонентов в системе записываются private и public nonestatic поля, содержащие значение ECSFilter.
        /// </summary>
        [Test]
        public void CreateSystemWithFilter()
        {
            var world = new ECSWorld();
            var entity1 = world.Entities.AddEntity(typeof(SampleComponent));
            var system = world.Systems.GetSystem<SystemWithOneFilter>();
            var entity2 = world.Entities.AddEntity(typeof(SampleComponent));
            
            Assert.AreEqual(2, system.PrivateFilter.Count);
            Assert.AreEqual(2, system.PublicFilter.Count);
            Assert.IsNull(system.StaticFilter);
        }
        
        /// <summary>
        /// Фильтр добавляется в поле системы ISystem
        /// </summary>
        [Test]
        public void GetFilterFromSystem()
        {
            var world = new ECSWorld();
            var entity = world.Entities.AddEntity(typeof(SampleComponent));
            var system = world.Systems.GetSystem<SystemWithOneFilter>();
            
            Assert.AreEqual(entity, system.PrivateFilter.Get(0));
        }
        
        /// <summary>
        /// При удалении компонента из Entity фильтр обновится
        /// </summary>
        [Test]
        public void ClearEntityWillUpdateFilter()
        {
            var world = new ECSWorld();
            var entity = world.Entities.AddEntity(typeof(SampleComponent));
            var system = world.Systems.GetSystem<SystemWithOneFilter>();
            
            Assert.AreEqual(1, system.PrivateFilter.Count);
            
            entity.Remove<SampleComponent>();
            
            Assert.AreEqual(0, system.PrivateFilter.Count);
        }
        
        /// <summary>
        /// При удалении системы фильтры в ней вернут null значение
        /// </summary>
        [Test]
        public void FilterAfterDestroySystemAssertNull()
        {
            var world = new ECSWorld();
            var entity = world.Entities.AddEntity(typeof(SampleComponent));
            var system = world.Systems.GetSystem<SystemWithOneFilter>();
            
            world.Systems.RemoveSystem<SystemWithOneFilter>();
            
            Assert.IsNull(system.PrivateFilter);
        }
        
        /// <summary>
        /// При создании нескольких Entity фильтры обновятся.
        /// </summary>
        [Test]
        public void AddSeveralEntitiesWillUpdateFilters()
        {
            var world = new ECSWorld();
            var system = world.Systems.GetSystem<SystemWithOneFilter>();
            
            world.Entities.AddEntity(typeof(SampleComponent));
            world.Entities.AddEntity(typeof(SampleComponent));
            
            Assert.AreEqual(2, system.PrivateFilter.Count);
            
            world.Entities.AddEntity(typeof(SampleComponent));
            world.Entities.AddEntity(typeof(SampleComponent2));
            
            Assert.AreEqual(3, system.PrivateFilter.Count);
        }
        
        /// <summary>
        /// В фильтр с несколькими компонентоми добавляются только те Entity, у которых присутствует именно 2 этих компонента
        /// При удалении второго компонента из такого фильтра удалится Entity
        /// Фильтры Sample1AndSample2Filter,Sample1Filter и Sample2Filter работают независимо друг от друга
        /// </summary>
        [Test]
        public void CheckFilterWithTwoComponents()
        {
            var world = new ECSWorld();
            
            var entity1 = world.Entities.AddEntity(typeof(SampleComponent), typeof(SampleComponent2));
            
            var system = world.Systems.GetSystem<SystemWithSeveralFilters>();

            Assert.AreEqual(1, system.Sample1AndSample2Filter.Count);
            Assert.AreEqual(1, system.Sample1Filter.Count);
            Assert.AreEqual(1, system.Sample2Filter.Count);
            
            var entity2 = world.Entities.AddEntity(typeof(SampleComponent));
            
            Assert.AreEqual(1, system.Sample1AndSample2Filter.Count);
            Assert.AreEqual(2, system.Sample1Filter.Count);
            Assert.AreEqual(1, system.Sample2Filter.Count);
            
            entity1.Remove<SampleComponent2>();
            
            Assert.AreEqual(0, system.Sample1AndSample2Filter.Count);
            Assert.AreEqual(2, system.Sample1Filter.Count);
            Assert.AreEqual(0, system.Sample2Filter.Count);

            entity2.Get<SampleComponent2>();
            
            Assert.AreEqual(1, system.Sample1AndSample2Filter.Count);
            Assert.AreEqual(2, system.Sample1Filter.Count);
            Assert.AreEqual(1, system.Sample2Filter.Count);
        }
    }
}