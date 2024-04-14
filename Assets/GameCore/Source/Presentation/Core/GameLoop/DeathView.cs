using GameCore.Source.Presentation.Api.GameLoop;
using Modules.MVPPassiveView.Runtime;
using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;

namespace GameCore.Source.Presentation.Core.GameLoop
{
    public class DeathView : ViewBase, IDeathView
    {
        [field: SerializeField] public Canvas[] Canvases { get; private set; }
        [field: SerializeField] public ActionButton ResurrectButton { get; private set; }
        [field: SerializeField] public ActionButton DoubleRewardAdsButton { get; private set; }
        [field: SerializeField] public ActionButton BackToMenuButton { get; private set; }

        public void Initialize()
        {
            ResurrectButton.Initialize();
            DoubleRewardAdsButton.Initialize();
            BackToMenuButton.Initialize();
        }
        public void Show()
        {
            foreach (Canvas canvas in Canvases)
                canvas.enabled = true;
        }

        public void Hide()
        {
            foreach (Canvas canvas in Canvases)
                canvas.enabled = false;
        }
    }
}