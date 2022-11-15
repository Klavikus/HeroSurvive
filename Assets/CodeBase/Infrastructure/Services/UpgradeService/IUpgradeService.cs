using System;
using CodeBase.Domain;
using CodeBase.MVVM.Models;
using CodeBase.MVVM.Views;

namespace CodeBase.Infrastructure.Services.UpgradeService
{
    public interface IUpgradeService : IService
    {
        event Action Updated;
        MainProperties GetUpgradesPropertiesData();
        void AddProperties(UpgradeModel upgradeModel);
    }
}