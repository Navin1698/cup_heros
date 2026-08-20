using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OrbRaiders.Core;
using OrbRaiders.Skills;

namespace OrbRaiders.UI
{
    public class SkillSelectionUI : MonoBehaviour
    {
        [Header("Modal UI Container")]
        [SerializeField] private GameObject modalPanel;

        [Header("Skill Cards")]
        [SerializeField] private Transform cardContainer;
        [SerializeField] private GameObject cardPrefab;

        private List<SkillDefinition> currentOptions = new List<SkillDefinition>();

        private void OnEnable()
        {
            EventBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        private void OnGameStateChanged(GameStateChangedSignal sig)
        {
            if (sig.NewState == GameState.SkillSelection)
            {
                ShowSkillSelection();
            }
            else
            {
                if (modalPanel != null) modalPanel.SetActive(false);
            }
        }

        public void ShowSkillSelection()
        {
            if (modalPanel != null) modalPanel.SetActive(true);

            currentOptions = SkillManager.Instance != null ? SkillManager.Instance.GetRandomSkills(3) : new List<SkillDefinition>();

            // Populate UI Cards
            if (cardContainer != null)
            {
                foreach (Transform child in cardContainer)
                {
                    Destroy(child.gameObject);
                }

                foreach (var skill in currentOptions)
                {
                    CreateSkillCardUI(skill);
                }
            }
        }

        private void CreateSkillCardUI(SkillDefinition skill)
        {
            GameObject card = cardPrefab != null ? Instantiate(cardPrefab, cardContainer) : CreateFallbackCardUI(skill);

            var button = card.GetComponent<Button>();
            if (button == null) button = card.AddComponent<Button>();

            button.onClick.AddListener(() => OnSkillSelected(skill));
        }

        private GameObject CreateFallbackCardUI(SkillDefinition skill)
        {
            GameObject cardObj = new GameObject($"Card_{skill.displayName}");
            cardObj.transform.SetParent(cardContainer, false);

            var image = cardObj.AddComponent<Image>();
            image.color = GetRarityColor(skill.rarity);

            var textObj = new GameObject("Text");
            textObj.transform.SetParent(cardObj.transform, false);
            var tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = $"<b>{skill.displayName}</b>\n<size=14>{skill.rarity}</size>\n\n{skill.description}";
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontSize = 18;

            return cardObj;
        }

        private void OnSkillSelected(SkillDefinition skill)
        {
            Debug.Log($"[SkillSelectionUI] Selected skill: {skill.displayName}");

            if (Player.PlayerController.Instance != null && Player.PlayerController.Instance.Skills != null)
            {
                Player.PlayerController.Instance.Skills.ApplySkill(skill);
            }

            if (Services.HapticManager.Instance != null)
            {
                Services.HapticManager.Instance.TriggerMedium();
            }

            // Resume Battle
            GameStateManager.Instance?.ChangeState(GameState.Battle);
        }

        private Color GetRarityColor(SkillRarity rarity)
        {
            switch (rarity)
            {
                case SkillRarity.Rare: return new Color(0.1f, 0.6f, 1.0f);
                case SkillRarity.Epic: return new Color(0.6f, 0.2f, 1.0f);
                case SkillRarity.Legendary: return new Color(1.0f, 0.6f, 0.0f);
                case SkillRarity.Mythic: return new Color(1.0f, 0.1f, 0.4f);
                default: return new Color(0.2f, 0.8f, 0.4f);
            }
        }
    }
}
