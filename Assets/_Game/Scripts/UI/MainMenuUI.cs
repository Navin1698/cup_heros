using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OrbRaiders.Core;
using OrbRaiders.Save;

namespace OrbRaiders.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Menu Views")]
        [SerializeField] private GameObject menuPanel;

        [Header("Header Currencies")]
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI crystalText;

        [Header("Navigation Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button upgradeAttackButton;
        [SerializeField] private Button upgradeHPButton;

        private void OnEnable()
        {
            EventBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
            UpdateUI();
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        private void Start()
        {
            if (playButton != null) playButton.onClick.AddListener(OnPlayClicked);
            if (upgradeAttackButton != null) upgradeAttackButton.onClick.AddListener(OnUpgradeAttackClicked);
            if (upgradeHPButton != null) upgradeHPButton.onClick.AddListener(OnUpgradeHPClicked);

            UpdateUI();
        }

        private void OnGameStateChanged(GameStateChangedSignal sig)
        {
            if (menuPanel != null)
            {
                menuPanel.SetActive(sig.NewState == GameState.MainMenu);
            }

            if (sig.NewState == GameState.MainMenu)
            {
                UpdateUI();
            }
        }

        public void UpdateUI()
        {
            if (SaveManager.Instance != null)
            {
                var save = SaveManager.Instance.CurrentData;
                if (goldText != null) goldText.text = $"{save.Gold:N0}";
                if (crystalText != null) crystalText.text = $"{save.Crystals:N0}";
            }
        }

        private void OnPlayClicked()
        {
            GameStateManager.Instance?.ChangeState(GameState.Battle);
        }

        private void OnUpgradeAttackClicked()
        {
            if (Progression.ProgressionManager.Instance != null && Progression.ProgressionManager.Instance.UpgradeAttack())
            {
                UpdateUI();
            }
        }

        private void OnUpgradeHPClicked()
        {
            if (Progression.ProgressionManager.Instance != null && Progression.ProgressionManager.Instance.UpgradeHP())
            {
                UpdateUI();
            }
        }
    }
}
