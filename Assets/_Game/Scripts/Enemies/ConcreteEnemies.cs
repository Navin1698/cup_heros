using UnityEngine;
using OrbRaiders.Combat;

namespace OrbRaiders.Enemies
{
    // 1. Slime (Slow melee)
    public class SlimeEnemy : EnemyBase
    {
        protected override void ExecuteAttack()
        {
            if (playerTransform == null) return;
            var playerHealth = playerTransform.GetComponent<Player.PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(new DamageResult
                {
                    amount = definition != null ? definition.damage : 10f,
                    type = DamageType.Physical,
                    source = gameObject,
                    target = playerTransform.gameObject
                });
            }
        }
    }

    // 2. Swarmer (Fast weak enemy)
    public class SwarmerEnemy : EnemyBase
    {
        protected override void ExecuteAttack()
        {
            if (playerTransform == null) return;
            var playerHealth = playerTransform.GetComponent<Player.PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(new DamageResult
                {
                    amount = definition != null ? definition.damage : 5f,
                    type = DamageType.Physical,
                    source = gameObject,
                    target = playerTransform.gameObject
                });
            }
        }
    }

    // 3. Archer (Ranged attacker)
    public class ArcherEnemy : EnemyBase
    {
        protected override void ExecuteAttack()
        {
            if (playerTransform == null) return;

            // Shoots a simple ranged projectile towards player
            Vector3 aimDir = (playerTransform.position - transform.position).normalized;
            GameObject arrow = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            arrow.transform.position = transform.position + Vector3.up * 0.5f;
            arrow.transform.localScale = Vector3.one * 0.3f;

            var proj = arrow.AddComponent<Projectile>();
            proj.Initialize(aimDir, 8f, definition != null ? definition.damage : 12f, false, 0, 0, false, false, gameObject);
        }
    }

    // 4. Tank (High HP defensive)
    public class TankEnemy : EnemyBase
    {
        protected override void ExecuteAttack()
        {
            if (playerTransform == null) return;
            var playerHealth = playerTransform.GetComponent<Player.PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(new DamageResult
                {
                    amount = definition != null ? definition.damage : 20f,
                    type = DamageType.Physical,
                    source = gameObject,
                    target = playerTransform.gameObject
                });
            }
        }
    }

    // 5. Bomber (Suicide explosion)
    public class BomberEnemy : EnemyBase
    {
        protected override void ExecuteAttack()
        {
            if (playerTransform == null) return;

            // Explode near player
            Collider[] hits = Physics.OverlapSphere(transform.position, 3.5f);
            foreach (var hit in hits)
            {
                var ph = hit.GetComponent<Player.PlayerHealth>();
                if (ph != null)
                {
                    ph.TakeDamage(new DamageResult
                    {
                        amount = definition != null ? definition.damage * 2f : 30f,
                        type = DamageType.Fire,
                        source = gameObject,
                        target = ph.gameObject
                    });
                }
            }

            OnDeath();
        }
    }
}
