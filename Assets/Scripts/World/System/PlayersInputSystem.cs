using System.Linq;
using Core;
using ECS.Core;
using ECS.System;
using UnityEngine;
using Utils;
using World.Component;
using World.Components;

namespace World.System
{
    /// <summary>
    /// Выполняет действия игрока
    /// </summary>
    public class PlayersInputSystem : IECSSystem
    {
        private ECSFilter<PlayerComponent> players;

        private ECSEntity GetPlayer() => players.GetEntities().FirstOrDefault();


        public void OnCreate()
        {
            
        }

        public void OnDestroy()
        {
            
        }

        public void Move(Vector2 direction)
        {
            var player = GetPlayer();
            if(player != null)
            {
                var spaceObject = player.Get<SpaceObjectComponent>();
                
                if (direction.x != 0)
                {
                    var newRotate = spaceObject.GameObject.transform.rotation.eulerAngles.Add(z: - direction.x * Settings.INPUT_ROTATE_ANGLE);
                    spaceObject.GameObject.transform.rotation = Quaternion.Euler(newRotate);
                }

                if (direction.y > 0)
                {
                    var rotate = spaceObject.GameObject.transform.rotation.eulerAngles.z / 180 * Mathf.PI;
                    var x = Settings.ACCELERATION * Mathf.Cos(rotate);
                    var y = Settings.ACCELERATION * Mathf.Sin(rotate);
                    player.Get<SpaceObjectComponent>().Speed += new Vector3(x, y, 0);
                }
            }
        }

        public void ShootBullet()
        {
            var player = GetPlayer();
            if(player != null)
            {
                var spaceObject = player.Get<SpaceObjectComponent>();
                Game.Main.WorldFactory.CreateBullet(spaceObject.Direction * Settings.BULLET_SPEED, spaceObject.GameObject.transform.position);
            }
        }

        public void ShootLaser()
        {
            var player = GetPlayer();
            if (player != null)
            {
                if (Game.User.Player.TryShootByLaser())
                {
                    var spaceObject = player.Get<SpaceObjectComponent>();
                    Game.Main.WorldFactory.CreateLaser(spaceObject.GameObject.transform.rotation.eulerAngles, spaceObject.Direction, spaceObject.GameObject.transform.position);
                }
            }
        }
    }
}