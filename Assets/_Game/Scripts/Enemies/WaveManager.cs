using System.Collections;
using UnityEngine;
using OrbRaiders.Core;

namespace OrbRaiders.Enemies
{
    public class WaveManager : MonoBehaviour
    {
        public static WaveManager Instance { get; private set; }

        public int CurrentWave { get; private set; } = 1;
        public int TotalWaves { get; private set; } = 10;
        public bool IsWaveInProgress { get; private set; } = false;

        private int enemiesRemainingToSpawn = 0;
        private float spawnInterval = 1.2f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void StartBattle()
        {
            CurrentWave = 1;
            StartWave(CurrentWave);
        }

        public void StartWave(int waveNumber)
        {
            CurrentWave = waveNumber;
            IsWaveInProgress = true;

            EventBus.Raise(new WaveChangedSignal
            {
                CurrentWave = CurrentWave,
                TotalWaves = TotalWaves
            });

            if (CurrentWave == TotalWaves)
            {
                // Boss Wave!
                Debug.Log("[WaveManager] WAVE 10: BOSS WAVE REACHED!");
                Bosses.BossManager.Instance?.SpawnBoss("EmberGolem");
            }
            else
            {
                int enemyCount = 10 + (CurrentWave - 1) * 5;
                StartCoroutine(SpawnWaveRoutine(enemyCount));
            }
        }

        private IEnumerator SpawnWaveRoutine(int count)
        {
            enemiesRemainingToSpawn = count;

            while (enemiesRemainingToSpawn > 0)
            {
                if (GameStateManager.Instance != null && GameStateManager.Instance.IsState(GameState.Battle))
                {
                    EnemyType randomType = SelectEnemyTypeForWave(CurrentWave);
                    SpawnManager.Instance?.SpawnEnemy(randomType);
                    enemiesRemainingToSpawn--;
                }
                yield return new WaitForSeconds(spawnInterval);
            }

            // Wait until all enemies cleared
            while (SpawnManager.Instance != null && SpawnManager.Instance.ActiveEnemyCount > 0)
            {
                yield return new WaitForSeconds(0.5f);
            }

            OnWaveCleared();
        }

        private EnemyType SelectEnemyTypeForWave(int wave)
        {
            float rand = Random.value;
            if (wave < 3)
            {
                return rand < 0.7f ? EnemyType.Slime : EnemyType.Swarmer;
            }
            else if (wave < 7)
            {
                if (rand < 0.4f) return EnemyType.Slime;
                if (rand < 0.7f) return EnemyType.Swarmer;
                return EnemyType.Archer;
            }
            else
            {
                if (rand < 0.3f) return EnemyType.Archer;
                if (rand < 0.6f) return EnemyType.Tank;
                return EnemyType.Bomber;
            }
        }

        private void OnWaveCleared()
        {
            IsWaveInProgress = false;
            Debug.Log($"[WaveManager] Wave {CurrentWave} Cleared!");

            if (CurrentWave < TotalWaves)
            {
                StartWave(CurrentWave + 1);
            }
        }
    }
}
