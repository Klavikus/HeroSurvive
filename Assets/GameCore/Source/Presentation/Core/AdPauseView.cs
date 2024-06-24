using GameCore.Source.Presentation.Api;
using Modules.MVPPassiveView.Runtime;
using UnityEngine;

namespace GameCore.Source.Presentation.Core
{
    public class AdPauseView : ViewBase, IAdPauseView
    {
        [SerializeField] private Canvas _mainCanvas;

        public void Show() =>
            _mainCanvas.enabled = true;

        public void Hide() =>
            _mainCanvas.enabled = false;
    }
}