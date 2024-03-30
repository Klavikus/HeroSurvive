using System;
using GameCore.Source.Domain.Models;

namespace GameCore.Source.Controllers.Api.Services
{
    public interface IUpgradeService
    {
        event Action Updated;
        MainProperties GetUpgradesPropertiesData();
        void AddProperties(UpgradeModel upgradeModel);
    }
}