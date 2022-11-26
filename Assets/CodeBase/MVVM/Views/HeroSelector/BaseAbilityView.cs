using CodeBase.Domain.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.MVVM.Views.HeroSelector
{
    public class BaseAbilityView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _abilityViewText;
        [SerializeField] private Image _abilityIcon;

        public void Render(HeroData heroData)
        {
            _abilityViewText.text = heroData.InitialAbilityConfig.UpgradeViewData.Name;
            _abilityIcon.sprite = heroData.InitialAbilityConfig.UpgradeViewData.Icon;
        }
    }
}