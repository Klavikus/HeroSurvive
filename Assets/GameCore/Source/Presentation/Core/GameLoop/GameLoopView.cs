using System.Linq;
using GameCore.Source.Common;
using GameCore.Source.Presentation.Api.GameLoop;
using Modules.MVPPassiveView.Runtime;
using Modules.UIComponents.Runtime.Implementations.Buttons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Core.GameLoop
{
    public class GameLoopView : ViewBase, IGameLoopView
    {
        [field: SerializeField] public ActionButton CloseButton { get; private set; }
        [field: SerializeField] public TMP_Text WaveCountText { get; private set; }
        [field: SerializeField] public TMP_Text KillCountText { get; private set; }
        [field: SerializeField] public TMP_Text GoldCountText { get; private set; }
        [field: SerializeField] public TMP_Text LevelCountText { get; private set; }
        [field: SerializeField] public Image LevelCompletionImage { get; private set; }
    }
}