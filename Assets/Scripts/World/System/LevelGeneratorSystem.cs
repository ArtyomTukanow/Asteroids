using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using ECS.Core;
using ECS.System;
using Model;
using Utils;
using World.Component;
using World.Components;
using Object = UnityEngine.Object;

namespace World.System
{
    /// <summary>
    /// Управляет добавление объектов на поле
    /// </summary>
    public class LevelGeneratorSystem : IECSSystem, IECSFixedUpdateSystem
    {
        public ECSEntity Player => players.GetEntities().FirstOrDefault();
            
        private ECSFilter<PlayerComponent> players;
        private ECSFilter<SpaceBorderComponent> borders;
        
        private ECSFilter<AsteroidComponent> asteroids;
        private ECSFilter<UFOComponent> ufos;


        public void OnCreate()
        {
            Game.User.Level.OnChangeLevel += OnChangeLevel;
        }

        public void OnDestroy()
        {
            Game.User.Level.OnChangeLevel -= OnChangeLevel;
        }

        public void FixedUpdate()
        {
            CheckLevelComplete();
            CheckGameOver();
        }
        

        private void CheckLevelComplete()
        {
            if (asteroids.IsEmpty() && ufos.IsEmpty())
                Game.User.Level.NextLevel();
        }

        private void CheckGameOver()
        {
            if (players.IsEmpty())
                Game.User.Level.ChangeToGameOver();
        }

        private void OnChangeLevel()
        {
            CreateLevel(Game.User.Level.LevelDifficult);
        }
        
        
        
        
        
        public void CreateLevel(LevelDifficult levelData)
        {
            DestroyLevel();
            
            CreatePlayer();
            CreateBorders();
            CreateAsteroids(levelData);
        }

        private void DestroyLevel()
        {
            foreach (var asteroid in new List<ECSEntity>(asteroids.GetEntities()))
            {
                Object.Destroy(asteroid.Get<SpaceObjectComponent>().GameObject);
                asteroid.Destroy();
            }

            foreach (var ufo in new List<ECSEntity>(ufos.GetEntities()))
            {
                Object.Destroy(ufo.Get<SpaceObjectComponent>().GameObject);
                ufo.Destroy();
            }
        }

        private void CreatePlayer()
        {
            if(players.IsEmpty())
                Game.Main.WorldFactory.CreatePlayer();
        }

        private void CreateBorders()
        {
            if(borders.IsEmpty())
                foreach (SpaceBorderAnchor anchor in Enum.GetValues(typeof(SpaceBorderAnchor)))
                    Game.Main.WorldFactory.CreateBorder(anchor);
        }

        private void CreateAsteroids(LevelDifficult levelData)
        {
            for (var i = 0; i < levelData.AsteroidsCount; i++)
            {
                var asteroid = Game.Main.WorldFactory.CreateAsteroid();
                MoveSpaceObjectToRandomPosition(asteroid);
            }
        }

        public void CreateUFO()
        {
            var ufo = Game.Main.WorldFactory.CreateUFO();
            var bounds = Game.MainCamera.GetWorldBounds();
            ufo.Get<SpaceObjectComponent>().GameObject.transform.position = (bounds.center + bounds.max).Set(z: 0);
        }
        

        
        
        
        


        private void MoveSpaceObjectToRandomPosition(ECSEntity spaceObject)
        {
            var throwCounter = 100;
            var bounds = Game.MainCamera.GetWorldBounds();

            do
            {
                spaceObject.Get<SpaceObjectComponent>().GameObject.transform.position = bounds.RandomPosition().Set(z: 0);
            } while (SpaceObjectHitAnyPlayer(spaceObject) && --throwCounter > 0);

            if (throwCounter == 0)
                throw new Exception("Can't find random position for asteroid!");
        }

        private bool SpaceObjectHitAnyPlayer(ECSEntity spaceObject)
        {
            foreach (var player in players.GetEntities())
            {
                var playerCollider = player.Get<SpaceObjectComponent>().Collider;
                var spaceObjCollider = spaceObject.Get<SpaceObjectComponent>().Collider;
                
                if (playerCollider.IsTouching(spaceObjCollider))
                    return true;
            }

            return false;
        }
    }
}