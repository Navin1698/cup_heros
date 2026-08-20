using UnityEngine;
using System;

namespace OrbRaiders.Core
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }

        public GameState CurrentState { get; private set; } = GameState.Boot;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void ChangeState(GameState newState)
        {
            if (CurrentState == newState) return;

            GameState previousState = CurrentState;
            CurrentState = newState;

            // Handle time scale for pause/modal states
            if (newState == GameState.Pause || newState == GameState.SkillSelection)
            {
                Time.timeScale = 0f;
            }
            else if (previousState == GameState.Pause || previousState == GameState.SkillSelection)
            {
                Time.timeScale = 1f;
            }

            Debug.Log($"[GameStateManager] Transition: {previousState} -> {newState}");

            EventBus.Raise(new GameStateChangedSignal
            {
                PreviousState = previousState,
                NewState = newState
            });
        }

        public bool IsState(GameState state)
        {
            return CurrentState == state;
        }
    }
}
