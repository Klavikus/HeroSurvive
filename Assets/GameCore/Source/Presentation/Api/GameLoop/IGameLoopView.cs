using Modules.UIComponents.Runtime.Implementations.Buttons;
using TMPro;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Api.GameLoop
{
    public interface IGameLoopView
    {
        ActionButton CloseButton { get; }
        TMP_Text WaveCountText { get; }
        TMP_Text KillCountText { get; }
        TMP_Text GoldCountText { get; }
        TMP_Text LevelCountText { get; }
        Image LevelCompletionImage { get; }
    }
}