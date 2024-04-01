﻿using GameCore.Source.Presentation.Api;
using Modules.MVPPassiveView.Runtime;
using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;

namespace GameCore.Source.Presentation.Core.MainMenu
{
    public class GameLoopView : ViewBase, IGameLoopView
    {
        [field: SerializeField] public Canvas Canvas { get; set; }
        [field: SerializeField] public ActionButton CloseButton { get; set; }
    }
}