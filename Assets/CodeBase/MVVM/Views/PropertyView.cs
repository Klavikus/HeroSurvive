using System.Collections.Generic;
using CodeBase.Enums;
using CodeBase.MVVM.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.MVVM.Views
{
    public class PropertyView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _shortName;
        [SerializeField] private TMP_Text _value;

        private MainPropertyViewData _viewData;
    
        private MainPropertiesViewModel _viewModel;

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

        public void Initialize(MainPropertiesViewModel viewModel, MainPropertyViewData viewData)
        {
            _viewModel = viewModel;
            _viewData = viewData;
            _image.sprite = _viewData.Icon;
            _shortName.text = _viewData.ShortName;
            _value.text = $"{_viewData.Prefix} {_viewData.Value} {_viewData.Suffix}";
            _viewModel.PropertiesChanged += Render;
        }

        private void Render(IReadOnlyDictionary<BaseProperty, float> mainProperties) => 
            _value.text = $"{_viewData.Prefix} {mainProperties[_viewData.BaseProperty]} {_viewData.Suffix}";
    }
}