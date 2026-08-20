using System;
using System.IO;
using UnityEngine;

namespace OrbRaiders.Save
{
    public class SaveManager : MonoBehaviour, ISaveService
    {
        public static SaveManager Instance { get; private set; }

        public SaveData CurrentData { get; private set; } = new SaveData();

        private string SaveFilePath => Path.Combine(Application.persistentDataPath, "orbraiders_save.json");

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Load();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Save();
            }
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        public void Save()
        {
            try
            {
                string json = JsonUtility.ToJson(CurrentData, true);
                File.WriteAllText(SaveFilePath, json);
                Debug.Log($"[SaveManager] Saved data to {SaveFilePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveManager] Error saving data: {ex.Message}");
            }
        }

        public void Load()
        {
            try
            {
                if (File.Exists(SaveFilePath))
                {
                    string json = File.ReadAllText(SaveFilePath);
                    CurrentData = JsonUtility.FromJson<SaveData>(json);
                    Debug.Log($"[SaveManager] Loaded save data. Gold: {CurrentData.Gold}, Hero: {CurrentData.SelectedHeroId}");
                }
                else
                {
                    CurrentData = new SaveData();
                    Save();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveManager] Error loading save data: {ex.Message}");
                CurrentData = new SaveData();
            }
        }

        public void ResetProgress()
        {
            CurrentData = new SaveData();
            Save();
            Debug.Log("[SaveManager] Reset progress.");
        }
    }
}
