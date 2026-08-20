using UnityEngine;
using OrbRaiders.Save;

namespace OrbRaiders.Progression
{
    public class ProgressionManager : MonoBehaviour
    {
        public static ProgressionManager Instance { get; private set; }

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

        public int GetUpgradeCost(int currentLevel)
        {
            return 250 + currentLevel * 200;
        }

        public bool UpgradeAttack()
        {
            var save = SaveManager.Instance.CurrentData;
            int cost = GetUpgradeCost(save.BonusAttackLevel);
            if (save.Gold >= cost)
            {
                save.Gold -= cost;
                save.BonusAttackLevel++;
                SaveManager.Instance.Save();
                return true;
            }
            return false;
        }

        public bool UpgradeHP()
        {
            var save = SaveManager.Instance.CurrentData;
            int cost = GetUpgradeCost(save.BonusHPLevel);
            if (save.Gold >= cost)
            {
                save.Gold -= cost;
                save.BonusHPLevel++;
                SaveManager.Instance.Save();
                return true;
            }
            return false;
        }

        public bool UpgradeMoveSpeed()
        {
            var save = SaveManager.Instance.CurrentData;
            int cost = GetUpgradeCost(save.BonusMoveSpeedLevel);
            if (save.Gold >= cost)
            {
                save.Gold -= cost;
                save.BonusMoveSpeedLevel++;
                SaveManager.Instance.Save();
                return true;
            }
            return false;
        }

        public void AddGold(int amount)
        {
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.CurrentData.Gold += amount;
                SaveManager.Instance.Save();
            }
        }
    }
}
