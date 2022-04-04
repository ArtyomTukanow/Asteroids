using ECS.Core;
using ECS.Exceptions;
using NUnit.Framework;
using Tests.Implementation;

namespace Tests.ECS
{
    public class EntityTests
    {
        /// <summary>
        /// Если создаем Entity до инициализации системы, эта Entity должна быть включена в систему
        /// </summary>
        [Test]
        public void CreateEntityBeforeCreatingSystem()
        {
            var world = new ECSWorld();
            var entity = world.Entities.AddEntity(typeof(SampleComponent));
            var system = world.Systems.GetSystem<SystemWithOneFilter>();
            
            Assert.AreEqual(1, system.PrivateFilter.Count);
        }
        
        /// <summary>
        /// Если создаем Entity после инициализации системы, эта Entity должна быть включена в систему
        /// </summary>
        [Test]
        public void CreateEntityAfterCreatingSystem()
        {
            var world = new ECSWorld();
            var system = world.Systems.GetSystem<SystemWithOneFilter>();
            var entity = world.Entities.AddEntity(typeof(SampleComponent));

            Assert.AreEqual(1, system.PrivateFilter.Count);
        }
        
        /// <summary>
        /// Пустой Entity без компонентов при создании генерирует ошибку
        /// </summary>
        [Test]
        public void CreateEmptyEntityDropException()
        {
            try
            {
                new ECSWorld().Entities.AddEntity();
            }
            catch (TypeException e)
            {
                Assert.Pass("It was exception while creating empty entity");
            }
            
            Assert.Fail("Was creating a new entity without components");
        }
        
        /// <summary>
        /// Entity с компонентом, не наследуемым IComponent при создании генерирует ошибку
        /// </summary>
        [Test]
        public void CreateEmptyEntityWithWrongComponentDropException()
        {
            try
            {
                new ECSWorld().Entities.AddEntity(typeof(NotAComponent));
            }
            catch (TypeException e)
            {
                Assert.Pass("It was exception while creating empty entity");
            }
            
            Assert.Fail("Was creating a new entity with wrong components");
        }
        
        /// <summary>
        /// 2 одинаковый компонента с одним типом в Entity создадутся как один
        /// </summary>
        [Test]
        public void CreateEntityWithSameComponents()
        {
            var world = new ECSWorld();
            var entity = world.Entities.AddEntity(typeof(SampleComponent), typeof(SampleComponent));
            
            Assert.AreEqual(1, entity.Count);
        }
        
        /// <summary>
        /// Добавляение компонента через ECSEntity.Get равносильно добавлению компонента через NewEntity. Компонент будет создан лишь раз
        /// </summary>
        [Test]
        public void GetComponentFromEntityNotCreateComponentIfItExist()
        {
            var world = new ECSWorld();
            var entity = world.Entities.AddEntity(typeof(SampleComponent));
            entity.Get<SampleComponent>();
            
            Assert.AreEqual(1, entity.Count);
        }
        
        /// <summary>
        /// Добавляение компонента через ECSEntity.Get создаст новый компонент, если он не был указан в NewEntity
        /// </summary>
        [Test]
        public void GetComponentFromEntityAssertCreateComponentIfItNotExist()
        {
            var world = new ECSWorld();
            var entity = world.Entities.AddEntity(typeof(SampleComponent));
            entity.Get<SampleComponent2>();
            
            Assert.AreEqual(2, entity.Count);
        }
        
        /// <summary>
        /// При удалении всех компонентов из Entity, он будет удален
        /// </summary>
        [Test]
        public void RemoveAllComponentsMakeEntityToDestroy()
        {
            var world = new ECSWorld();
            var entity = world.Entities.AddEntity(typeof(SampleComponent));
            entity.Remove<SampleComponent>();
            
            Assert.IsTrue(entity.Destroyed);
        }
    }
}