using System.Collections.Generic;
using UnityEngine;
using OrbRaiders.Core;

namespace OrbRaiders.Enemies
{
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager Instance { get; private set; }

        [SerializeField] private float minSpawnRadius = 12.0f;
        [SerializeField] private float maxSpawnRadius = 16.0f;

        private readonly List<EnemyBase> activeEnemies = new List<EnemyBase>();

        public int ActiveEnemyCount => activeEnemies.Count;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public EnemyBase SpawnEnemy(EnemyType type, EnemyDefinition def = null)
        {
            Vector3 center = Player.PlayerController.Instance != null ? Player.PlayerController.Instance.transform.position : Vector3.zero;

            float angle = Random.Range(0f, Mathf.PI * 2f);
            float radius = Random.Range(minSpawnRadius, maxSpawnRadius);
            Vector3 spawnPos = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

            GameObject enemyGO = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemyGO.name = $"Enemy_{type}";
            enemyGO.tag = "Enemy";

            EnemyBase enemyComp = null;
            switch (type)
            {
                case EnemyType.Swarmer: enemyComp = enemyGO.AddComponent<SwarmerEnemy>(); break;
                case EnemyType.Archer: enemyComp = enemyGO.AddComponent<ArcherEnemy>(); break;
                case EnemyType.Tank: enemyComp = enemyGO.AddComponent<TankEnemy>(); break;
                case EnemyType.Bomber: enemyComp = enemyGO.AddComponent<BomberEnemy>(); break;
                default: enemyComp = enemyGO.AddComponent<SlimeEnemy>(); break;
            }

            if (def == null)
            {
                def = ScriptableObject.CreateInstance<EnemyDefinition>();
                def.type = type;
                def.maxHealth = GetDefaultHP(type);
                def.moveSpeed = GetDefaultSpeed(type);
                def.damage = GetDefaultDamage(type);
                def.xpDropValue = GetDefaultXP(type);
            }

            var renderer = enemyGO.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = new Material(Shader.Find("Sprites/Default"));
                renderer.material.color = GetTypeColor(type);
            }

            enemyComp.Initialize(def, spawnPos);
            activeEnemies.Add(enemyComp);

            return enemyComp;
        }

        public void ClearAllEnemies()
        {
            for (int i = activeEnemies.Count - 1; i >= 0; i--)
            {
                if (activeEnemies[i] != null)
                {
                    Destroy(activeEnemies[i].gameObject);
                }
            }
            activeEnemies.Clear();
        }

        private Color GetTypeColor(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Swarmer: return new Color(1f, 0.8f, 0.2f);
                case EnemyType.Archer: return new Color(0.2f, 0.8f, 1f);
                case EnemyType.Tank: return new Color(0.5f, 0.3f, 0.8f);
                case EnemyType.Bomber: return new Color(1f, 0.2f, 0.2f);
                default: return new Color(0.2f, 1f, 0.4f);
            }
        }

        private float GetDefaultHP(EnemyType type)
        {
            switch (type) { case EnemyType.Tank: return 120f; case EnemyType.Swarmer: return 25f; default: return 50f; }
        }
        private float GetDefaultSpeed(EnemyType type)
        {
            switch (type) { case EnemyType.Swarmer: return 5.0f; case EnemyType.Tank: return 2.0f; default: return 3.2f; }
        }
        private float GetDefaultDamage(EnemyType type)
        {
            switch (type) { case EnemyType.Bomber: return 30f; case EnemyType.Swarmer: return 5f; default: return 10f; }
        }
        private int GetDefaultXP(EnemyType type)
        {
            switch (type) { case EnemyType.Tank: return 30; case EnemyType.Swarmer: return 8; default: return 15; }
        }
    }
}
