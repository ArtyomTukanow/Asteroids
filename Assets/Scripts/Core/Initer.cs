using UnityEngine;

namespace Core
{
    public class Initer : MonoBehaviour
    {
        private static Initer self;
        
        private void Awake()
        {
            if (self)
                Destroy(gameObject);

            self = this;

#if UNITY_EDITOR
            Caching.ClearCache();
#endif
            QualitySettings.SetQualityLevel(0);

            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            QualitySettings.antiAliasing = 0;
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            QualitySettings.shadows = ShadowQuality.Disable;
            QualitySettings.shadowCascades = 0;
            QualitySettings.shadowDistance = 0;

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private void Start()
        {
            if (Game.Instance == null)
                Game.Create();

            Destroy(gameObject);
        }
    }
}