using UnityEngine;

namespace OrbRaiders.Bosses
{
    public class BossManager : MonoBehaviour
    {
        public static BossManager Instance { get; private set; }

        public BossBase ActiveBoss { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public BossBase SpawnBoss(string bossId, BossDefinition def = null)
        {
            Vector3 spawnPos = Vector3.forward * 10f;
            if (Player.PlayerController.Instance != null)
            {
                spawnPos = Player.PlayerController.Instance.transform.position + Vector3.forward * 12f;
            }

            GameObject bossGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bossGO.name = $"Boss_{bossId}";
            bossGO.transform.localScale = Vector3.one * 2.5f;
            bossGO.tag = "Enemy";

            if (def == null)
            {
                def = ScriptableObject.CreateInstance<BossDefinition>();
                def.bossId = bossId;
                def.bossName = bossId == "EmberGolem" ? "EMBER GOLEM" : "VOID SERPENT";
                def.maxHealth = 1200f;
                def.baseDamage = 30f;
            }

            var renderer = bossGO.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = new Material(Shader.Find("Sprites/Default"));
                renderer.material.color = def.bossColor;
            }

            BossBase bossComp = bossGO.AddComponent<EmberGolemBoss>();
            bossComp.Initialize(def, spawnPos);
            ActiveBoss = bossComp;

            return bossComp;
        }
    }
}
