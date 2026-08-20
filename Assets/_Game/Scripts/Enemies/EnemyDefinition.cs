using UnityEngine;
using OrbRaiders.Core;

namespace OrbRaiders.Enemies
{
    [CreateAssetMenu(fileName = "NewEnemyDefinition", menuName = "Orb Raiders/Enemy Definition")]
    public class EnemyDefinition : ScriptableObject
    {
        public EnemyType type;
        public string displayName;
        public float maxHealth = 50f;
        public float damage = 10f;
        public float moveSpeed = 3f;
        public float attackRange = 1.2f;
        public float attackCooldown = 1.5f;
        public int xpDropValue = 15;
        public Color modelColor = Color.green;
        public GameObject prefab;
    }
}
