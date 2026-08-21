using UnityEngine;

namespace OrbRaiders.Core
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private bool autoLoadMainMenuOnStart = true;

        private void Awake()
        {
            // Lock framerate & orientation as requested for Mobile (Section 51 & 54)
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Screen.orientation = ScreenOrientation.Portrait;

            Debug.Log("[GameBootstrap] Initializing ORB RAIDERS...");

            InitializeServices();
        }

        private void Start()
        {
            if (autoLoadMainMenuOnStart)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("TestArena");
            }
        }

        private void InitializeServices()
        {
            // Ensure singletons exist
            if (GameStateManager.Instance == null)
            {
                GameObject gsm = new GameObject("[GameStateManager]");
                gsm.AddComponent<GameStateManager>();
            }

            if (PoolManager.Instance == null)
            {
                GameObject pool = new GameObject("[PoolManager]");
                pool.AddComponent<PoolManager>();
            }

            if (Save.SaveManager.Instance == null)
            {
                GameObject save = new GameObject("[SaveManager]");
                save.AddComponent<Save.SaveManager>();
            }

            if (Audio.AudioManager.Instance == null)
            {
                GameObject audio = new GameObject("[AudioManager]");
                audio.AddComponent<Audio.AudioManager>();
            }

            if (Services.HapticManager.Instance == null)
            {
                GameObject haptic = new GameObject("[HapticManager]");
                haptic.AddComponent<Services.HapticManager>();
            }
        }
    }
}
