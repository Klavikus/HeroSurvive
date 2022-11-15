using System.Collections.Generic;
using CodeBase.Domain.Data;
using CodeBase.Domain.Enums;
using CodeBase.MVVM.Builders;
using CodeBase.MVVM.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.MVVM.Views.HeroSelector
{
    public class PropertyView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _shortName;
        [SerializeField] private TMP_Text _value;

        private MainPropertyViewData _viewData;

        private MainPropertiesViewModel _viewModel;
        private UpgradeDescriptionBuilder _upgradeDescriptionBuilder;

        private void OnEnable()
        {
            if (_viewModel == null)
                return;

            _viewModel.PropertiesChanged += Render;
        }

        private void OnDisable()
        {
            if (_viewModel == null)
                return;

            _viewModel.PropertiesChanged -= Render;
        }

        public void Initialize(MainPropertiesViewModel mainPropertiesViewModel, MainPropertyViewData viewData,
            UpgradeDescriptionBuilder upgradeDescriptionBuilder)
        {
            _viewModel = mainPropertiesViewModel;
            _viewData = viewData;
            _upgradeDescriptionBuilder = upgradeDescriptionBuilder;
            _image.sprite = _viewData.Icon;
            _shortName.text = _viewData.ShortName;
            _value.text = $"{_viewData.Prefix} {_viewData.Value} {_viewData.Postfix}";
            Render(mainPropertiesViewModel.BaseProperties);
            _viewModel.PropertiesChanged += Render;
        }

        private void Render(IReadOnlyDictionary<BaseProperty, float> mainProperties)
        {
            _value.text =
                _upgradeDescriptionBuilder.GetPropertyTextDescription(_viewData,
                    mainProperties[_viewData.BaseProperty]);
        }
    }
}