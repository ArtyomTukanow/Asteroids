using ECS.Core;
using NUnit.Framework;
using Tests.Implementation;

namespace Tests.ECS
{
    public class SystemTests
    {
        /// <summary>
        /// world.Update запускает IUpdateSystem.Update()
        /// world.FixedUpdate запускает IFixedUpdateSystem.FixedUpdate()
        /// </summary>
        [Test]
        public void CheckWasUpdateAndFixedUpdate()
        {
            var world = new ECSWorld();
            var system = world.Systems.GetSystem<SystemWithFlags>();
            
            world.Update();
            world.FixedUpdate();
            world.FixedUpdate();
            
            Assert.AreEqual(1, system.UpdateCounter);
            Assert.AreEqual(2, system.FixedUpdateCounter);
        }
        
        /// <summary>
        /// При создании системы вызывается ISystem.OnCreate()
        /// При удалении системы вызывается ISystem.OnDestroy()
        /// </summary>
        [Test]
        public void CheckWasCreateAndDestroy()
        {
            var world = new ECSWorld();
            var system = world.Systems.GetSystem<SystemWithFlags>();
            
            Assert.IsTrue(system.WasCreate);
            
            world.OnDestroy();
            
            Assert.IsTrue(system.WasDestroy);
        }
        
        /// <summary>
        /// В одном ECSWorld не может быть несколько систем одного типа 
        /// </summary>
        [Test]
        public void AddSystemToWorldOnce()
        {
            var world = new ECSWorld();
            
            var system1 = world.Systems.GetSystem<SystemWithFlags>();
            var system2 = world.Systems.GetSystem<SystemWithFlags>();
            
            Assert.AreEqual(system1, system2);
            Assert.AreEqual(1, world.Systems.Count);
        }
        
        /// <summary>
        /// Если удалить систему определенного типа, а потом добавить такую же, они будут разными экземплярами класса.
        /// В ECSWorld останется одна система
        /// </summary>
        [Test]
        public void CreateNewSystemAfterDestroySystem()
        {
            var world = new ECSWorld();
            
            var system1 = world.Systems.GetSystem<SystemWithFlags>();
            world.Systems.RemoveSystem<SystemWithFlags>();
            var system2 = world.Systems.GetSystem<SystemWithFlags>();
            
            Assert.AreNotEqual(system1, system2);
            Assert.AreEqual(1, world.Systems.Count);
        }
        
        /// <summary>
        /// Если удалить систему, которой нет в ECSWorld, ничего не произойдет
        /// </summary>
        [Test]
        public void RemoveEmptySystemWithNoErrors()
        {
            var world = new ECSWorld();
            
            world.Systems.GetSystem<SystemWithFlags>();
            world.Systems.RemoveSystem<SystemWithFlags>();
            world.Systems.RemoveSystem<SystemWithFlags>();
            
            Assert.AreEqual(0, world.Systems.Count);
        }
        
        /// <summary>
        /// Если в ECSWorld добавить несколько систем, они обе будут храниться в памяти
        /// </summary>
        [Test]
        public void CreateSeveralSystems()
        {
            var world = new ECSWorld();
            
            world.Systems.GetSystem<SystemWithFlags>();
            world.Systems.GetSystem<SampleSystem>();
            
            Assert.AreEqual(2, world.Systems.Count);
        }
    }
}