﻿using System;
using GameCore.Source.Domain.Data;

namespace GameCore.Source.Controllers.Api.Services
{
    public interface IPersistentUpgradeService
    {
        event Action<UpgradeData, int> Upgraded;
        event Action<UpgradeData, int> UpgradeSelected;
        void SelectUpgrade(UpgradeData upgradeData);
        void Upgrade(UpgradeData upgradeData);
        void Reset(UpgradeData upgradeData);
        bool CheckPay(UpgradeData upgradeData, int levelToCheck);
        int GetCurrentUpgradeLevel(UpgradeData upgradeData);
        void HandleMove(int dX, int dY, int rowCount, int colCount);
        UpgradeData GetCurrentUpgradeData();
    }
}