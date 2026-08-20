using System.Collections.Generic;
using UnityEngine;

namespace OrbRaiders.Services
{
    public interface IAnalyticsService
    {
        void LogEvent(string eventName, Dictionary<string, object> parameters = null);
    }

    public class AnalyticsManager : MonoBehaviour, IAnalyticsService
    {
        public static AnalyticsManager Instance { get; private set; }

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

        public void LogEvent(string eventName, Dictionary<string, object> parameters = null)
        {
            string paramText = "";
            if (parameters != null)
            {
                foreach (var kvp in parameters)
                {
                    paramText += $"{kvp.Key}={kvp.Value}; ";
                }
            }
            Debug.Log($"[Analytics] {eventName} -> {paramText}");
        }
    }
}
