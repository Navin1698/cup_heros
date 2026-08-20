using System.Collections.Generic;
using UnityEngine;
using OrbRaiders.Core;
using OrbRaiders.Player;

namespace OrbRaiders.Skills
{
    public class SkillSynergySystem : MonoBehaviour
    {
        public static SkillSynergySystem Instance { get; private set; }

        private readonly HashSet<SynergyType> activeSynergies = new HashSet<SynergyType>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void EvaluateSynergies(Dictionary<string, int> acquiredSkills, PlayerStats stats)
        {
            if (acquiredSkills == null || stats == null) return;

            // 1. Frozen Chain (Frost Core + Chain Energy)
            if (acquiredSkills.ContainsKey("FrostCore") && acquiredSkills.ContainsKey("ChainEnergy"))
            {
                ActivateSynergy(SynergyType.FrozenChain, stats, () =>
                {
                    Debug.Log("[Synergy] FROZEN CHAIN ACTIVATED! Chained targets are frozen!");
                    stats.ApplySlow = true;
                    stats.ApplyChain = true;
                });
            }

            // 2. Burning Spear (Flame Core + Piercing Core)
            if (acquiredSkills.ContainsKey("FlameCore") && acquiredSkills.ContainsKey("PiercingCore"))
            {
                ActivateSynergy(SynergyType.BurningSpear, stats, () =>
                {
                    Debug.Log("[Synergy] BURNING SPEAR ACTIVATED! Piercing projectiles deal +50% fire damage!");
                    stats.AttackDamage *= 1.25f;
                    stats.ApplyBurn = true;
                });
            }

            // 3. Bounce Storm (Orb Split + Bounce Core)
            if (acquiredSkills.ContainsKey("OrbSplit") && acquiredSkills.ContainsKey("BounceCore"))
            {
                ActivateSynergy(SynergyType.BounceStorm, stats, () =>
                {
                    Debug.Log("[Synergy] BOUNCE STORM ACTIVATED! Projectiles bounce +2 additional times!");
                    stats.BounceCount += 2;
                });
            }

            // 4. Crit Rush (Critical Core + Rapid Fire)
            if (acquiredSkills.ContainsKey("CriticalCore") && acquiredSkills.ContainsKey("RapidFire"))
            {
                ActivateSynergy(SynergyType.CritRush, stats, () =>
                {
                    Debug.Log("[Synergy] CRIT RUSH ACTIVATED! +15% Crit Chance & +50% Crit Damage!");
                    stats.CritChance += 0.15f;
                    stats.CritMultiplier += 0.5f;
                });
            }
        }

        private void ActivateSynergy(SynergyType synergy, PlayerStats stats, System.Action applyCallback)
        {
            if (!activeSynergies.Contains(synergy))
            {
                activeSynergies.Add(synergy);
                applyCallback?.Invoke();
            }
        }
    }
}
