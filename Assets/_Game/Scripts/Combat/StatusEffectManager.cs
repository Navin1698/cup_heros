using System.Collections.Generic;
using UnityEngine;
using OrbRaiders.Core;

namespace OrbRaiders.Combat
{
    public class StatusInstance
    {
        public StatusEffectType Type;
        public float DurationRemaining;
        public float Strength; // Slow % or tick damage
        public float TickTimer;
    }

    public class StatusEffectManager : MonoBehaviour
    {
        private readonly List<StatusInstance> activeEffects = new List<StatusInstance>();

        public float MoveSpeedMultiplier { get; private set; } = 1.0f;
        public bool IsFrozen { get; private set; } = false;

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            float speedMult = 1.0f;
            bool frozen = false;

            for (int i = activeEffects.Count - 1; i >= 0; i--)
            {
                var effect = activeEffects[i];
                effect.DurationRemaining -= deltaTime;

                switch (effect.Type)
                {
                    case StatusEffectType.Slow:
                        speedMult *= Mathf.Clamp01(1.0f - effect.Strength);
                        break;
                    case StatusEffectType.Freeze:
                        frozen = true;
                        speedMult = 0f;
                        break;
                    case StatusEffectType.Burn:
                    case StatusEffectType.Poison:
                        effect.TickTimer += deltaTime;
                        if (effect.TickTimer >= 0.5f)
                        {
                            effect.TickTimer = 0f;
                            ApplyDotDamage(effect.Strength, effect.Type == StatusEffectType.Burn ? DamageType.Fire : DamageType.Physical);
                        }
                        break;
                }

                if (effect.DurationRemaining <= 0)
                {
                    activeEffects.RemoveAt(i);
                }
            }

            MoveSpeedMultiplier = speedMult;
            IsFrozen = frozen;
        }

        public void ApplyStatus(StatusEffectType type, float duration, float strength = 0.3f)
        {
            var existing = activeEffects.Find(e => e.Type == type);
            if (existing != null)
            {
                existing.DurationRemaining = Mathf.Max(existing.DurationRemaining, duration);
                existing.Strength = Mathf.Max(existing.Strength, strength);
            }
            else
            {
                activeEffects.Add(new StatusInstance
                {
                    Type = type,
                    DurationRemaining = duration,
                    Strength = strength,
                    TickTimer = 0f
                });
            }
        }

        private void ApplyDotDamage(float damageAmount, DamageType damageType)
        {
            var target = GetComponent<Enemies.EnemyBase>();
            if (target != null)
            {
                target.TakeDamage(new DamageResult
                {
                    amount = damageAmount,
                    type = damageType,
                    isCritical = false,
                    target = gameObject
                });
            }
        }
    }
}
