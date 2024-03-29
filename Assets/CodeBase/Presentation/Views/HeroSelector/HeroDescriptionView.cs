using CodeBase.Domain.Data;
using CodeBase.Infrastructure.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Presentation.Views.HeroSelector
{
    public class HeroDescriptionView : MonoBehaviour
    {
        [SerializeField] private Image _heroPreview;
        [SerializeField] private TMP_Text _heroName;
        [SerializeField] private TMP_Text _description;

        private ITranslationService _translationService;

        public void Initialize() => _translationService = AllServices.Container.AsSingle<ITranslationService>();

        public void Render(HeroData heroData)
        {
            _heroPreview.sprite = heroData.Sprite;
            _heroName.text = _translationService.GetLocalizedText(heroData.TranslatableName);
            _description.text = _translationService.GetLocalizedText(heroData.TranslatableDescriptions);
        }
    }
}