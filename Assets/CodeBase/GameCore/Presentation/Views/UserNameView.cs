using CodeBase.GameCore.Domain.Data;
using CodeBase.GameCore.Domain.Services;
using CodeBase.GameCore.Infrastructure.Services;
using TMPro;
using UnityEngine;

namespace CodeBase.GameCore.Presentation.Views
{
    public class UserNameView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private IAuthorizeService _authorizeService;
        private ITranslationService _translationService;

        private void Start()
        {
            _authorizeService = AllServices.Container.AsSingle<IAuthorizeService>();
            _translationService = AllServices.Container.AsSingle<ITranslationService>();

            _text.text = _authorizeService.IsAuthorized
                ? string.IsNullOrEmpty(_authorizeService.GetUserData().Name)
                    ? _translationService.GetLocalizedHiddenUser()
                    : _authorizeService.GetUserData().Name
                : _translationService.GetLocalizedHiddenUser();

            _authorizeService.UserDataUpdated += OnUserDataUpdated;
        }

        private void OnDestroy()
        {
            if (_authorizeService == null)
                return;

            _authorizeService.UserDataUpdated -= OnUserDataUpdated;
        }

        private void OnUserDataUpdated(UserData userData)
        {
            _text.text = string.IsNullOrEmpty(userData.Name)
                ? _translationService.GetLocalizedHiddenUser()
                : _authorizeService.GetUserData().Name;
        }
    }
}