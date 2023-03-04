﻿using CodeBase.MVVM.Models;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.MVVM.Views
{
    public class LeaderBoardAuthorizeRequestView : MonoBehaviour
    {
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private Button _approve;
        [SerializeField] private Button _decline;

        private LeaderBoardsViewModel _leaderBoardsViewModel;

        public void Initialize(LeaderBoardsViewModel leaderBoardsViewModel)
        {
            _leaderBoardsViewModel = leaderBoardsViewModel;

            _leaderBoardsViewModel.LeaderboardAuthorizeRequest += OnAuthorizeRequest;
            _leaderBoardsViewModel.LeaderboardAuthorizeRequestHandled += OnAuthorizeRequestHandled;

            _approve.onClick.AddListener(OnApproveButtonClicked);
            _decline.onClick.AddListener(OnDeclineButtonClicked);
        }

        private void OnDestroy()
        {
            _approve.onClick.RemoveListener(OnApproveButtonClicked);
            _decline.onClick.RemoveListener(OnDeclineButtonClicked);
        }

        private void OnAuthorizeRequest() => Show();
        private void OnAuthorizeRequestHandled(bool authorizeRequestApproved) => Hide();
        private void Show() => _mainCanvas.enabled = true;
        private void Hide() => _mainCanvas.enabled = false;
        private void OnApproveButtonClicked() => _leaderBoardsViewModel.ApproveAuthorizeRequest();
        private void OnDeclineButtonClicked() => _leaderBoardsViewModel.DeclineAuthorizeRequest();
    }
}