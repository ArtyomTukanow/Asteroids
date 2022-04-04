using UnityEngine;
using UnityEngine.InputSystem;
using World.System;

namespace Core
{
    public class GameInput : MonoBehaviour
    {
        private Vector2 direction;
        
        private void Update()
        {
            if (direction != default)
            {
                Game.Main.World
                    .GetSystem<PlayersInputSystem>()
                    .Move(direction);
            }
        }

        public void Move(InputAction.CallbackContext ctx)
        {
            if (ctx.canceled)
                direction = default;
            else
                direction = ctx.ReadValue<Vector2>();
        }

        public void Shoot(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                Game.Main.World
                    .GetSystem<PlayersInputSystem>()
                    .ShootBullet();
            }
        }

        public void Laser(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                Game.Main.World
                    .GetSystem<PlayersInputSystem>()
                    .ShootLaser();
            }
        }
    }
}