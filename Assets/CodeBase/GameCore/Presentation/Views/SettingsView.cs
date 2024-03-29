using CodeBase.GameCore.Presentation.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GameCore.Presentation.Views
{
    public class SettingsView : MonoBehaviour
    {
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private Button _closeButton;

        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Toggle _muteToggle;

        private SettingsViewModel _settingsViewModel;
        private bool _isInitialize;

        public void Initialize(SettingsViewModel settingsViewModel)
        {
            _settingsViewModel = settingsViewModel;

            _settingsViewModel.InvokedShow += Show;
            _settingsViewModel.InvokedHide += Hide;

            _closeButton.onClick.AddListener(OnExitButtonClicked);
            
            _masterSlider.value = settingsViewModel.GetMasterVolume();
            _musicSlider.value = settingsViewModel.GetMusicVolume();
            _sfxSlider.value = settingsViewModel.GetSfxVolume();

            _muteToggle.isOn = settingsViewModel.GetMuteStatus();

            _masterSlider.onValueChanged.AddListener(OnMasterValueChanged);
            _musicSlider.onValueChanged.AddListener(OnMusicValueChanged);
            _sfxSlider.onValueChanged.AddListener(OnSfxValueChanged);
            _muteToggle.onValueChanged.AddListener(OnMuteValueChanged);
            _isInitialize = true;
        }

        private void OnExitButtonClicked() => _settingsViewModel.Hide();

        private void OnDestroy()
        {
            if (_isInitialize == false)
                return;

            _settingsViewModel.InvokedShow -= Show;
            _settingsViewModel.InvokedHide -= Hide;
            
            _closeButton.onClick.RemoveListener(OnExitButtonClicked);

            _masterSlider.onValueChanged.RemoveListener(OnMasterValueChanged);
            _musicSlider.onValueChanged.RemoveListener(OnMusicValueChanged);
            _sfxSlider.onValueChanged.RemoveListener(OnSfxValueChanged);
            _muteToggle.onValueChanged.RemoveListener(OnMuteValueChanged);
            
            _muteToggle.onValueChanged.AddListener(OnMuteValueChanged);
        }

        private void OnMasterValueChanged(float value) => _settingsViewModel.SetMasterVolume(value);
        private void OnMusicValueChanged(float value) => _settingsViewModel.SetMusicVolume(value);
        private void OnSfxValueChanged(float value) => _settingsViewModel.SetSfxVolume(value);
        private void OnMuteValueChanged(bool value) => _settingsViewModel.SetMuteStatus(value);

        private void Show() => _mainCanvas.enabled = true;
        private void Hide() => _mainCanvas.enabled = false;
    }
}