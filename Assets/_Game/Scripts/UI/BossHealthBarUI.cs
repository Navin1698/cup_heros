using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OrbRaiders.Core;

namespace OrbRaiders.UI
{
    public class BossHealthBarUI : MonoBehaviour
    {
        [SerializeField] private GameObject bossBarContainer;
        [SerializeField] private TextMeshProUGUI bossNameText;
        [SerializeField] private Slider hpSlider;
        [SerializeField] private TextMeshProUGUI phaseText;

        private void OnEnable()
        {
            EventBus.Subscribe<BossSpawnedSignal>(OnBossSpawned);
            EventBus.Subscribe<BossHPChangedSignal>(OnBossHPChanged);
            EventBus.Subscribe<BossDefeatedSignal>(OnBossDefeated);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<BossSpawnedSignal>(OnBossSpawned);
            EventBus.Unsubscribe<BossHPChangedSignal>(OnBossHPChanged);
            EventBus.Unsubscribe<BossDefeatedSignal>(OnBossDefeated);
        }

        private void OnBossSpawned(BossSpawnedSignal sig)
        {
            if (bossBarContainer != null) bossBarContainer.SetActive(true);
            if (bossNameText != null) bossNameText.text = sig.BossName;
            if (hpSlider != null)
            {
                hpSlider.maxValue = sig.MaxHP;
                hpSlider.value = sig.MaxHP;
            }
            if (phaseText != null) phaseText.text = "PHASE 1";
        }

        private void OnBossHPChanged(BossHPChangedSignal sig)
        {
            if (hpSlider != null)
            {
                hpSlider.value = sig.CurrentHP;
            }
            if (phaseText != null)
            {
                phaseText.text = $"PHASE {sig.CurrentPhase}";
            }
        }

        private void OnBossDefeated(BossDefeatedSignal sig)
        {
            if (bossBarContainer != null) bossBarContainer.SetActive(false);
        }
    }
}
