using System;
using CodeBase.HeroSelection;

namespace CodeBase.Infrastructure.Services.UpgradeService
{
    public interface IUpgradeService : IService
    {
        event Action Updated;
        MainProperties GetUpgradesPropertiesData();
    }
}