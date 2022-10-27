using CodeBase.MVVM.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.MVVM.Views
{
    public class BaseAbilityView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _baseText;
        [SerializeField] private TMP_Text _abilityViewText;
        [SerializeField] private Image _abilityIcon;

        private BaseAbilityViewModel _baseAbilityViewModel;

        private void OnEnable()
        {
            if (_baseAbilityViewModel == null)
                return;

            _baseAbilityViewModel.Changed += OnBaseAbilityChanged;
        }

        private void OnDisable()
        {
            if (_baseAbilityViewModel == null)
                return;

            _baseAbilityViewModel.Changed -= OnBaseAbilityChanged;
        }

        public void Initialize(BaseAbilityViewModel baseAbilityViewModel)
        {
            _baseAbilityViewModel = baseAbilityViewModel;
            _baseAbilityViewModel.Changed += OnBaseAbilityChanged;
        }

        private void OnBaseAbilityChanged(HeroData heroData)
        {
            _baseText.text = "Base Ability:";
            _abilityViewText.text = heroData.AbilityViewData.Name;
            _abilityIcon.sprite = heroData.AbilityViewData.Icon;
        }
    }
}