using System.Linq;
using CodeBase.Configs;
using CodeBase.MVVM.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.MVVM.Views
{
    public class UserNameSetterView : MonoBehaviour
    {
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private Button _applyButton;
        [SerializeField] private TMP_InputField _inputField;

        private UserNameViewModel _userNameViewModel;

        public void Initialize(UserNameViewModel userNameViewModel)
        {
            _userNameViewModel = userNameViewModel;
            _inputField.text = _userNameViewModel.Name;

            Hide();

            _applyButton.onClick.AddListener(OnApplyButtonClicked);
            _inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
            _userNameViewModel.ShowInvoked += Show;
            _userNameViewModel.HideInvoked += Hide;

            if (_userNameViewModel.IsDefaultName)
                Show();
        }

        private void OnDisable()
        {
            _userNameViewModel.ShowInvoked -= Show;
            _userNameViewModel.HideInvoked -= Hide;
        }

        private void OnInputFieldValueChanged(string currentText) =>
            _applyButton.gameObject.SetActive(CheckInputStatus(currentText));

        private bool CheckInputStatus(string currentText)
        {
            return string.IsNullOrEmpty(currentText) == false
                   && currentText.Length <= GameConstants.MaxNameLength
                   && string.IsNullOrWhiteSpace(currentText) == false
                   && currentText.All(char.IsLetterOrDigit);
        }

        private void OnApplyButtonClicked() => _userNameViewModel.SetUserName(_inputField.text);

        private void Show() => _mainCanvas.enabled = true;
        private void Hide() => _mainCanvas.enabled = false;
    }
}