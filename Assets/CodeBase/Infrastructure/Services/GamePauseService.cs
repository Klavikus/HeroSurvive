using System;
using Agava.WebUtility;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public class GamePauseService : IGamePauseService
    {
        private int _pauseCalls;
        private int _pauseCallsFromFocus;
        private int _pauseCallsFromRewardedAds;
        private int _pauseCallsFromInterstitialAds;
        private int _pauseCallsFromRaceMenu;
        private bool _isPaused;

        public GamePauseService()
        {
            Application.focusChanged += OnFocusChanged;
            WebApplication.InBackgroundChangeEvent += OnBackgroundChanged;
        }

        ~GamePauseService()
        {
            Application.focusChanged -= OnFocusChanged;
            WebApplication.InBackgroundChangeEvent -= OnBackgroundChanged;
        }

        public event Action PauseStarted;
        public event Action PauseEnded;

        public bool IsPaused => _isPaused;

        public void StartPauseByRewarded()
        {
            HandlePauseCall(
                isIncreaseCall: true,
                currentCalls: ref _pauseCallsFromRewardedAds,
                totalCalls: ref _pauseCalls);
            HandlePause();
        }

        public void StopPauseByRewarded()
        {
            HandlePauseCall(
                isIncreaseCall: false,
                currentCalls: ref _pauseCallsFromRewardedAds,
                totalCalls: ref _pauseCalls);
            HandlePause();
        }

        public void StartPauseByMenu()
        {
            HandlePauseCall(
                isIncreaseCall: true,
                currentCalls: ref _pauseCallsFromRaceMenu,
                totalCalls: ref _pauseCalls);
            HandlePause();
        }

        public void StopPauseByRaceMenu()
        {
            HandlePauseCall(
                isIncreaseCall: false,
                currentCalls: ref _pauseCallsFromRaceMenu,
                totalCalls: ref _pauseCalls);
            HandlePause();
        }

        public void StartPauseByInterstitial()
        {
            HandlePauseCall(
                isIncreaseCall: true,
                currentCalls: ref _pauseCallsFromInterstitialAds,
                totalCalls: ref _pauseCalls);
            HandlePause();
        }

        public void StopPauseByInterstitial()
        {
            HandlePauseCall(
                isIncreaseCall: false,
                currentCalls: ref _pauseCallsFromInterstitialAds,
                totalCalls: ref _pauseCalls);
            HandlePause();
        }

        private void HandlePauseCall(bool isIncreaseCall, ref int currentCalls, ref int totalCalls)
        {
            if (isIncreaseCall)
            {
                if (currentCalls == 0)
                {
                    currentCalls = 1;
                    totalCalls++;
                }
            }
            else
            {
                if (currentCalls > 0)
                {
                    currentCalls = 0;
                    totalCalls--;
                }
            }
        }

        private void OnBackgroundChanged(bool inBackground) => OnFocusChanged(!inBackground);

        private void OnFocusChanged(bool inFocus)
        {
            if (inFocus)
            {
                HandlePauseCall(
                    isIncreaseCall: false,
                    currentCalls: ref _pauseCallsFromFocus,
                    totalCalls: ref _pauseCalls);
            }
            else
            {
                HandlePauseCall(
                    isIncreaseCall: true,
                    currentCalls: ref _pauseCallsFromFocus,
                    totalCalls: ref _pauseCalls);
            }

            HandlePause();
        }

        private void HandlePause()
        {
            if (_pauseCalls != 0)
            {
                if (_isPaused)
                    return;

                _isPaused = true;

                AudioListener.pause = true;
                AudioListener.volume = 0;
                PauseStarted?.Invoke();
                Time.timeScale = 0;
            }
            else
            {
                if (_isPaused == false)
                    return;

                _isPaused = false;

                PauseEnded?.Invoke();
                Time.timeScale = 1;
            }
        }
    }
}