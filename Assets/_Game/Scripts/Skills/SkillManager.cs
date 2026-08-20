using System.Collections.Generic;
using UnityEngine;
using OrbRaiders.Core;

namespace OrbRaiders.Skills
{
    public class SkillManager : MonoBehaviour
    {
        public static SkillManager Instance { get; private set; }

        [SerializeField] private List<SkillDefinition> availableSkills = new List<SkillDefinition>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            InitializeDefaultSkillsIfEmpty();
        }

        public List<SkillDefinition> GetRandomSkills(int count = 3)
        {
            List<SkillDefinition> selected = new List<SkillDefinition>();
            List<SkillDefinition> pool = new List<SkillDefinition>(availableSkills);

            while (selected.Count < count && pool.Count > 0)
            {
                int randomIndex = Random.Range(0, pool.Count);
                selected.Add(pool[randomIndex]);
                pool.RemoveAt(randomIndex);
            }

            return selected;
        }

        private void InitializeDefaultSkillsIfEmpty()
        {
            if (availableSkills.Count > 0) return;

            // 1. Rapid Fire (+25% attack speed)
            var s1 = ScriptableObject.CreateInstance<SkillDefinition>();
            s1.id = "RapidFire"; s1.displayName = "RAPID FIRE"; s1.rarity = SkillRarity.Rare;
            s1.description = "+25% Attack Speed";
            s1.modifiers = new[] { new SkillModifier { effectType = SkillEffectType.AttackSpeed, value = 0.25f, isPercentage = true } };
            availableSkills.Add(s1);

            // 2. Orb Split (+1 projectile)
            var s2 = ScriptableObject.CreateInstance<SkillDefinition>();
            s2.id = "OrbSplit"; s2.displayName = "ORB SPLIT"; s2.rarity = SkillRarity.Epic;
            s2.description = "+1 Projectile";
            s2.modifiers = new[] { new SkillModifier { effectType = SkillEffectType.ProjectileCount, value = 1, isPercentage = false } };
            availableSkills.Add(s2);

            // 3. Power Core (+20% damage)
            var s3 = ScriptableObject.CreateInstance<SkillDefinition>();
            s3.id = "PowerCore"; s3.displayName = "POWER CORE"; s3.rarity = SkillRarity.Common;
            s3.description = "+20% Damage";
            s3.modifiers = new[] { new SkillModifier { effectType = SkillEffectType.Damage, value = 0.20f, isPercentage = true } };
            availableSkills.Add(s3);

            // 4. Piercing Core (+1 penetration)
            var s4 = ScriptableObject.CreateInstance<SkillDefinition>();
            s4.id = "PiercingCore"; s4.displayName = "PIERCING CORE"; s4.rarity = SkillRarity.Rare;
            s4.description = "+1 Piercing target";
            s4.modifiers = new[] { new SkillModifier { effectType = SkillEffectType.Piercing, value = 1, isPercentage = false } };
            availableSkills.Add(s4);

            // 5. Critical Core (+10% crit chance)
            var s5 = ScriptableObject.CreateInstance<SkillDefinition>();
            s5.id = "CriticalCore"; s5.displayName = "CRITICAL CORE"; s5.rarity = SkillRarity.Epic;
            s5.description = "+10% Critical Chance";
            s5.modifiers = new[] { new SkillModifier { effectType = SkillEffectType.CritChance, value = 0.10f, isPercentage = false } };
            availableSkills.Add(s5);

            // 6. Swift Step (+15% movement speed)
            var s6 = ScriptableObject.CreateInstance<SkillDefinition>();
            s6.id = "SwiftStep"; s6.displayName = "SWIFT STEP"; s6.rarity = SkillRarity.Common;
            s6.description = "+15% Movement Speed";
            s6.modifiers = new[] { new SkillModifier { effectType = SkillEffectType.MovementSpeed, value = 0.15f, isPercentage = true } };
            availableSkills.Add(s6);

            // 7. Frost Core (Apply slow)
            var s7 = ScriptableObject.CreateInstance<SkillDefinition>();
            s7.id = "FrostCore"; s7.displayName = "FROST CORE"; s7.rarity = SkillRarity.Rare;
            s7.description = "Projectiles apply 30% Slow effect";
            s7.modifiers = new[] { new SkillModifier { effectType = SkillEffectType.Slow, value = 0.30f, isPercentage = false } };
            availableSkills.Add(s7);

            // 8. Flame Core (Apply burn)
            var s8 = ScriptableObject.CreateInstance<SkillDefinition>();
            s8.id = "FlameCore"; s8.displayName = "FLAME CORE"; s8.rarity = SkillRarity.Rare;
            s8.description = "Projectiles ignite enemies with Burn DOT";
            s8.modifiers = new[] { new SkillModifier { effectType = SkillEffectType.Burn, value = 5f, isPercentage = false } };
            availableSkills.Add(s8);

            // 9. Chain Energy (Chance to chain)
            var s9 = ScriptableObject.CreateInstance<SkillDefinition>();
            s9.id = "ChainEnergy"; s9.displayName = "CHAIN ENERGY"; s9.rarity = SkillRarity.Legendary;
            s9.description = "Projectiles chain lightning to nearby targets";
            s9.modifiers = new[] { new SkillModifier { effectType = SkillEffectType.Chain, value = 1, isPercentage = false } };
            availableSkills.Add(s9);

            // 10. Bounce Core (Projectile bounces)
            var s10 = ScriptableObject.CreateInstance<SkillDefinition>();
            s10.id = "BounceCore"; s10.displayName = "BOUNCE CORE"; s10.rarity = SkillRarity.Epic;
            s10.description = "+1 Projectile Bounce";
            s10.modifiers = new[] { new SkillModifier { effectType = SkillEffectType.Bounce, value = 1, isPercentage = false } };
            availableSkills.Add(s10);

            // 11. Life Spark (+2% lifesteal)
            var s11 = ScriptableObject.CreateInstance<SkillDefinition>();
            s11.id = "LifeSpark"; s11.displayName = "LIFE SPARK"; s11.rarity = SkillRarity.Mythic;
            s11.description = "+2% Lifesteal on damage dealt";
            s11.modifiers = new[] { new SkillModifier { effectType = SkillEffectType.Lifesteal, value = 0.02f, isPercentage = false } };
            availableSkills.Add(s11);
        }
    }
}
