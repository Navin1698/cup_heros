using UnityEngine;
using OrbRaiders.Core;
using OrbRaiders.Combat;

namespace OrbRaiders.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Projectile Reference")]
        [SerializeField] private Projectile defaultProjectilePrefab;

        private PlayerStats stats;
        private PlayerTargeting targeting;
        private PlayerHealth health;
        private float attackTimer = 0f;

        public float UltimateEnergy { get; private set; } = 0f;
        public float MaxUltimateEnergy { get; private set; } = 100f;
        public bool IsUltimateReady => UltimateEnergy >= MaxUltimateEnergy;

        private void Awake()
        {
            stats = GetComponent<PlayerStats>();
            targeting = GetComponent<PlayerTargeting>();
            health = GetComponent<PlayerHealth>();

            // Create default runtime projectile if prefab not set
            if (defaultProjectilePrefab == null)
            {
                GameObject projGO = new GameObject("DefaultProjectilePrefab");
                projGO.SetActive(false);
                defaultProjectilePrefab = projGO.AddComponent<Projectile>();
                var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.SetParent(projGO.transform);
                sphere.transform.localScale = Vector3.one * 0.4f;
                var col = sphere.GetComponent<Collider>();
                if (col != null) col.isTrigger = true;
            }
        }

        private void Update()
        {
            if (health != null && !health.IsAlive) return;

            float attackInterval = 1.0f / Mathf.Max(0.1f, stats.AttackSpeed);
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackInterval && targeting.HasTarget)
            {
                attackTimer = 0f;
                ExecuteAttack();
            }
        }

        public void AddUltimateEnergy(float amount)
        {
            UltimateEnergy = Mathf.Clamp(UltimateEnergy + amount, 0f, MaxUltimateEnergy);
        }

        public void TriggerUltimate()
        {
            if (!IsUltimateReady) return;

            UltimateEnergy = 0f;
            Debug.Log("[PlayerCombat] CORE BURST ULTIMATE ACTIVATED!");

            // Explosive burst around Nova
            Collider[] hits = Physics.OverlapSphere(transform.position, 10.0f);
            foreach (var hit in hits)
            {
                var enemy = hit.GetComponent<Enemies.EnemyBase>();
                if (enemy != null && enemy.IsAlive)
                {
                    enemy.TakeDamage(new DamageResult
                    {
                        amount = stats.AttackDamage * 3.5f,
                        type = DamageType.Void,
                        isCritical = true,
                        source = gameObject,
                        target = enemy.gameObject,
                        knockbackDirection = (enemy.transform.position - transform.position).normalized,
                        knockbackForce = 6.0f
                    });
                }
            }

            if (Services.HapticManager.Instance != null)
            {
                Services.HapticManager.Instance.TriggerHeavy();
            }
        }

        private void ExecuteAttack()
        {
            if (targeting.CurrentTarget == null) return;

            Vector3 targetPos = targeting.CurrentTarget.position;
            Vector3 aimDir = (targetPos - transform.position);
            aimDir.y = 0;
            aimDir.Normalize();

            int count = stats.ProjectileCount;
            float spreadAngle = GetSpreadAngle(count);

            for (int i = 0; i < count; i++)
            {
                float offsetAngle = 0f;
                if (count > 1)
                {
                    float step = spreadAngle / (count - 1);
                    offsetAngle = -spreadAngle * 0.5f + step * i;
                }

                Quaternion rotation = Quaternion.AngleAxis(offsetAngle, Vector3.up);
                Vector3 projDir = rotation * aimDir;

                bool isCrit = Random.value <= stats.CritChance;
                float projDamage = isCrit ? stats.AttackDamage * stats.CritMultiplier : stats.AttackDamage;

                Projectile proj = PoolManager.Instance.Spawn(defaultProjectilePrefab, transform.position + Vector3.up * 0.8f, Quaternion.identity);
                proj.Initialize(
                    projDir,
                    7.0f,
                    projDamage,
                    isCrit,
                    stats.PierceCount,
                    stats.BounceCount,
                    stats.ApplySlow,
                    stats.ApplyBurn,
                    gameObject
                );
            }

            AddUltimateEnergy(5.0f);
        }

        private float GetSpreadAngle(int count)
        {
            switch (count)
            {
                case 2: return 15f;
                case 3: return 30f;
                case 5: return 45f;
                case 7: return 60f;
                case 9: return 80f;
                default: return 0f;
            }
        }
    }
}
