﻿using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.MVVM.Views
{
    public class LeaderBoardViewInvoker : MonoBehaviour
    {
        [SerializeField] private Button _showButton;
        private MenuViewModel _menuViewModel;

        private void Start()
        {
            _menuViewModel = AllServices.Container.Single<IViewModelProvider>().MenuViewModel;
            _showButton.onClick.AddListener(_menuViewModel.InvokeLeaderBoardShow);
        }

        private void OnDisable() => _showButton.onClick.RemoveListener(_menuViewModel.InvokeLeaderBoardShow);
    }
}