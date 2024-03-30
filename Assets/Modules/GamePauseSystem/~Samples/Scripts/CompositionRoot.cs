using Modules.Common.Utils;
using Modules.GamePauseSystem.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.GamePauseSystem._Samples.scripts
{
    public class CompositionRoot : MonoBehaviour
    {
        [SerializeField] private Button _adsCall;
        [SerializeField] private Button _adsRelease;
        [SerializeField] private Button _focusCall;
        [SerializeField] private Button _focusRelease;
        [SerializeField] private Button _uiCall;
        [SerializeField] private Button _uiRelease;

        private void Start()
        {
            IMultiCallHandler multiCallHandler = new MultiCallHandler();
            IGamePauseService gamePauseService = new GamePauseService(multiCallHandler);

            gamePauseService.Paused += () => Debug.Log("Paused");
            gamePauseService.Resumed += () => Debug.Log("Resumed");

            _adsCall.onClick.AddListener(() => gamePauseService.InvokeByAds(true));
            _focusCall.onClick.AddListener(() => gamePauseService.InvokeByFocusChanging(true));
            _uiCall.onClick.AddListener(() => gamePauseService.InvokeByUI(true));
            _adsRelease.onClick.AddListener(() => gamePauseService.InvokeByAds(false));
            _focusRelease.onClick.AddListener(() => gamePauseService.InvokeByFocusChanging(false));
            _uiRelease.onClick.AddListener(() => gamePauseService.InvokeByUI(false));
        }
    }
}