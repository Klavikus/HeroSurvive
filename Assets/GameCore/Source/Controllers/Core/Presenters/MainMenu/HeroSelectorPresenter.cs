using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Api.ViewModels;
using GameCore.Source.Controllers.Core.Presenters.Base;
using GameCore.Source.Controllers.Core.WindowFsms.Windows;
using GameCore.Source.Domain.Data;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Presentation.Api.GameLoop;
using Modules.Common.WindowFsm.Runtime.Abstract;
using UnityEngine.InputSystem;

namespace GameCore.Source.Controllers.Core.Presenters.MainMenu
{
    public class HeroSelectorPresenter : BaseWindowPresenter<HeroSelectorWindow>
    {
        private readonly IHeroSelectorView _view;
        private readonly IHeroSelectorViewModel _viewModel;
        private readonly ILocalizationService _localizationService;

        private readonly PlayerInputActions _playerInputActions;

        private readonly int _rowCount;
        private readonly int _colCount;
        private readonly IGameStateMachine _gameStateMachine;

        public HeroSelectorPresenter(
            IWindowFsm windowFsm,
            IHeroSelectorView view,
            IHeroSelectorViewModel viewModel,
            ILocalizationService localizationService,
            IGameStateMachine gameStateMachine
        ) : base(windowFsm, view.Show, view.Hide)
        {
            _view = view;
            _viewModel = viewModel;
            _localizationService = localizationService;
            _gameStateMachine = gameStateMachine;
            _playerInputActions = new PlayerInputActions();
            _rowCount = view.RowCount;
            _colCount = view.ColCount;
        }

        protected override void OnAfterEnable()
        {
            _view.Initialize();
        }

        protected override void OnAfterDisable()
        {
            _view.CloseButton.Clicked -= OnCloseButtonClicked;
            _view.ContinueButton.Clicked -= OnContinueButtonClicked;

            UnsubscribeToViewModel();
            UnsubscribeToInputActions();
        }

        protected override void OnAfterOpened()
        {
            _view.CloseButton.Clicked += OnCloseButtonClicked;
            _view.ContinueButton.Clicked += OnContinueButtonClicked;

            SubscribeToViewModel();
            SubscribeToInputActions();

            if (_viewModel.CurrentSelectedHeroData != null)
                FillView(_viewModel.CurrentSelectedHeroData);
        }

        protected override void OnAfterClosed()
        {
            _view.CloseButton.Clicked -= OnCloseButtonClicked;
            _view.ContinueButton.Clicked -= OnContinueButtonClicked;

            UnsubscribeToViewModel();
            UnsubscribeToInputActions();
        }

        private void SubscribeToViewModel()
        {
            _viewModel.HeroSelected += FillView;
        }

        private void UnsubscribeToViewModel()
        {
            _viewModel.HeroSelected -= FillView;
        }

        private void SubscribeToInputActions()
        {
            _playerInputActions.UI.Apply.performed += OnApplyPerformed;
            _playerInputActions.UI.Cancel.performed += OnCancelPerformed;
            _playerInputActions.UI.ScrollLeft.performed += OnScrollLeftPerformed;
            _playerInputActions.UI.ScrollRight.performed += OnScrollRightPerformed;
            _playerInputActions.UI.ScrollUp.performed += OnScrollUpPerformed;
            _playerInputActions.UI.ScrollDown.performed += OnScrollDownPerformed;

            _playerInputActions.Enable();
        }

        private void UnsubscribeToInputActions()
        {
            _playerInputActions.UI.Apply.performed -= OnApplyPerformed;
            _playerInputActions.UI.Cancel.performed -= OnCancelPerformed;
            _playerInputActions.UI.ScrollLeft.performed -= OnScrollLeftPerformed;
            _playerInputActions.UI.ScrollRight.performed -= OnScrollRightPerformed;
            _playerInputActions.UI.ScrollUp.performed -= OnScrollUpPerformed;
            _playerInputActions.UI.ScrollDown.performed -= OnScrollDownPerformed;

            _playerInputActions.Dispose();
        }

        private void FillView(HeroData heroData)
        {
            string heroName = _localizationService.GetLocalizedText(heroData.TranslatableName);
            string heroDescription = _localizationService.GetLocalizedText(heroData.TranslatableDescriptions);
            string abilityName =
                _localizationService.GetLocalizedText(heroData.InitialAbilityConfig.UpgradeViewData.TranslatableName);

            _view.FillHeroDescription(heroData.Sprite, heroName, heroDescription);
            _view.FillBaseAbilityView(heroData.InitialAbilityConfig.UpgradeViewData.Icon,
                abilityName);
        }

        private void OnContinueButtonClicked()
        {
            // _viewModel.Continue();
            _gameStateMachine.GoToGameLoop();
        }

        private void OnCloseButtonClicked() =>
            WindowFsm.Close<HeroSelectorWindow>();

        private void OnApplyPerformed(InputAction.CallbackContext context) =>
            OnContinueButtonClicked();

        private void OnCancelPerformed(InputAction.CallbackContext context) =>
            OnCloseButtonClicked();

        private void OnScrollUpPerformed(InputAction.CallbackContext context) =>
            _viewModel.HandleMove(0, -1, _rowCount, _colCount);

        private void OnScrollDownPerformed(InputAction.CallbackContext context) =>
            _viewModel.HandleMove(0, 1, _rowCount, _colCount);

        private void OnScrollLeftPerformed(InputAction.CallbackContext context) =>
            _viewModel.HandleMove(-1, 0, _rowCount, _colCount);

        private void OnScrollRightPerformed(InputAction.CallbackContext context) =>
            _viewModel.HandleMove(1, 0, _rowCount, _colCount);
    }
}