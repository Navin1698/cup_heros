using UnityEngine;
using OrbRaiders.Core;
using OrbRaiders.Combat;

namespace OrbRaiders.Enemies
{
    [RequireComponent(typeof(StatusEffectManager))]
    public abstract class EnemyBase : MonoBehaviour
    {
        [SerializeField] protected EnemyDefinition definition;

        public float CurrentHP { get; protected set; }
        public float MaxHP => definition != null ? definition.maxHealth : 50f;
        public bool IsAlive => CurrentHP > 0;
        public EnemyType Type => definition != null ? definition.type : EnemyType.Slime;

        protected Transform playerTransform;
        protected StatusEffectManager statusManager;
        protected float attackTimer = 0f;

        protected virtual void Awake()
        {
            statusManager = GetComponent<StatusEffectManager>();
        }

        public virtual void Initialize(EnemyDefinition def, Vector3 spawnPos)
        {
            definition = def;
            transform.position = spawnPos;
            CurrentHP = MaxHP;
            attackTimer = 0f;
            gameObject.SetActive(true);

            if (Player.PlayerController.Instance != null)
            {
                playerTransform = Player.PlayerController.Instance.transform;
            }
        }

        protected virtual void Update()
        {
            if (!IsAlive) return;

            if (playerTransform == null && Player.PlayerController.Instance != null)
            {
                playerTransform = Player.PlayerController.Instance.transform;
            }

            if (playerTransform == null) return;

            float dist = Vector3.Distance(transform.position, playerTransform.position);
            float currentSpeed = definition != null ? definition.moveSpeed : 3f;
            if (statusManager != null)
            {
                currentSpeed *= statusManager.MoveSpeedMultiplier;
            }

            if (dist > (definition != null ? definition.attackRange : 1.2f))
            {
                Vector3 moveDir = (playerTransform.position - transform.position).normalized;
                transform.position += moveDir * currentSpeed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(moveDir);
            }
            else
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= (definition != null ? definition.attackCooldown : 1.5f))
                {
                    attackTimer = 0f;
                    ExecuteAttack();
                }
            }
        }

        public virtual void TakeDamage(DamageResult damage)
        {
            if (!IsAlive) return;

            CurrentHP -= damage.amount;

            // Apply status effect if any
            if (damage.statusEffect.HasValue && statusManager != null)
            {
                statusManager.ApplyStatus(damage.statusEffect.Value, damage.statusDuration);
            }

            // Apply knockback
            if (damage.knockbackForce > 0)
            {
                transform.position += damage.knockbackDirection * damage.knockbackForce * 0.1f;
            }

            if (CurrentHP <= 0)
            {
                OnDeath();
            }
        }

        protected abstract void ExecuteAttack();

        protected virtual void OnDeath()
        {
            CurrentHP = 0;
            Debug.Log($"[EnemyBase] {gameObject.name} Defeated.");

            // Spawn XP Orb
            Progression.XPOrb.SpawnOrb(transform.position, definition != null ? definition.xpDropValue : 15);

            EventBus.Raise(new EnemyKilledSignal
            {
                Type = Type,
                Position = transform.position,
                XpValue = definition != null ? definition.xpDropValue : 15
            });

            SpawnManager.Instance?.UnregisterEnemy(this);
            gameObject.SetActive(false);
        }
    }
}
