using ECS.Core;
using ECS.Exceptions;
using NUnit.Framework;
using Tests.Implementation;

namespace Tests.ECS
{
    public class WorldTests
    {
        /// <summary>
        /// Пустой ECSWorld запускает Update и FixedUpdate без ошибок
        /// </summary>
        [Test]
        public void UpdateEmptyWorldWithoutErrors()
        {
            var world = new ECSWorld();
            world.Update();
            world.FixedUpdate();
        }

        /// <summary>
        /// При уничтожении ECSWorld, ECSWorld.Update() и ECSWorld.FixedUpdate() генерируют ошибку DestroyException
        /// </summary>
        [Test]
        public void UpdateWorldAfterDestroy()
        {
            var world = new ECSWorld();
            world.OnDestroy();
            
            try
            {
                world.Update();
                world.FixedUpdate();
            }
            catch (DestroyException e)
            {
                Assert.Pass("ECSWorld.Update and ECSWorld.FixedUpdate was excepted after destroy!");
                return;
            }
                
            Assert.Fail("ECSWorld.Update and ECSWorld.FixedUpdate must be excepted after destroy!");
        }

        /// <summary>
        /// При уничтожении ECSWorld, создание тового ECSEntity генерируют ошибку DestroyException
        /// </summary>
        [Test]
        public void AddEntityAfterDestroy()
        {
            var world = new ECSWorld();
            world.OnDestroy();
            
            try
            {
                world.Entities.AddEntity(typeof(SampleComponent));
            }
            catch (DestroyException e)
            {
                Assert.Pass("ECSEntities.AddEntity was excepted after destroy!");
                return;
            }
                
            Assert.Fail("ECSEntities.AddEntity must be excepted after destroy!");
        }

        /// <summary>
        /// Чейнинг работает в последовательности и не вызывает ошибок
        /// </summary>
        [Test]
        public void CreateSystemsFromChaining()
        {
            var world = new ECSWorld()
                .WithSystem<SystemWithOneFilter>()
                .WithEntity(typeof(SampleComponent), typeof(SampleComponent2))
                .WithSystem<SystemWithSeveralFilters>();
            
            Assert.AreEqual(2, world.Systems.Count);
            Assert.AreEqual(1, world.GetSystem<SystemWithSeveralFilters>().Sample1AndSample2Filter.Count);
        }
    }
}