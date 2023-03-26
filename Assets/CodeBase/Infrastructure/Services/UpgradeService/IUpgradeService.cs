using System;
using CodeBase.Domain;

namespace CodeBase.Infrastructure
{
    public interface IUpgradeService : IService, IInitializeable
    {
        event Action Updated;
        MainProperties GetUpgradesPropertiesData();
        void AddProperties(UpgradeModel upgradeModel);
    }
}