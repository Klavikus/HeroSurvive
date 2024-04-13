﻿using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;

namespace GameCore.Source.Presentation.Api.GameLoop
{
    public interface IUpgradesSelectorView
    {
        Canvas Canvas { get; }
        Transform UpgradeViewsContainer { get; }
        ActionButton CloseButton { get; }
        ActionButton UserNameButton { get; }
        int RowCount { get; }
        int ColCount { get; }
        void Initialize();
    }
}