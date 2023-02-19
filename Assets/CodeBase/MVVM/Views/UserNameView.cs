using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.Models;
using TMPro;
using UnityEngine;

namespace CodeBase.MVVM.Views
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
                ? _authorizeService.GetUserData().Name
                : _translationService.GetLocalizedHiddenUser();

            _authorizeService.UserDataUpdated += OnUserDataUpdated;
        }

        private void OnDestroy()
        {
            if (_authorizeService == null)
                return;
            
            _authorizeService.UserDataUpdated -= OnUserDataUpdated;
        }

        private void OnUserDataUpdated(UserData userData) => _text.text = userData.Name;
    }
}