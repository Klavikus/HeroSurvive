using System;
using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.ViewModels;
using TMPro;
using UnityEngine;

namespace CodeBase.MVVM.Views
{
    public class UserNameView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private UserNameViewModel _userNameViewModel;
        private IViewModelProvider _viewModelProvider;

        private void OnEnable()
        {
            if (_userNameViewModel == null)
                return;
            
            _userNameViewModel.UserNameChanged += OnNameChanged;
        }

        private void OnDisable() => _userNameViewModel.UserNameChanged -= OnNameChanged;

        private void Start()
        {
            _viewModelProvider = AllServices.Container.AsSingle<IViewModelProvider>();
            _userNameViewModel = _viewModelProvider.UserNameViewModel;
            _text.text = _userNameViewModel.Name;
            _userNameViewModel.UserNameChanged += OnNameChanged;
        }

        private void OnNameChanged(string newName) => _text.text = newName;
    }
}