using System;
using Core;
using Model;

namespace UserData
{
    public class UserLevel
    {
        public int LevelId { get; private set; }

        private LevelDifficult levelDifficult;
        public LevelDifficult LevelDifficult => levelDifficult ??= Game.Static.LevelsDifficult.GetLevel(LevelId);

        public event Action OnChangeLevel;
        public event Action OnGameOver;

        public bool IsGameOver { get; private set; }

        public void Init()
        {
            
        }

        public void OnDestroy()
        {
            
        }

        public void ChangeToGameOver()
        {
            IsGameOver = true;
            OnGameOver?.Invoke();
        }

        public void NextLevel()
        {
            levelDifficult = null;
            LevelId++;
            
            OnChangeLevel?.Invoke();
        }

        public void Restart()
        {
            IsGameOver = false;
            levelDifficult = null;
            LevelId = 1;
            OnChangeLevel?.Invoke();
        }
    }
}