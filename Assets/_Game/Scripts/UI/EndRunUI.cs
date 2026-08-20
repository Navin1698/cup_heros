using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OrbRaiders.Core;

namespace OrbRaiders.UI
{
    public class VictoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject victoryPanel;
        [SerializeField] private TextMeshProUGUI rewardsText;
        [SerializeField] private Button continueButton;
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
            if (continueButton != null) continueButton.onClick.AddListener(OnHomeClicked);
            if (homeButton != null) homeButton.onClick.AddListener(OnHomeClicked);
        }

        private void OnGameStateChanged(GameStateChangedSignal sig)
        {
            if (sig.NewState == GameState.Victory)
            {
                if (victoryPanel != null) victoryPanel.SetActive(true);

                int rewardGold = 1000;
                int rewardCrystals = 50;

                Progression.ProgressionManager.Instance?.AddGold(rewardGold);

                if (rewardsText != null)
                {
                    rewardsText.text = $"VICTORY!\n\nREWARDS:\n+ {rewardGold} GOLD\n+ {rewardCrystals} CRYSTALS";
                }
            }
            else
            {
                if (victoryPanel != null) victoryPanel.SetActive(false);
            }
        }

        private void OnHomeClicked()
        {
            GameStateManager.Instance?.ChangeState(GameState.MainMenu);
        }
    }

    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI statsText;
        [SerializeField] private Button retryButton;
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
            if (retryButton != null) retryButton.onClick.AddListener(OnRetryClicked);
            if (homeButton != null) homeButton.onClick.AddListener(OnHomeClicked);
        }

        private void OnGameStateChanged(GameStateChangedSignal sig)
        {
            if (sig.NewState == GameState.GameOver)
            {
                if (gameOverPanel != null) gameOverPanel.SetActive(true);

                int currentWave = Enemies.WaveManager.Instance != null ? Enemies.WaveManager.Instance.CurrentWave : 1;
                int earnedGold = 250 + currentWave * 50;

                Progression.ProgressionManager.Instance?.AddGold(earnedGold);

                if (statsText != null)
                {
                    statsText.text = $"RUN COMPLETE\n\nWAVE REACHED: {currentWave}\nREWARDS: +{earnedGold} GOLD";
                }
            }
            else
            {
                if (gameOverPanel != null) gameOverPanel.SetActive(false);
            }
        }

        private void OnRetryClicked()
        {
            GameStateManager.Instance?.ChangeState(GameState.Battle);
        }

        private void OnHomeClicked()
        {
            GameStateManager.Instance?.ChangeState(GameState.MainMenu);
        }
    }
}
