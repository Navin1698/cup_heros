using UnityEngine;
using OrbRaiders.Core;

namespace OrbRaiders.Player
{
    public class PlayerExperience : MonoBehaviour
    {
        public int CurrentLevel { get; private set; } = 1;
        public int CurrentXP { get; private set; } = 0;
        public int MaxXP { get; private set; } = 100;

        private void Start()
        {
            RecalculateMaxXP();
            NotifyXPChanged();
        }

        public void AddXP(int amount)
        {
            CurrentXP += amount;

            while (CurrentXP >= MaxXP)
            {
                CurrentXP -= MaxXP;
                LevelUp();
            }

            NotifyXPChanged();
        }

        private void LevelUp()
        {
            CurrentLevel++;
            RecalculateMaxXP();

            Debug.Log($"[PlayerExperience] LEVEL UP! Reached Level {CurrentLevel}");

            EventBus.Raise(new PlayerLevelUpSignal { NewLevel = CurrentLevel });

            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.ChangeState(GameState.SkillSelection);
            }
        }

        private void RecalculateMaxXP()
        {
            // Scaling progression formula: Level 1=100, Level 2=150, Level 3=220, Level 4=300...
            MaxXP = Mathf.RoundToInt(100f * Mathf.Pow(CurrentLevel, 1.25f));
        }

        private void NotifyXPChanged()
        {
            EventBus.Raise(new PlayerXPChangedSignal
            {
                CurrentXP = CurrentXP,
                MaxXP = MaxXP
            });
        }
    }
}
