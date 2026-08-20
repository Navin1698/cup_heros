using UnityEngine;
using OrbRaiders.Core;

namespace OrbRaiders.Combat
{
    public struct DamageResult
    {
        public float amount;
        public DamageType type;
        public bool isCritical;
        public GameObject source;
        public GameObject target;
        public Vector3 knockbackDirection;
        public float knockbackForce;
        public StatusEffectType? statusEffect;
        public float statusDuration;
    }
}
