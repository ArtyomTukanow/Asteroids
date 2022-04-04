using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

namespace Core
{
    public class GameTimer : MonoBehaviour
    {
        public event Action OnTimer;
        public List<Action> mainThreadActions = new List<Action>();

        public int Time { get; private set; }

        private void FixedUpdate()
        {
            CheckTimer();
            ExecuteActionsOnMainThread();
        }

        private void CheckTimer()
        {
            if (Time < (int)UnityEngine.Time.fixedTime)
            {
                Time = (int) UnityEngine.Time.fixedTime;
                OnTimer?.Invoke();
            }
        }

        private void ExecuteActionsOnMainThread()
        {
            if (mainThreadActions.Count > 0)
            {
                foreach (var action in mainThreadActions)
                    action?.Invoke();
                mainThreadActions.Clear();
            }
        }

        public void DelayedCall(float timeInSeconds, Action callback)
        {
            var timer = new Timer(timeInSeconds * 1000);
            timer.Elapsed += OnTimeEnd;
            timer.Enabled = true;
            timer.Start();
            
            void OnTimeEnd(object sender, ElapsedEventArgs e)
            {
                timer.Close();
                mainThreadActions.Add(callback);
            }
        }
    }
}