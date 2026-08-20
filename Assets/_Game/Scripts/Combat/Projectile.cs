using UnityEngine;
using OrbRaiders.Core;

namespace OrbRaiders.Combat
{
    public class Projectile : MonoBehaviour
    {
        [Header("Projectile Base Properties")]
        [SerializeField] protected float speed = 7.0f;
        [SerializeField] protected float maxRange = 8.0f;
        [SerializeField] protected float damage = 10f;
        [SerializeField] protected bool isCritical = false;
        [SerializeField] protected int pierceRemaining = 0;
        [SerializeField] protected int bounceRemaining = 0;
        [SerializeField] protected bool applySlow = false;
        [SerializeField] protected bool applyBurn = false;

        protected Vector3 launchPosition;
        protected Vector3 moveDirection;
        protected GameObject ownerSource;
        protected bool isInitialized = false;

        public virtual void Initialize(Vector3 direction, float projSpeed, float projDamage, bool crit, int pierce, int bounce, bool slow, bool burn, GameObject owner)
        {
            moveDirection = direction.normalized;
            speed = projSpeed;
            damage = projDamage;
            isCritical = crit;
            pierceRemaining = pierce;
            bounceRemaining = bounce;
            applySlow = slow;
            applyBurn = burn;
            ownerSource = owner;
            launchPosition = transform.position;
            isInitialized = true;

            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
            }
        }

        protected virtual void Update()
        {
            if (!isInitialized) return;

            float delta = speed * Time.deltaTime;
            transform.position += moveDirection * delta;

            if (Vector3.Distance(launchPosition, transform.position) >= maxRange)
            {
                OnMaxRangeReached();
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!isInitialized) return;

            // Check enemy hit
            if (other.CompareTag("Enemy") || other.GetComponent<Enemies.EnemyBase>() != null)
            {
                var enemy = other.GetComponent<Enemies.EnemyBase>();
                if (enemy != null && enemy.IsAlive)
                {
                    OnHitEnemy(enemy);
                }
            }
        }

        protected virtual void OnHitEnemy(Enemies.EnemyBase enemy)
        {
            DamageResult result = new DamageResult
            {
                amount = damage,
                type = isCritical ? DamageType.Critical : DamageType.Physical,
                isCritical = isCritical,
                source = ownerSource,
                target = enemy.gameObject,
                knockbackDirection = moveDirection,
                knockbackForce = 2.0f
            };

            if (applySlow)
            {
                result.statusEffect = StatusEffectType.Slow;
                result.statusDuration = 3.0f;
            }
            else if (applyBurn)
            {
                result.statusEffect = StatusEffectType.Burn;
                result.statusDuration = 4.0f;
            }

            enemy.TakeDamage(result);

            if (pierceRemaining > 0)
            {
                pierceRemaining--;
            }
            else if (bounceRemaining > 0)
            {
                bounceRemaining--;
                ReflectDirection(enemy.transform.position);
            }
            else
            {
                Despawn();
            }
        }

        protected virtual void ReflectDirection(Vector3 hitPos)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            moveDirection = Vector3.Reflect(moveDirection, randomOffset).normalized;
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }

        protected virtual void OnMaxRangeReached()
        {
            Despawn();
        }

        public virtual void Despawn()
        {
            isInitialized = false;
            gameObject.SetActive(false);
        }
    }
}
