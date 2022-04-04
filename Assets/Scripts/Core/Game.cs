using System;
using Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UserData;
using View;

namespace Core
{
    public class Game : MonoBehaviour
    {
        private const int BASE_THRESHOLD = 10;
        private const int BASE_DPI = 96;

        private static Game instance;
        public static Game Instance => instance;
    
        private EventSystem eventSystem;
        public static EventSystem EventSystem => Instance.eventSystem ??= GameObject.Find("EventSystem").GetComponent<EventSystem>();
    
        private Camera mainCam;
        public static Camera MainCamera => Instance.mainCam ??= Camera.main;
        
        private StaticData staticData;
        public static StaticData Static => Instance.staticData;
        
        private User user;
        public static User User => Instance.user;
        
        private World.Main main;
        public static World.Main Main => Instance.main;
        
        private GameTimer timer;
        public static GameTimer Timer => Instance.timer;
        
        private HUD hud;
        public static HUD Hud => Instance.hud == null ? Instance.hud = FindObjectOfType<HUD>() : Instance.hud;

        public static void Create()
        {
            if (instance != null)
                return;
            
            var singleton = new GameObject("Game");
            DontDestroyOnLoad(singleton);
            instance = singleton.AddComponent<Game>();
        
            EventSystem.pixelDragThreshold = (int)(BASE_THRESHOLD * Screen.dpi / BASE_DPI);

            instance.Init();
        }

        private void Init()
        {
            CreateTimer();
            ParseStatic();
            CreateUser();
            CreateWorld();
            CreateHud();
        }

        private void CreateTimer()
        {
            timer = gameObject.AddComponent<GameTimer>();
        }
        
        private void ParseStatic()
        {
            staticData = new StaticData();
            Static.LoadModel();
        }

        private void CreateUser()
        {
            user = new User();
            user.Init();
        }

        private void CreateWorld()
        {
            main = gameObject.AddComponent<World.Main>();
            main.AutoCreateAllSystems();
        }

        private void CreateHud()
        {
            Hud.HudInit();
        }

        private void OnDestroy()
        {
            user.OnDestroy();
            main.World.OnDestroy();
        }
    }
}
