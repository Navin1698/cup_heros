using UnityEngine;
using System.Collections;
using OrbRaiders.Core;
using OrbRaiders.Combat;

namespace OrbRaiders.Bosses
{
    public abstract class BossBase : MonoBehaviour
    {
        [SerializeField] protected BossDefinition definition;

        public float CurrentHP { get; protected set; }
        public float MaxHP => definition != null ? definition.maxHealth : 1000f;
        public bool IsAlive => CurrentHP > 0;
        public int CurrentPhase { get; protected set; } = 1;
        public string BossName => definition != null ? definition.bossName : "BOSS";

        protected Transform playerTransform;
        protected bool isAttacking = false;

        public virtual void Initialize(BossDefinition def, Vector3 spawnPos)
        {
            definition = def;
            transform.position = spawnPos;
            CurrentHP = MaxHP;
            CurrentPhase = 1;
            gameObject.SetActive(true);

            if (Player.PlayerController.Instance != null)
            {
                playerTransform = Player.PlayerController.Instance.transform;
            }

            EventBus.Raise(new BossSpawnedSignal
            {
                BossName = BossName,
                MaxHP = MaxHP
            });

            StartCoroutine(BossLoopRoutine());
        }

        protected abstract IEnumerator BossLoopRoutine();

        public virtual void TakeDamage(DamageResult damage)
        {
            if (!IsAlive) return;

            CurrentHP = Mathf.Max(0f, CurrentHP - damage.amount);

            EventBus.Raise(new BossHPChangedSignal
            {
                CurrentHP = CurrentHP,
                MaxHP = MaxHP,
                CurrentPhase = CurrentPhase
            });

            // Check Phase 2 trigger
            if (CurrentPhase == 1 && definition != null && CurrentHP / MaxHP <= definition.phase2Threshold)
            {
                TriggerPhase2();
            }

            if (CurrentHP <= 0)
            {
                OnDeath();
            }
        }

        protected virtual void TriggerPhase2()
        {
            CurrentPhase = 2;
            Debug.Log($"[BossBase] {BossName} ENTERED PHASE 2!");
        }

        protected virtual void OnDeath()
        {
            CurrentHP = 0;
            Debug.Log($"[BossBase] {BossName} DEFEATED!");

            EventBus.Raise(new BossDefeatedSignal { BossName = BossName });

            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.ChangeState(GameState.Victory);
            }

            gameObject.SetActive(false);
        }
    }
}
