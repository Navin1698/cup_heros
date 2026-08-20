using UnityEngine;
using UnityEngine.UI;
using OrbRaiders.Core;

namespace OrbRaiders.UI
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button homeButton;

        private void OnEnable()
        {
            EventBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        private void Start()
        {
            if (resumeButton != null) resumeButton.onClick.AddListener(OnResumeClicked);
            if (restartButton != null) restartButton.onClick.AddListener(OnRestartClicked);
            if (homeButton != null) homeButton.onClick.AddListener(OnHomeClicked);
        }

        private void OnGameStateChanged(GameStateChangedSignal sig)
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(sig.NewState == GameState.Pause);
            }
        }

        private void OnResumeClicked()
        {
            GameStateManager.Instance?.ChangeState(GameState.Battle);
        }

        private void OnRestartClicked()
        {
            GameStateManager.Instance?.ChangeState(GameState.Battle);
        }

        private void OnHomeClicked()
        {
            GameStateManager.Instance?.ChangeState(GameState.MainMenu);
        }
    }
}
