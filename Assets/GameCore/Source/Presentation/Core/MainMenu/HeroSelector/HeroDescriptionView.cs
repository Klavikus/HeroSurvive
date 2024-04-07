using GameCore.Source.Presentation.Api.MainMenu.HeroSelector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Core.MainMenu.HeroSelector
{
    public class HeroDescriptionView : MonoBehaviour, IHeroDescriptionView
    {
        [SerializeField] private Image _heroPreview;
        [SerializeField] private TMP_Text _heroName;
        [SerializeField] private TMP_Text _description;

        public void Fill(Sprite icon, string heroName, string description)
        {
            _heroPreview.sprite = icon;
            _heroName.text = heroName;
            _description.text = description;
        }
    }
}