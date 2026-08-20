using UnityEngine;
using TMPro;
using OrbRaiders.Core;
using OrbRaiders.Enemies;
using OrbRaiders.Bosses;

namespace OrbRaiders.TestArena
{
    public class TestArenaManager : MonoBehaviour
    {
        [Header("Debug HUD UI")]
        [SerializeField] private TextMeshProUGUI debugHudText;

        private float fpsTimer = 0f;
        private int frameCount = 0;
        private float currentFPS = 60f;

        private void Start()
        {
            Debug.Log("[TestArenaManager] Initializing Test Arena environment...");

            // Automatically bootstrap player and managers if launching directly in TestArena
            if (GameStateManager.Instance == null)
            {
                GameObject gsm = new GameObject("[GameStateManager]");
                gsm.AddComponent<GameStateManager>();
            }

            if (Player.PlayerController.Instance == null)
            {
                GameObject playerObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                playerObj.name = "Player_Nova";
                playerObj.tag = "Player";
                playerObj.AddComponent<Player.PlayerController>();
            }

            GameStateManager.Instance.ChangeState(GameState.Battle);
        }

        private void Update()
        {
            // FPS Calculation
            frameCount++;
            fpsTimer += Time.unscaledDeltaTime;
            if (fpsTimer >= 0.5f)
            {
                currentFPS = frameCount / fpsTimer;
                frameCount = 0;
                fpsTimer = 0f;
            }

            UpdateDebugHUD();
        }

        private void UpdateDebugHUD()
        {
            if (debugHudText == null) return;

            int activeEnemies = SpawnManager.Instance != null ? SpawnManager.Instance.ActiveEnemyCount : 0;
            int currentWave = WaveManager.Instance != null ? WaveManager.Instance.CurrentWave : 1;
            float playerHP = Player.PlayerController.Instance != null && Player.PlayerController.Instance.Health != null ? Player.PlayerController.Instance.Health.CurrentHP : 0f;

            debugHudText.text = $"<b>DEBUG HUD</b>\n" +
                               $"FPS: {currentFPS:F1}\n" +
                               $"Wave: {currentWave}/10\n" +
                               $"Active Enemies: {activeEnemies}\n" +
                               $"Player HP: {playerHP:F0}\n" +
                               $"[Keys: 1=Slime, 2=Swarmer, 3=Archer, 4=Boss, 5=LevelUp, 6=KillAll]";

            // Quick Debug Keybinds
            if (Input.GetKeyDown(KeyCode.Alpha1)) SpawnManager.Instance?.SpawnEnemy(EnemyType.Slime);
            if (Input.GetKeyDown(KeyCode.Alpha2)) SpawnManager.Instance?.SpawnEnemy(EnemyType.Swarmer);
            if (Input.GetKeyDown(KeyCode.Alpha3)) SpawnManager.Instance?.SpawnEnemy(EnemyType.Archer);
            if (Input.GetKeyDown(KeyCode.Alpha4)) BossManager.Instance?.SpawnBoss("EmberGolem");
            if (Input.GetKeyDown(KeyCode.Alpha5)) Player.PlayerController.Instance?.Experience?.AddXP(100);
            if (Input.GetKeyDown(KeyCode.Alpha6)) SpawnManager.Instance?.ClearAllEnemies();
        }
    }
}
