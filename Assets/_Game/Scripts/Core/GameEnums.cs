using System;

namespace OrbRaiders.Core
{
    public enum GameState
    {
        Boot,
        MainMenu,
        WorldMap,
        HeroSelection,
        Battle,
        SkillSelection,
        Victory,
        GameOver,
        Pause,
        Shop,
        Inventory,
        Settings
    }

    public enum SkillRarity
    {
        Common,
        Rare,
        Epic,
        Legendary,
        Mythic
    }

    public enum DamageType
    {
        Physical,
        Fire,
        Ice,
        Void,
        Electric,
        Critical
    }

    public enum StatusEffectType
    {
        Burn,
        Freeze,
        Slow,
        Shock,
        Poison,
        Knockback
    }

    public enum EnemyType
    {
        Slime,
        Swarmer,
        Archer,
        Tank,
        Bomber,
        Splitter,
        Shield,
        Healer,
        Teleporter,
        Summoner
    }

    public enum SynergyType
    {
        None,
        FrozenChain,   // Frost + Chain
        BurningSpear,  // Flame + Pierce
        BounceStorm,   // Split + Bounce
        CritRush       // Critical + Rapid Fire
    }

    public enum EquipmentSlot
    {
        Weapon,
        Armor,
        Ring,
        Core
    }

    public enum HeroId
    {
        Nova,
        Ember,
        Frost,
        Volt,
        Void
    }

    public enum TelegraphType
    {
        Circle,
        Cone,
        Line,
        Ring
    }
}
