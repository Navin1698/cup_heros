using UnityEngine;
using OrbRaiders.Core;
using OrbRaiders.Combat;

namespace OrbRaiders.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        private PlayerStats stats;

        public float CurrentHP { get; private set; }
        public float MaxHP => stats != null ? stats.MaxHealth : 100f;
        public bool IsAlive => CurrentHP > 0;

        private void Awake()
        {
            stats = GetComponent<PlayerStats>();
        }

        private void Start()
        {
            ResetHealth();
        }

        public void ResetHealth()
        {
            CurrentHP = MaxHP;
            NotifyHealthChanged();
        }

        public void TakeDamage(DamageResult damage)
        {
            if (!IsAlive) return;

            float damageReduction = stats.Armor / (stats.Armor + 50f);
            float finalDamage = Mathf.Max(1.0f, damage.amount * (1.0f - damageReduction));

            CurrentHP = Mathf.Max(0f, CurrentHP - finalDamage);
            NotifyHealthChanged();

            if (Services.HapticManager.Instance != null)
            {
                Services.HapticManager.Instance.TriggerMedium();
            }

            if (CurrentHP <= 0)
            {
                OnDeath();
            }
        }

        public void Heal(float amount)
        {
            if (!IsAlive) return;
            CurrentHP = Mathf.Min(MaxHP, CurrentHP + amount);
            NotifyHealthChanged();
        }

        public void ApplyLifesteal(float damageDealt)
        {
            if (stats != null && stats.LifestealPercent > 0)
            {
                Heal(damageDealt * stats.LifestealPercent);
            }
        }

        private void OnDeath()
        {
            Debug.Log("[PlayerHealth] Player Defeated!");
            GameStateManager.Instance?.ChangeState(GameState.GameOver);
        }

        private void NotifyHealthChanged()
        {
            EventBus.Raise(new PlayerHPChangedSignal
            {
                CurrentHP = CurrentHP,
                MaxHP = MaxHP
            });
        }
    }
}
