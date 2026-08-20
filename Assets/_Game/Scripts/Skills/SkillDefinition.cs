using UnityEngine;
using OrbRaiders.Core;

namespace OrbRaiders.Skills
{
    public enum SkillEffectType
    {
        AttackSpeed,
        ProjectileCount,
        Damage,
        ProjectileSize,
        Piercing,
        CritChance,
        CritMultiplier,
        MovementSpeed,
        Lifesteal,
        Slow,
        Burn,
        Chain,
        Bounce,
        Explosive,
        HealthRegen
    }

    [System.Serializable]
    public struct SkillModifier
    {
        public SkillEffectType effectType;
        public float value; // e.g. 0.25 for +25% or 1 for +1 projectile/pierce
        public bool isPercentage;
    }

    [CreateAssetMenu(fileName = "NewSkillDefinition", menuName = "Orb Raiders/Skill Definition")]
    public class SkillDefinition : ScriptableObject
    {
        public string id;
        public string displayName;
        [TextArea] public string description;
        public Sprite icon;
        public SkillRarity rarity = SkillRarity.Common;
        public int maxLevel = 5;

        public SkillModifier[] modifiers;

        public string[] prerequisiteSkillIds;
        public SynergyType synergyType = SynergyType.None;
    }
}
