using CodeBase.MVVM.Builders;
using CodeBase.MVVM.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.MVVM.Views.HeroSelector
{
    public class HeroSelectorView : MonoBehaviour
    {
        [SerializeField] private Canvas _baseCanvas;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private RectTransform _heroViewsContainer;
        [SerializeField] private RectTransform _propertiesViewContainer;
        [SerializeField] private HeroDescriptionView _heroDescriptionView;
        [SerializeField] private BaseAbilityView _baseAbilityView;
        [SerializeField] private CurrencyView _currencyView;

        private HeroSelectorViewModel _heroSelectorViewModel;

        private void OnEnable()
        {
            if (_heroSelectorViewModel == null)
                return;

            SubscribeToViewModel();
        }

        private void OnDisable()
        {
            if (_heroSelectorViewModel == null)
                return;

            UnsubscribeToViewModel();
        }

        public void Initialize(HeroSelectorViewModel heroSelectorViewModel,
            CurrencyViewModel currencyViewModel,
            UpgradeDescriptionBuilder descriptionBuilder)
        {
            _heroSelectorViewModel = heroSelectorViewModel;
            SubscribeToViewModel();

            _currencyView.Initialize(currencyViewModel, descriptionBuilder);
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
            _closeButton.onClick.AddListener(OnCloseButtonClicked);

            if (_heroSelectorViewModel.CurrentSelectedHeroData != null)
                _heroDescriptionView.Render(_heroSelectorViewModel.CurrentSelectedHeroData);

            Hide();
        }


        public void SetHeroViews(HeroView[] heroViews)
        {
            foreach (HeroView heroView in heroViews)
            {
                heroView.transform.SetParent(_heroViewsContainer);
                heroView.transform.localScale = Vector3.one;
            }

            heroViews[0].SelectAsInitial();
        }

        public void SetPropertyViews(PropertyView[] propertiesViews)
        {
            foreach (PropertyView propertyView in propertiesViews)
            {
                propertyView.transform.SetParent(_propertiesViewContainer);
                propertyView.transform.localScale = Vector3.one;
            }
        }

        private void SubscribeToViewModel()
        {
            _heroSelectorViewModel.HeroSelected += _heroDescriptionView.Render;
            _heroSelectorViewModel.HeroSelected += _baseAbilityView.Render;
            _heroSelectorViewModel.HeroSelectorEnabled += Show;
            _heroSelectorViewModel.HeroSelectorDisabled += Hide;
        }

        private void UnsubscribeToViewModel()
        {
            _heroSelectorViewModel.HeroSelected -= _heroDescriptionView.Render;
            _heroSelectorViewModel.HeroSelected -= _baseAbilityView.Render;
            _heroSelectorViewModel.HeroSelectorEnabled -= Show;
            _heroSelectorViewModel.HeroSelectorDisabled -= Hide;
        }


        private void Show() => _baseCanvas.enabled = true;

        private void Hide() => _baseCanvas.enabled = false;

        private void OnContinueButtonClicked() => _heroSelectorViewModel.Continue();

        private void OnCloseButtonClicked() => _heroSelectorViewModel.DisableHeroSelector();
    }
}