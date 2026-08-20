using UnityEngine;

namespace OrbRaiders.Services
{
    public class HapticManager : MonoBehaviour
    {
        public static HapticManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void TriggerLight()
        {
            if (!IsHapticsEnabled()) return;
#if UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif
        }

        public void TriggerMedium()
        {
            if (!IsHapticsEnabled()) return;
#if UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif
        }

        public void TriggerHeavy()
        {
            if (!IsHapticsEnabled()) return;
#if UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif
        }

        private bool IsHapticsEnabled()
        {
            return Save.SaveManager.Instance == null || Save.SaveManager.Instance.CurrentData.HapticsEnabled;
        }
    }
}
