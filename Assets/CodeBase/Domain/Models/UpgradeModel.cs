using System;
using System.Linq;
using CodeBase.Domain;
using UnityEngine;

namespace CodeBase.Domain
{
    [Serializable]
    public class UpgradeModel
    {
        public readonly MainProperties Properties;

        public UpgradeModel(UpgradeData data)
        {
            Data = data;
            Properties = new MainProperties();
            Recalculate();
        }

        private void Recalculate()
        {
            foreach (BaseProperty propertyType in Enum.GetValues(typeof(BaseProperty)).Cast<BaseProperty>())
                Properties.UpdateProperty(propertyType, 0);
            foreach (AdditionalHeroProperty property in GetCurrentAdditionalProperties())
                Properties.UpdateProperty(property.BaseProperty, property.Value);
        }

        [field: SerializeField] public UpgradeData Data { get; }
        [field: SerializeField] public int CurrentLevel { get; private set; }

        public Action<UpgradeModel> LevelChanged;

        private AdditionalHeroProperty[] GetCurrentAdditionalProperties()
        {
            if (CurrentLevel == 0)
                return new[] {new AdditionalHeroProperty(BaseProperty.MaxHealth, 0)};

            UpgradesLevelData result = Data.Upgrades[CurrentLevel - 1];

            return result.AdditionalHeroProperties;
        }

        public int GetUpgradeCost()
        {
            if (CurrentLevel == Data.Upgrades.Length)
                return 0;

            return Data.Upgrades[CurrentLevel].Price;
        }

        public int GetResetCost()
        {
            if (CurrentLevel == 0)
                return 0;

            int result = 0;

            for (int i = 0; i < CurrentLevel; i++)
                result += Data.Upgrades[i].Price;

            return result;
        }

        public void SetLevel(int newLevel)
        {
            CurrentLevel = newLevel;
            Recalculate();
            LevelChanged?.Invoke(this);
        }

        public void LevelUp()
        {
            if (CurrentLevel == Data.Upgrades.Length)
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(CurrentLevel)} should be lower or equal to {nameof(Data.Upgrades.Length)}");
            }

            CurrentLevel++;
            Recalculate();
            LevelChanged?.Invoke(this);
        }

        public void ResetLevel()
        {
            CurrentLevel = 0;
            Recalculate();
            LevelChanged?.Invoke(this);
        }
    }
}