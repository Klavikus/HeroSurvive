using GameCore.Source.Presentation.Api.MainMenu.HeroSelector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Core.MainMenu.HeroSelector
{
    public class BaseAbilityView : MonoBehaviour, IBaseAbilityView
    {
        [SerializeField] private Image _abilityIcon;
        [SerializeField] private TMP_Text _abilityViewText;

        public void Fill(Sprite icon, string text)
        {
            _abilityIcon.sprite = icon;
            _abilityViewText.text = text;
        }
    }
}