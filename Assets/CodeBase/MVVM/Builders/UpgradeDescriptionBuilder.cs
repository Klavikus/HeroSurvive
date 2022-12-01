using System;
using System.Collections.Generic;
using System.Text;
using CodeBase.Configs;
using CodeBase.Domain.Data;
using CodeBase.Domain.Enums;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.MVVM.Builders
{
    public class UpgradeDescriptionBuilder
    {
        private readonly IReadOnlyDictionary<BaseProperty, MainPropertyViewData> _propertiesData;
        private readonly ColorConfigSO _colorsConfig;

        public UpgradeDescriptionBuilder(ConfigurationProvider configurationProvider)
        {
            _colorsConfig = configurationProvider.ColorsConfig;
            _propertiesData = configurationProvider.BasePropertiesConfig.GetPropertyViewsData();
        }

        //TODO Need refactoring
        public string BuildDescriptionText(UpgradeData upgradeData, int currentLevel)
        {
            StringBuilder stringBuilder = new StringBuilder();

            bool isMaxLevel = upgradeData.Upgrades.Length == currentLevel;

            if (isMaxLevel)
                currentLevel -= 1;

            AdditionalHeroProperty[] additionalHeroProperties =
                upgradeData.Upgrades[currentLevel].AdditionalHeroProperties;

            foreach (AdditionalHeroProperty additionalHeroProperty in additionalHeroProperties)
            {
                //TODO add this » to properties
                MainPropertyViewData property = _propertiesData[additionalHeroProperty.BaseProperty];

                stringBuilder.AppendLine(
                    isMaxLevel == false
                        ? $"{property.FullName} » {GetAdditionalPropertyColorValue(additionalHeroProperty)}"
                        : $"{property.FullName}{GetAdditionalPropertyColorValue(additionalHeroProperty)}");
            }

            return stringBuilder.ToString();
        }

        private string GetAdditionalPropertyColorValue(AdditionalHeroProperty additionalHeroProperty)
        {
            MainPropertyViewData propertyData = _propertiesData[additionalHeroProperty.BaseProperty];


            bool isIncreasing = additionalHeroProperty.Value > 0;
            Color valueHighlight = !(propertyData.IsIncreaseGood ^ isIncreasing)
                ? _colorsConfig.GoodPropertyColor
                : _colorsConfig.BadPropertyColor;


            string valuePrefix = isIncreasing ? "+" : "";
            string hexRGBA = ColorUtility.ToHtmlStringRGBA(valueHighlight);
            return $"<color=#{hexRGBA}> {valuePrefix}{additionalHeroProperty.Value}{propertyData.Postfix}</color>";
        }

        public string BuildBuyPriceText(UpgradesLevelData[] upgradesData, int currentLevel, bool cantUpgrade,
            bool isMaxLevel)
        {
            string hexRGBA = ColorUtility.ToHtmlStringRGBA(_colorsConfig.CurrencyColor);

            if (isMaxLevel)
                return $"<color=#{hexRGBA}>Max Level</color>";

            if (cantUpgrade == false)
                return $"<color=#{hexRGBA}>-{upgradesData[currentLevel].Price}</color>";

            hexRGBA = ColorUtility.ToHtmlStringRGBA(_colorsConfig.CurrencyColorCantPay);

            return $"<color=#{hexRGBA}>-{upgradesData[currentLevel].Price}</color>";
        }

        public string BuildSellPriceText(UpgradesLevelData[] upgradesData, int currentLevel, bool canReset)
        {
            string hexRGBA = ColorUtility.ToHtmlStringRGBA(_colorsConfig.CurrencyColor);

            if (canReset == false)
                return String.Empty;

            int resetCurrencyIncome = 0;

            for (int i = 0; i < currentLevel; i++)
                resetCurrencyIncome += upgradesData[i].Price;

            return $"<color=#{hexRGBA}>+{resetCurrencyIncome}</color>";
        }

        //TODO: Нужно ли так именовать параметры метода
        public string GetPropertyTextDescription(MainPropertyViewData viewData, float propertyValue)
        {
            bool isSigned = viewData.IsSigned;
            bool isIncreasing = propertyValue > 0;
            Color valueHighlight = !(viewData.IsIncreaseGood ^ isIncreasing)
                ? _colorsConfig.GoodPropertyColor
                : _colorsConfig.BadPropertyColor;
            string prefix = string.Empty;

            if (isSigned)
                prefix = propertyValue >= 0 ? "+" : String.Empty;

            string hexRGBA = ColorUtility.ToHtmlStringRGBA(valueHighlight);
            return $"<color=#{hexRGBA}> {prefix} {propertyValue} {viewData.Postfix}</color>";
        }

        public string GetCurrencyText(int amount) =>
            $"<color=#{ColorUtility.ToHtmlStringRGBA(_colorsConfig.CurrencyColor)}>{amount}</color>";

        public string GetAbilityUpgradeDescription(AbilityUpgradeData abilityUpgradeData)
        {
            if (abilityUpgradeData.IsFirstAbilityGain == false)
            {
                string hexRGBA = ColorUtility.ToHtmlStringRGBA(_colorsConfig.GoodPropertyColor);
                return $"<color=#{hexRGBA}>New ability!</color>";
            }

            string nameString = _propertiesData[abilityUpgradeData.PropertyType].FullName;
            string valueString = GetPropertyTextDescription(_propertiesData[abilityUpgradeData.PropertyType],
                abilityUpgradeData.Value);
            return $"{nameString} {valueString}";
        }
    }
}