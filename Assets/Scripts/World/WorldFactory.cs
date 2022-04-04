using System;
using ECS.Core;
using UnityEngine;
using Utils;
using World.Component;
using World.Components;
using World.Events;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace World
{
    public class WorldFactory
    {
        private const string BORDER_PATH = "Prefabs/Border";
        private const string PLAYER_PATH = "Prefabs/Player";
        private const string ASTEROID_PATH = "Prefabs/Asteroid";
        private const string BULLET_PATH = "Prefabs/Bullet";
        private const string LASER_PATH = "Prefabs/Laser";
        private const string UFO_PATH = "Prefabs/UFO";

        private readonly Main main;

        public WorldFactory(Main main)
        {
            this.main = main;
        }

        public ECSEntity CreatePlayer()
        {
            var player = CreateSpaceObject(PLAYER_PATH);
            player.Get<SpaceObjectComponent>().Braking = 0.0001f;
            return player.Replace(new PlayerComponent());
        }

        public ECSEntity CreateAsteroid(AsteroidLevel level = AsteroidLevel.big)
        {
            var asteroid = CreateSpaceObject(
                ASTEROID_PATH,
                Random.Range(-3f, 3f),
                GetSpeedByAsteroidLevel(level));
            asteroid.Replace(new AsteroidComponent {Level = level});

            asteroid.Get<SpaceObjectComponent>().GameObject.transform.localScale = new Vector3((int)level, (int)level);
            
            return asteroid;
        }

        public ECSEntity CreateBullet(Vector2 speed, Vector2 position)
        {
            var bullet = CreateSpaceObject(BULLET_PATH, 0, speed);
            bullet.Get<SpaceObjectComponent>().GameObject.transform.position = position;
            bullet.Get<SpaceObjectComponent>().Behavior = SpaceBorderBehavior.destroy;
            bullet.Get<BulletComponent>();
            return bullet;
        }

        public ECSEntity CreateUFO()
        {
            var bullet = CreateSpaceObject(UFO_PATH);
            bullet.Get<UFOComponent>();
            return bullet;
        }

        public ECSEntity CreateLaser(Vector3 euler, Vector2 direction, Vector2 position)
        {
            var laser = CreateSpaceObject(LASER_PATH);

            var spaceObject = laser.Get<SpaceObjectComponent>();
            spaceObject.GameObject.transform.position = position;
            spaceObject.GameObject.transform.rotation = Quaternion.Euler(euler);
            
            laser.Replace(new LaserComponent { Direction = direction.normalized });
            return laser;
        }
        

        public ECSEntity CreateSpaceObject(string path, float angle = 0, Vector3 speed = default)
        {
            var resource = Resources.Load<GameObject>(path);
            
            if(resource == null)
                throw new ArgumentException($"Can't create GameObject with path {path}");
            
            var spaceObjectGO = Object.Instantiate(resource, main.transform);

            var entity = main.World.AddEntity(typeof(SpaceObjectComponent));
            entity.Replace(new SpaceObjectComponent
            {
                GameObject = spaceObjectGO,
                Collider = spaceObjectGO.GetComponent<Collider2D>(),
                Angle = angle,
                Speed = speed,
            });

            return entity;
        }

        public ECSEntity CreateBorder(SpaceBorderAnchor anchor)
        {
            var resource = Resources.Load<GameObject>(BORDER_PATH);
            var borderObjectGO = Object.Instantiate(resource, main.transform);

            var entity = main.World.AddEntity(typeof(SpaceBorderComponent));
            entity.Replace(new SpaceBorderComponent
            {
                Anchor = anchor,
                Collider = borderObjectGO.GetComponent<BoxCollider2D>()
            });

            //Для того, чтобы расположить границу относительно камеры
            entity.Get<CameraSEvent>();

            return entity;
        }
        
        
        
        

        private Vector3 GetSpeedByAsteroidLevel(AsteroidLevel level)
        {
            return new Vector3(
                Random.Range(level.MinSpeed(), level.MaxSpeed()).RandomSign(), 
                Random.Range(level.MinSpeed(), level.MaxSpeed()).RandomSign());
        }
    }
}