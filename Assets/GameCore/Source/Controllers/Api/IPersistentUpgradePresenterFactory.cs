﻿using GameCore.Source.Domain.Data;
using GameCore.Source.Presentation.Api.GameLoop;
using Modules.MVPPassiveView.Runtime;

namespace GameCore.Source.Controllers.Api
{
    public interface IPersistentUpgradePresenterFactory
    {
        IPresenter Create(UpgradeData upgradeData, IPersistentUpgradeView view);
    }
}