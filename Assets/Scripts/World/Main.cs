using Core;
using ECS.Core;
using ECS.System;
using UnityEngine;
using Utils;
using World.System;

namespace World
{
    public class Main : MonoBehaviour
    {
        public ECSWorld World { get; private set; }
        public WorldFactory WorldFactory { get; private set; }

        private void Awake()
        {
            WorldFactory = new WorldFactory(this);
            World = new ECSWorld();
        }

        private void Update()
        {
            World.Update();
        }

        private void FixedUpdate()
        {
            World.FixedUpdate();
        }

        /// <summary>
        /// Автоматически создает контроллеры из пространства имен, где хранится MainController
        /// </summary>
        public void AutoCreateAllSystems() => WorldUtils.InvokeMethodWithGenericTypes(typeof(Main),"AutoAddSystem", AllSystems.GetAll());

        /// <summary>
        /// Используется в <seealso cref="AutoCreateAllSystems"/>
        /// </summary>
        private static void AutoAddSystem<T>() where T: IECSSystem, new() => Game.Main.World.GetSystem<T>();

    }
}