using GameCore.Source.Presentation.Api.GameLoop;
using Modules.MVPPassiveView.Runtime;
using Modules.UIComponents.Runtime.Implementations;
using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;

namespace GameCore.Source.Presentation.Core.GameLoop
{
    public class WinView : ViewBase, IWinView
    {
        [field: SerializeField] public Canvas Canvas { get; private set; }
        [field: SerializeField] public ActionButton ContinueButton { get; private set; }
        [field: SerializeField] public ActionButton DoubleRewardButton { get; private set; }
        [field: SerializeField] public ActionCounter KillCounter { get; private set; }
        [field: SerializeField] public ActionCounter GoldCounter { get; private set; }

        public void Initialize()
        {
            ContinueButton.Initialize();
            DoubleRewardButton.Initialize();
        }
    }
}