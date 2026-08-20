using UnityEngine;

namespace OrbRaiders.Bosses
{
    [CreateAssetMenu(fileName = "NewBossDefinition", menuName = "Orb Raiders/Boss Definition")]
    public class BossDefinition : ScriptableObject
    {
        public string bossId = "EmberGolem";
        public string bossName = "EMBER GOLEM";
        public float maxHealth = 1000f;
        public float moveSpeed = 2.0f;
        public float baseDamage = 25f;
        public int phaseCount = 2;
        public float phase2Threshold = 0.5f; // 50% HP triggers Phase 2
        public Color bossColor = new Color(1.0f, 0.35f, 0.1f);
        public GameObject prefab;
    }
}
