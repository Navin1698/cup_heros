using UnityEngine;
using OrbRaiders.Core;

namespace OrbRaiders.Combat
{
    public class PiercingProjectile : Projectile
    {
        [SerializeField] private int baseExtraPierce = 2;

        public override void Initialize(Vector3 direction, float projSpeed, float projDamage, bool crit, int pierce, int bounce, bool slow, bool burn, GameObject owner)
        {
            base.Initialize(direction, projSpeed, projDamage, crit, pierce + baseExtraPierce, bounce, slow, burn, owner);
        }
    }

    public class BouncingProjectile : Projectile
    {
        [SerializeField] private int baseExtraBounce = 2;

        public override void Initialize(Vector3 direction, float projSpeed, float projDamage, bool crit, int pierce, int bounce, bool slow, bool burn, GameObject owner)
        {
            base.Initialize(direction, projSpeed, projDamage, crit, pierce, bounce + baseExtraBounce, slow, burn, owner);
        }
    }

    public class ChainProjectile : Projectile
    {
        [SerializeField] private float chainRadius = 5.0f;

        protected override void OnHitEnemy(Enemies.EnemyBase enemy)
        {
            base.OnHitEnemy(enemy);

            // Chain to nearest adjacent enemy
            Collider[] hits = Physics.OverlapSphere(enemy.transform.position, chainRadius);
            foreach (var hit in hits)
            {
                var nextEnemy = hit.GetComponent<Enemies.EnemyBase>();
                if (nextEnemy != null && nextEnemy != enemy && nextEnemy.IsAlive)
                {
                    nextEnemy.TakeDamage(new DamageResult
                    {
                        amount = damage * 0.6f,
                        type = DamageType.Electric,
                        isCritical = false,
                        source = ownerSource,
                        target = nextEnemy.gameObject
                    });
                    break;
                }
            }
        }
    }

    public class ExplosiveProjectile : Projectile
    {
        [SerializeField] private float blastRadius = 3.0f;

        protected override void OnHitEnemy(Enemies.EnemyBase enemy)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, blastRadius);
            foreach (var hit in hits)
            {
                var target = hit.GetComponent<Enemies.EnemyBase>();
                if (target != null && target.IsAlive)
                {
                    target.TakeDamage(new DamageResult
                    {
                        amount = damage,
                        type = DamageType.Fire,
                        isCritical = isCritical,
                        source = ownerSource,
                        target = target.gameObject,
                        knockbackDirection = (target.transform.position - transform.position).normalized,
                        knockbackForce = 4.0f
                    });
                }
            }

            Despawn();
        }
    }
}
