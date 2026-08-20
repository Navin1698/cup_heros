using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OrbRaiders.Core;

namespace OrbRaiders.UI
{
    public class BattleUI : MonoBehaviour
    {
        [Header("UI Controls")]
        [SerializeField] private Slider hpBar;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private Slider xpBar;
        [SerializeField] private TextMeshProUGUI xpText;
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private Button ultimateButton;
        [SerializeField] private Image ultimateGlowImage;
        [SerializeField] private Button pauseButton;

        private void OnEnable()
        {
            EventBus.Subscribe<PlayerHPChangedSignal>(OnHPChanged);
            EventBus.Subscribe<PlayerXPChangedSignal>(OnXPChanged);
            EventBus.Subscribe<WaveChangedSignal>(OnWaveChanged);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<PlayerHPChangedSignal>(OnHPChanged);
            EventBus.Unsubscribe<PlayerXPChangedSignal>(OnXPChanged);
            EventBus.Unsubscribe<WaveChangedSignal>(OnWaveChanged);
        }

        private void Start()
        {
            if (ultimateButton != null)
            {
                ultimateButton.onClick.AddListener(OnUltimateClicked);
            }

            if (pauseButton != null)
            {
                pauseButton.onClick.AddListener(OnPauseClicked);
            }
        }

        private void Update()
        {
            if (Player.PlayerController.Instance != null && Player.PlayerController.Instance.Combat != null)
            {
                var combat = Player.PlayerController.Instance.Combat;
                if (ultimateGlowImage != null)
                {
                    ultimateGlowImage.enabled = combat.IsUltimateReady;
                }
            }
        }

        private void OnHPChanged(PlayerHPChangedSignal sig)
        {
            if (hpBar != null)
            {
                hpBar.maxValue = sig.MaxHP;
                hpBar.value = sig.CurrentHP;
            }
            if (hpText != null)
            {
                hpText.text = $"{Mathf.CeilToInt(sig.CurrentHP)} / {Mathf.CeilToInt(sig.MaxHP)}";
            }
        }

        private void OnXPChanged(PlayerXPChangedSignal sig)
        {
            if (xpBar != null)
            {
                xpBar.maxValue = sig.MaxXP;
                xpBar.value = sig.CurrentXP;
            }
            if (xpText != null)
            {
                xpText.text = $"{sig.CurrentXP} / {sig.MaxXP} XP";
            }
        }

        private void OnWaveChanged(WaveChangedSignal sig)
        {
            if (waveText != null)
            {
                waveText.text = $"WAVE {sig.CurrentWave:D2} / {sig.TotalWaves:D2}";
            }
        }

        private void OnUltimateClicked()
        {
            if (Player.PlayerController.Instance != null && Player.PlayerController.Instance.Combat != null)
            {
                Player.PlayerController.Instance.Combat.TriggerUltimate();
            }
        }

        private void OnPauseClicked()
        {
            GameStateManager.Instance?.ChangeState(GameState.Pause);
        }
    }
}
