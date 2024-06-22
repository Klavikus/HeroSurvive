using System;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Presenters.Base;
using GameCore.Source.Controllers.Core.ViewModels;
using GameCore.Source.Controllers.Core.WindowFsms.Windows;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Presentation.Api;
using Modules.Common.WindowFsm.Runtime.Abstract;

namespace GameCore.Source.Controllers.Core.Presenters.MainMenu
{
    public class SettingsPresenter : BaseWindowPresenter<SettingsWindow>
    {
        private readonly ISettingsView _view;
        private readonly ISettingsViewModel _viewModel;
        private readonly ILocalizationService _localizationService;

        private readonly IGameStateMachine _gameStateMachine;

        public SettingsPresenter(
            IWindowFsm windowFsm,
            ISettingsView view,
            ISettingsViewModel settingsViewModel
        ) : base(windowFsm, view.Show, view.Hide)
        {
            _view = view;
            _viewModel = settingsViewModel ?? throw new ArgumentNullException(nameof(settingsViewModel));
        }

        protected override void OnAfterEnable()
        {
            _view.ExitButton.Initialize();
            
            _view.MasterAudio.SetValueWithoutNotify(_viewModel.GetMasterVolume());
            _view.MusicAudio.SetValueWithoutNotify(_viewModel.GetMusicVolume());
            _view.VfxAudio.SetValueWithoutNotify(_viewModel.GetSfxVolume());

            _view.ExitButton.Clicked += OnCloseButtonClicked;
            _view.MasterAudio.onValueChanged.AddListener(OnMasterSliderChanged);
            _view.MusicAudio.onValueChanged.AddListener(OnMusicSliderChanged);
            _view.VfxAudio.onValueChanged.AddListener(OnVfxSliderChanged);
        }

        protected override void OnAfterDisable()
        {
            _view.ExitButton.Clicked -= OnCloseButtonClicked;
            _view.MasterAudio.onValueChanged.RemoveListener(OnMasterSliderChanged);
            _view.MusicAudio.onValueChanged.RemoveListener(OnMusicSliderChanged);
            _view.VfxAudio.onValueChanged.RemoveListener(OnVfxSliderChanged);
        }

        private void OnCloseButtonClicked() =>
            WindowFsm.Close<SettingsWindow>();

        private void OnMasterSliderChanged(float value) =>
            _viewModel.SetMasterVolume(value);

        private void OnMusicSliderChanged(float value) =>
            _viewModel.SetMusicVolume(value);

        private void OnVfxSliderChanged(float value) =>
            _viewModel.SetVfxVolume(value);
    }
}