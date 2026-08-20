using System;
using System.Collections.Generic;

namespace OrbRaiders.Core
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> SignalHandlers = new Dictionary<Type, List<Delegate>>();

        public static void Subscribe<T>(Action<T> handler)
        {
            Type type = typeof(T);
            if (!SignalHandlers.ContainsKey(type))
            {
                SignalHandlers[type] = new List<Delegate>();
            }
            SignalHandlers[type].Add(handler);
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            Type type = typeof(T);
            if (SignalHandlers.TryGetValue(type, out List<Delegate> handlers))
            {
                handlers.Remove(handler);
            }
        }

        public static void Raise<T>(T signal)
        {
            Type type = typeof(T);
            if (SignalHandlers.TryGetValue(type, out List<Delegate> handlers))
            {
                for (int i = handlers.Count - 1; i >= 0; i--)
                {
                    if (handlers[i] is Action<T> action)
                    {
                        action.Invoke(signal);
                    }
                }
            }
        }

        public static void Clear()
        {
            SignalHandlers.Clear();
        }
    }

    // Common Signal Definitions
    public struct GameStateChangedSignal
    {
        public GameState PreviousState;
        public GameState NewState;
    }

    public struct PlayerLevelUpSignal
    {
        public int NewLevel;
    }

    public struct PlayerXPChangedSignal
    {
        public int CurrentXP;
        public int MaxXP;
    }

    public struct PlayerHPChangedSignal
    {
        public float CurrentHP;
        public float MaxHP;
    }

    public struct EnemyKilledSignal
    {
        public EnemyType Type;
        public UnityEngine.Vector3 Position;
        public int XpValue;
    }

    public struct BossSpawnedSignal
    {
        public string BossName;
        public float MaxHP;
    }

    public struct BossHPChangedSignal
    {
        public float CurrentHP;
        public float MaxHP;
        public int CurrentPhase;
    }

    public struct BossDefeatedSignal
    {
        public string BossName;
    }

    public struct WaveChangedSignal
    {
        public int CurrentWave;
        public int TotalWaves;
    }
}
