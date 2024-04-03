using GameCore.Source.Presentation.Api.GameLoop;
using Modules.MVPPassiveView.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Core.GameLoop
{
    public class LevelUpSystemView : ViewBase, ILevelUpSystemView
    {
        [field: SerializeField] public Image LevelCompletionImage { get; private set; }
    }
}