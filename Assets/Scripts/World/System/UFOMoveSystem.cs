using System.Linq;
using Core;
using ECS.Core;
using ECS.System;
using JetBrains.Annotations;
using Utils;
using World.Component;
using World.Components;


namespace World.System
{
    public class UFOMoveSystem : IECSSystem
    {
        private ECSFilter<SpaceObjectComponent, PlayerComponent> players;
        private ECSFilter<SpaceObjectComponent, UFOComponent> ufos;

        [CanBeNull]
        private ECSEntity Player => players.GetEntities()?.FirstOrDefault();

        private int ufoNextGenerateTimeLeft;
        
        public void OnCreate()
        {
            Game.User.Level.OnChangeLevel += OnChangeLevel;
            Game.Timer.OnTimer += OnTimer;
        }

        public void OnDestroy()
        {
            Game.User.Level.OnChangeLevel -= OnChangeLevel;
            Game.Timer.OnTimer -= OnTimer;
        }

        private void OnChangeLevel()
        {
            ufoNextGenerateTimeLeft = Game.User.Level.LevelDifficult.UfoDelay;
        }
        
        private void OnTimer()
        {
            var player = Player;
            if (player == null)
                return;

            if (ufos.IsEmpty())
            {
                ufoNextGenerateTimeLeft--;
                if (ufoNextGenerateTimeLeft == 0)
                {
                    ufoNextGenerateTimeLeft = Game.User.Level.LevelDifficult.UfoDelay;
                    Game.Main.World.GetSystem<LevelGeneratorSystem>().CreateUFO();
                }
            }
            
            foreach (var ufo in ufos.GetEntities())
                ChangeDirectionForUFO(ufo, player);
        }

        private void ChangeDirectionForUFO(ECSEntity ufo, ECSEntity player)
        {
            var ufoGO = ufo.Get<SpaceObjectComponent>().GameObject;
            var playerGO = player.Get<SpaceObjectComponent>().GameObject;

            var newSpeed = (playerGO.transform.position - ufoGO.transform.position).normalized * Settings.UFO_SPEED;
            ufo.Get<SpaceObjectComponent>().Speed = newSpeed;
        }
    }
}