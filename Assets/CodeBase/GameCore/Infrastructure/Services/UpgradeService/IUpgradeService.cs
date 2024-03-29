using System;
using CodeBase.GameCore.Domain.Models;
using CodeBase.GameCore.Infrastructure.Factories;

namespace CodeBase.GameCore.Infrastructure.Services.UpgradeService
{
    public interface IUpgradeService : IService, IInitializeable
    {
        event Action Updated;
        MainProperties GetUpgradesPropertiesData();
        void AddProperties(UpgradeModel upgradeModel);
    }
}