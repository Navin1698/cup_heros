using System.Collections.Generic;
using UnityEngine;
using OrbRaiders.Skills;

namespace OrbRaiders.Player
{
    public class PlayerSkills : MonoBehaviour
    {
        private readonly Dictionary<string, int> acquiredSkillLevels = new Dictionary<string, int>();
        private PlayerStats stats;

        private void Awake()
        {
            stats = GetComponent<PlayerStats>();
        }

        public bool HasSkill(string skillId)
        {
            return acquiredSkillLevels.ContainsKey(skillId);
        }

        public int GetSkillLevel(string skillId)
        {
            return acquiredSkillLevels.TryGetValue(skillId, out int lvl) ? lvl : 0;
        }

        public void ApplySkill(SkillDefinition skill)
        {
            if (skill == null) return;

            int currentLvl = GetSkillLevel(skill.id);
            acquiredSkillLevels[skill.id] = currentLvl + 1;

            Debug.Log($"[PlayerSkills] Applied Skill: {skill.displayName} (Level {acquiredSkillLevels[skill.id]})");

            if (skill.modifiers != null)
            {
                foreach (var mod in skill.modifiers)
                {
                    ApplyModifier(mod);
                }
            }

            // Check Synergies
            Skills.SkillSynergySystem.Instance?.EvaluateSynergies(acquiredSkillLevels, stats);
        }

        private void ApplyModifier(SkillModifier mod)
        {
            if (stats == null) return;

            switch (mod.effectType)
            {
                case SkillEffectType.AttackSpeed:
                    stats.AttackSpeed += mod.isPercentage ? stats.AttackSpeed * mod.value : mod.value;
                    break;
                case SkillEffectType.ProjectileCount:
                    stats.ProjectileCount += (int)mod.value;
                    break;
                case SkillEffectType.Damage:
                    stats.AttackDamage += mod.isPercentage ? stats.AttackDamage * mod.value : mod.value;
                    break;
                case SkillEffectType.MovementSpeed:
                    stats.MoveSpeed += mod.isPercentage ? stats.MoveSpeed * mod.value : mod.value;
                    break;
                case SkillEffectType.Piercing:
                    stats.PierceCount += (int)mod.value;
                    break;
                case SkillEffectType.CritChance:
                    stats.CritChance = Mathf.Clamp01(stats.CritChance + mod.value);
                    break;
                case SkillEffectType.Lifesteal:
                    stats.LifestealPercent += mod.value;
                    break;
                case SkillEffectType.Slow:
                    stats.ApplySlow = true;
                    break;
                case SkillEffectType.Burn:
                    stats.ApplyBurn = true;
                    break;
                case SkillEffectType.Chain:
                    stats.ApplyChain = true;
                    break;
                case SkillEffectType.Bounce:
                    stats.BounceCount += (int)mod.value;
                    break;
            }
        }
    }
}
