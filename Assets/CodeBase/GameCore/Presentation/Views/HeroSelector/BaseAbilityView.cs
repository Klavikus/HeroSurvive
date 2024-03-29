using CodeBase.GameCore.Domain.Data;
using CodeBase.GameCore.Infrastructure.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GameCore.Presentation.Views.HeroSelector
{
    public class BaseAbilityView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _abilityViewText;
        [SerializeField] private Image _abilityIcon;

        private ITranslationService _translationService;

        public void Initialize()
        {
            _translationService = AllServices.Container.AsSingle<ITranslationService>();
        }

        public void Render(HeroData heroData)
        {
            _abilityViewText.text =
                _translationService.GetLocalizedText(heroData.InitialAbilityConfig.UpgradeViewData.TranslatableName);
            _abilityIcon.sprite = heroData.InitialAbilityConfig.UpgradeViewData.Icon;
        }
    }
}