using CodeBase.Domain;
using CodeBase.Infrastructure;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CodeBase.Presentation
{
    public class HeroSelectorView : MonoBehaviour
    {
        [SerializeField] private Canvas _baseCanvas;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _userNameButton;
        [SerializeField] private RectTransform _heroViewsContainer;
        [SerializeField] private RectTransform _propertiesViewContainer;
        [SerializeField] private HeroDescriptionView _heroDescriptionView;
        [SerializeField] private BaseAbilityView _baseAbilityView;
        [SerializeField] private CurrencyView _currencyView;
        [SerializeField] private int _rowCount;
        [SerializeField] private int _colCount;
        
        private HeroSelectorViewModel _heroSelectorViewModel;
        private PlayerInputActions _playerInputActions;

        private int _currentSelectedHeroId;
        private bool _isInitialized;


        private void OnEnable()
        {
            if (_isInitialized == false)
                return;

            SubscribeToViewModel();
            SubscribeToInputActions();
        }

        private void OnDisable()
        {
            if (_isInitialized == false)
                return;

            UnsubscribeToViewModel();
            UnsubscribeToInputActions();
        }

        public void Initialize(HeroSelectorViewModel heroSelectorViewModel,
            CurrencyViewModel currencyViewModel,
            UpgradeDescriptionBuilder descriptionBuilder)
        {
            _heroSelectorViewModel = heroSelectorViewModel;

            _currencyView.Initialize(currencyViewModel, descriptionBuilder);
            _baseAbilityView.Initialize();
            _heroDescriptionView.Initialize();

            _continueButton.onClick.AddListener(OnContinueButtonClicked);
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            SubscribeToViewModel();

            _playerInputActions = new PlayerInputActions();
            SubscribeToInputActions();

            if (_heroSelectorViewModel.CurrentSelectedHeroData != null)
                _heroDescriptionView.Render(_heroSelectorViewModel.CurrentSelectedHeroData);

            _isInitialized = true;

            Hide();
        }

        public void SetHeroViews(HeroView[] heroViews)
        {
            foreach (HeroView heroView in heroViews)
            {
                heroView.transform.SetParent(_heroViewsContainer);
                heroView.transform.localScale = Vector3.one;
            }

            _heroSelectorViewModel.SelectHero(_currentSelectedHeroId);
        }

        public void SetPropertyViews(PropertyView[] propertiesViews)
        {
            foreach (PropertyView propertyView in propertiesViews)
            {
                propertyView.transform.SetParent(_propertiesViewContainer);
                propertyView.transform.localScale = Vector3.one;
            }
        }

        private void OnHeroSelected(HeroData heroData)
        {
            _currentSelectedHeroId = _heroSelectorViewModel.CurrentSelectedHeroId;
            _heroDescriptionView.Render(heroData);
            _baseAbilityView.Render(heroData);
        }

        private void SubscribeToViewModel()
        {
            _heroSelectorViewModel.HeroSelected += OnHeroSelected;
            _heroSelectorViewModel.HeroSelectorEnabled += Show;
            _heroSelectorViewModel.HeroSelectorDisabled += Hide;
        }

        private void UnsubscribeToViewModel()
        {
            _heroSelectorViewModel.HeroSelected -= OnHeroSelected;
            _heroSelectorViewModel.HeroSelectorEnabled -= Show;
            _heroSelectorViewModel.HeroSelectorDisabled -= Hide;
        }

        private void SubscribeToInputActions()
        {
            _playerInputActions.UI.Apply.performed += OnApplyPerformed;
            _playerInputActions.UI.Cancel.performed += OnCancelPerformed;
            _playerInputActions.UI.ScrollLeft.performed += OnScrollLeftPerformed;
            _playerInputActions.UI.ScrollRight.performed += OnScrollRightPerformed;
            _playerInputActions.UI.ScrollUp.performed += OnScrollUpPerformed;
            _playerInputActions.UI.ScrollDown.performed += OnScrollDownPerformed;
        }

        private void UnsubscribeToInputActions()
        {
            _playerInputActions.UI.Apply.performed -= OnApplyPerformed;
            _playerInputActions.UI.Cancel.performed -= OnCancelPerformed;
            _playerInputActions.UI.ScrollLeft.performed -= OnScrollLeftPerformed;
            _playerInputActions.UI.ScrollRight.performed -= OnScrollRightPerformed;
            _playerInputActions.UI.ScrollUp.performed -= OnScrollUpPerformed;
            _playerInputActions.UI.ScrollDown.performed -= OnScrollDownPerformed;
        }

        private void Show()
        {
            _baseCanvas.enabled = true;
            _playerInputActions.Enable();
        }

        private void Hide()
        {
            _baseCanvas.enabled = false;
            _playerInputActions.Disable();
        }

        private void OnContinueButtonClicked() => _heroSelectorViewModel.Continue();

        private void OnCloseButtonClicked() => _heroSelectorViewModel.DisableHeroSelector();

        private void OnApplyPerformed(InputAction.CallbackContext context) => OnContinueButtonClicked();

        private void OnCancelPerformed(InputAction.CallbackContext context) => OnCloseButtonClicked();

        private void OnScrollUpPerformed(InputAction.CallbackContext context) =>
            _heroSelectorViewModel.HandleMove(0, -1, _rowCount, _colCount);

        private void OnScrollDownPerformed(InputAction.CallbackContext context) =>
            _heroSelectorViewModel.HandleMove(0, 1, _rowCount, _colCount);

        private void OnScrollLeftPerformed(InputAction.CallbackContext context) =>
            _heroSelectorViewModel.HandleMove(-1, 0, _rowCount, _colCount);

        private void OnScrollRightPerformed(InputAction.CallbackContext context) =>
            _heroSelectorViewModel.HandleMove(1, 0, _rowCount, _colCount);
    }
}