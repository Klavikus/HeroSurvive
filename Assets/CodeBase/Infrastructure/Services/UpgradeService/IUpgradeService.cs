using System;
using CodeBase.Domain;
using CodeBase.Domain.Models;
using CodeBase.Infrastructure.Factories;

namespace CodeBase.Infrastructure.Services.UpgradeService
{
    public interface IUpgradeService : IService, IInitializeable
    {
        event Action Updated;
        MainProperties GetUpgradesPropertiesData();
        void AddProperties(UpgradeModel upgradeModel);
    }
}