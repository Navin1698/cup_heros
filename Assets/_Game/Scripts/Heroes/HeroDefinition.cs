using UnityEngine;
using OrbRaiders.Core;

namespace OrbRaiders.Heroes
{
    [CreateAssetMenu(fileName = "NewHeroDefinition", menuName = "Orb Raiders/Hero Definition")]
    public class HeroDefinition : ScriptableObject
    {
        public string id = "Nova";
        public string displayName = "NOVA";
        [TextArea] public string description = "A stylized fantasy-tech warrior equipped with an energy gauntlet and floating orb.";
        public Sprite icon;
        public GameObject prefab;

        [Header("Base Stats")]
        public float baseMaxHealth = 100f;
        public float baseAttackDamage = 10f;
        public float baseAttackSpeed = 1.0f; // Attacks per sec
        public float baseMovementSpeed = 5.5f;
        public float baseArmor = 0f;
        public float baseCritChance = 0.05f; // 5%
        public float baseCritMultiplier = 1.5f; // 1.5x
        public float baseAttackRange = 8f;

        [Header("Ultimate Ability")]
        public string ultimateName = "CORE BURST";
        [TextArea] public string ultimateDescription = "Unleashes a massive energy explosion dealing 300% damage to all nearby enemies.";
        public float ultimateEnergyCost = 100f;
        public Sprite ultimateIcon;

        [Header("Rarity & Unlock")]
        public SkillRarity rarity = SkillRarity.Common;
        public int unlockCostGold = 0;
    }
}
