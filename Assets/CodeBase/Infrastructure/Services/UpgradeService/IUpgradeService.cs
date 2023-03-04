using System;
using CodeBase.Domain;
using CodeBase.Infrastructure.Factories;
using CodeBase.MVVM.Models;

namespace CodeBase.Infrastructure.Services.UpgradeService
{
    public interface IUpgradeService : IService, IInitializeable
    {
        event Action Updated;
        MainProperties GetUpgradesPropertiesData();
        void AddProperties(UpgradeModel upgradeModel);
    }
}