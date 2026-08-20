using System;
using System.Collections.Generic;

namespace OrbRaiders.Save
{
    [Serializable]
    public class SaveData
    {
        public int Gold = 0;
        public int Crystals = 0;
        public int PlayerAccountLevel = 1;
        public int HighestWorldUnlocked = 1;
        public int HighestLevelCompleted = 0;

        public string SelectedHeroId = "Nova";
        public List<string> UnlockedHeroIds = new List<string> { "Nova" };

        // Permanent Upgrades (Gold upgrades)
        public int BonusAttackLevel = 0;
        public int BonusHPLevel = 0;
        public int BonusArmorLevel = 0;
        public int BonusCritChanceLevel = 0;
        public int BonusCritDamageLevel = 0;
        public int BonusAttackSpeedLevel = 0;
        public int BonusMoveSpeedLevel = 0;

        // Settings
        public bool MusicEnabled = true;
        public bool SFXEnabled = true;
        public bool HapticsEnabled = true;
        public int GraphicsQualityTier = 2; // 0=Low, 1=Med, 2=High, 3=Ultra
    }

    public interface ISaveService
    {
        SaveData CurrentData { get; }
        void Save();
        void Load();
        void ResetProgress();
    }
}
