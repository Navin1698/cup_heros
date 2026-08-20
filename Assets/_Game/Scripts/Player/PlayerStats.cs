using UnityEngine;
using OrbRaiders.Heroes;

namespace OrbRaiders.Player
{
    public class PlayerStats : MonoBehaviour
    {
        public HeroDefinition BaseHero { get; private set; }

        public float MaxHealth { get; set; } = 100f;
        public float AttackDamage { get; set; } = 10f;
        public float AttackSpeed { get; set; } = 1.0f;
        public float MoveSpeed { get; set; } = 5.5f;
        public float Armor { get; set; } = 0f;
        public float CritChance { get; set; } = 0.05f;
        public float CritMultiplier { get; set; } = 1.5f;
        public float AttackRange { get; set; } = 8.0f;
        public float LifestealPercent { get; set; } = 0.0f;
        public int ProjectileCount { get; set; } = 1;
        public int PierceCount { get; set; } = 0;
        public int BounceCount { get; set; } = 0;
        public bool ApplySlow { get; set; } = false;
        public bool ApplyBurn { get; set; } = false;
        public bool ApplyChain { get; set; } = false;

        public void Initialize(HeroDefinition heroDef)
        {
            BaseHero = heroDef;
            RecalculateStats();
        }

        public void RecalculateStats()
        {
            if (BaseHero == null) return;

            float hpBonus = 0f;
            float dmgBonus = 0f;
            float speedBonus = 0f;
            float attackSpeedBonus = 0f;
            float critChanceBonus = 0f;

            if (Save.SaveManager.Instance != null)
            {
                var save = Save.SaveManager.Instance.CurrentData;
                hpBonus += save.BonusHPLevel * 10f;
                dmgBonus += save.BonusAttackLevel * 1.5f;
                speedBonus += save.BonusMoveSpeedLevel * 0.2f;
                attackSpeedBonus += save.BonusAttackSpeedLevel * 0.05f;
                critChanceBonus += save.BonusCritChanceLevel * 0.02f;
            }

            MaxHealth = BaseHero.baseMaxHealth + hpBonus;
            AttackDamage = BaseHero.baseAttackDamage + dmgBonus;
            AttackSpeed = BaseHero.baseAttackSpeed + attackSpeedBonus;
            MoveSpeed = BaseHero.baseMovementSpeed + speedBonus;
            Armor = BaseHero.baseArmor;
            CritChance = Mathf.Clamp01(BaseHero.baseCritChance + critChanceBonus);
            CritMultiplier = BaseHero.baseCritMultiplier;
            AttackRange = BaseHero.baseAttackRange;
        }
    }
}
