using UnityEngine;

namespace View
{
    public class HUD : MonoBehaviour
    {
        private const string HUD_CONTENT_PATH = "Prefabs/HudContent";
        
        public void HudInit()
        {
            Instantiate(Resources.Load<HudContent>(HUD_CONTENT_PATH), transform);
        }
    }
}