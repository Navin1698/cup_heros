using UnityEngine;
using OrbRaiders.Heroes;

namespace OrbRaiders.Player
{
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(PlayerHealth))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerTargeting))]
    [RequireComponent(typeof(PlayerCombat))]
    [RequireComponent(typeof(PlayerExperience))]
    [RequireComponent(typeof(PlayerSkills))]
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        public PlayerStats Stats { get; private set; }
        public PlayerHealth Health { get; private set; }
        public PlayerMovement Movement { get; private set; }
        public PlayerTargeting Targeting { get; private set; }
        public PlayerCombat Combat { get; private set; }
        public PlayerExperience Experience { get; private set; }
        public PlayerSkills Skills { get; private set; }

        [SerializeField] private HeroDefinition defaultHeroDefinition;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            Stats = GetComponent<PlayerStats>();
            Health = GetComponent<PlayerHealth>();
            Movement = GetComponent<PlayerMovement>();
            Targeting = GetComponent<PlayerTargeting>();
            Combat = GetComponent<PlayerCombat>();
            Experience = GetComponent<PlayerExperience>();
            Skills = GetComponent<PlayerSkills>();

            // Setup default hero definition if missing
            if (defaultHeroDefinition == null)
            {
                defaultHeroDefinition = ScriptableObject.CreateInstance<HeroDefinition>();
                defaultHeroDefinition.id = "Nova";
                defaultHeroDefinition.displayName = "NOVA";
            }

            Stats.Initialize(defaultHeroDefinition);
        }
    }
}
