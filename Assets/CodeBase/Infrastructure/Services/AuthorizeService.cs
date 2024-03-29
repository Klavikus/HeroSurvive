using System;
using Agava.YandexGames;
using CodeBase.Domain.Models;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public class AuthorizeService : IAuthorizeService
    {
        private UserData _userData;

        public AuthorizeService()
        {
            _userData = new UserData("");
        }
        
        public event Action Authorized;
        public event Action AuthorizeError;
        public event Action<UserData> UserDataUpdated;
        public bool IsAuthorized { get; private set; }

        public void Authorize()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            OnSuccessCallbackEditor();
            return;
#endif
            PlayerAccount.Authorize(onSuccessCallback: OnSuccessCallback, OnErrorCallback);
        }

        public UserData GetUserData() => _userData;

        private void OnSuccessCallback()
        {
            IsAuthorized = true;
            Authorized?.Invoke();
            PlayerAccount.GetProfileData(onSuccessCallback: OnSuccessGetProfileDataCallback);
        }

        private void OnSuccessCallbackEditor()
        {
            IsAuthorized = true;
            _userData = new UserData("");
            Authorized?.Invoke();
        }

        private void OnSuccessGetProfileDataCallback(PlayerAccountProfileDataResponse response)
        {
            _userData = new UserData(response.publicName);
            UserDataUpdated?.Invoke(_userData);
        }

        private void OnErrorCallback(string errorMessage)
        {
            IsAuthorized = false;
            Debug.Log($"Authorization error: {errorMessage}");
            AuthorizeError?.Invoke();
        }
    }
}