using UnityEngine;
using OrbRaiders.Enemies;
using OrbRaiders.Bosses;

namespace OrbRaiders.Player
{
    public class PlayerTargeting : MonoBehaviour
    {
        private PlayerStats stats;

        public Transform CurrentTarget { get; private set; }
        public bool HasTarget => CurrentTarget != null;

        private void Awake()
        {
            stats = GetComponent<PlayerStats>();
        }

        private void Update()
        {
            FindTarget();
        }

        private void FindTarget()
        {
            float searchRadius = stats != null ? stats.AttackRange : 8.0f;
            Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);

            Transform bestTarget = null;
            float closestDistanceSqr = float.MaxValue;
            bool bossFound = false;

            foreach (var col in colliders)
            {
                var boss = col.GetComponent<BossBase>();
                if (boss != null && boss.IsAlive)
                {
                    CurrentTarget = boss.transform;
                    return;
                }

                var enemy = col.GetComponent<EnemyBase>();
                if (enemy != null && enemy.IsAlive)
                {
                    float distSqr = (enemy.transform.position - transform.position).sqrMagnitude;
                    if (distSqr < closestDistanceSqr)
                    {
                        closestDistanceSqr = distSqr;
                        bestTarget = enemy.transform;
                    }
                }
            }

            CurrentTarget = bestTarget;
        }
    }
}
