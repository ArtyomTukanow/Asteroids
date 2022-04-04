using Core;
using UnityEngine;
using Utils;

namespace UserData
{
    public class UserPlayer
    {
        public int NextLaserTimeLeft => Mathf.Max(nextLaserTime - Game.Timer.Time, 0);
        public bool CanShootByLaser => LaserCount > 0 && Game.Timer.Time >= nextLaserTime;
        
        public int TimeToRegenLaser { get; private set; } = Settings.LASER_REGEN_TIME;
        public int LaserCount { get; private set; } = Settings.MAX_LASER_COUNT;
        public int Score { get; set; }

        private int nextLaserTime;

        public void Init()
        {
            Game.Timer.OnTimer += OnTimer;
        }

        public void OnDestroy()
        {
            Game.Timer.OnTimer -= OnTimer;
        }
        
        
        private void OnTimer()
        {
            if (LaserCount < Settings.MAX_LASER_COUNT)
            {
                TimeToRegenLaser--;
                if (TimeToRegenLaser <= 0)
                {
                    TimeToRegenLaser = Settings.LASER_REGEN_TIME;
                    LaserCount++;
                }
            }
        }

        public bool TryShootByLaser()
        {
            if (CanShootByLaser)
            {
                LaserCount--;
                nextLaserTime = Game.Timer.Time + Settings.LASER_SHOOT_INTERVAL;
                return true;
            }

            return false;
        }
    }
}