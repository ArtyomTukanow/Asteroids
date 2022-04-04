using Core;
using UnityEngine;
using UnityEngine.UI;
using World.Component;
using World.System;

namespace View
{
    public class HudContent : MonoBehaviour
    {
        [SerializeField] private Text level;
        [SerializeField] private Text score;
        [SerializeField] private Text coordinates;
        [SerializeField] private Text angle;
        [SerializeField] private Text speed;
        [SerializeField] private Text laserCount;
        [SerializeField] private Text nextLaserTimeLeft;
        [SerializeField] private Text timeToRegenLaser;
        
        [SerializeField] private GameObject gameOverWindow;
        [SerializeField] private Text gameOverText;
        [SerializeField] private Button againButton;

        private void Awake()
        {
            againButton.onClick.RemoveAllListeners();
            againButton.onClick.AddListener(Game.User.PlayAgain);

            Game.User.Level.OnGameOver += CheckGameOver;
            Game.User.Level.OnChangeLevel += CheckGameOver;
            CheckGameOver();
        }

        private void CheckGameOver()
        {
            var isGameOver = Game.User.Level.IsGameOver;
            gameOverWindow.SetActive(isGameOver);

            if (isGameOver)
                gameOverText.text = "Game over.\n Score: " + Game.User.Player.Score;
        }

        private void Update()
        {
            var player = Game.Main.World
                .TryGetSystem<LevelGeneratorSystem>()?
                .Player?.Get<SpaceObjectComponent>();
            
            level.text = "Level: " + Game.User.Level.LevelId;
            score.text = "Score: " + Game.User.Player.Score;
            coordinates.text = "Coordinates: " + player?.GameObject.transform.position;
            angle.text = "Angle: " + player?.GameObject.transform.rotation.eulerAngles.z;
            speed.text = "Speed: " + player?.Speed;
            laserCount.text = "Laser Count: " + Game.User.Player.LaserCount;
            nextLaserTimeLeft.text = "Next Laser Time Left: " + Game.User.Player.NextLaserTimeLeft;
            timeToRegenLaser.text = "Time to REGEN laser: " + Game.User.Player.TimeToRegenLaser;
        }
    }
}