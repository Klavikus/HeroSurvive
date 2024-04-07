using GameCore.Source.Domain.Data;

namespace GameCore.Source.Controllers.Api.Factories
{
    public interface IUpgradeDescriptionBuilder
    {
        string BuildDescriptionText(UpgradeData upgradeData, int currentLevel);

        string BuildBuyPriceText(UpgradesLevelData[] upgradesData, int currentLevel, bool cantUpgrade,
            bool isMaxLevel);

        string BuildSellPriceText(UpgradesLevelData[] upgradesData, int currentLevel, bool canReset);
        string GetPropertyTextDescription(MainPropertyViewData viewData, float propertyValue);
        string GetCurrencyText(int amount);
        string GetAbilityUpgradeDescription(AbilityUpgradeData abilityUpgradeData);
    }
}