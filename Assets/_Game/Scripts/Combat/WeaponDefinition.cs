using UnityEngine;

namespace OrbRaiders.Combat
{
    [CreateAssetMenu(fileName = "EnergyOrbWeapon", menuName = "Orb Raiders/Weapon Definition")]
    public class WeaponDefinition : ScriptableObject
    {
        public string id = "EnergyOrb";
        public string displayName = "ENERGY ORB";
        [TextArea] public string description = "Standard issue fantasy-tech energy orb that auto-targets nearby foes.";

        public float baseDamage = 10f;
        public float attackSpeed = 1.0f;     // 1 attack/sec
        public float projectileSpeed = 7.0f; // 7 units/sec
        public float range = 8.0f;           // 8 units
        public float projectileSize = 1.0f;  // 1 scale multiplier
        public int baseProjectileCount = 1;

        public GameObject projectilePrefab;
    }
}
