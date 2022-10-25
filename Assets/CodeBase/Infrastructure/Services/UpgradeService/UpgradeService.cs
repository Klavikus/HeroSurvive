using System;
using CodeBase.HeroSelection;

namespace CodeBase.Infrastructure.Services.UpgradeService
{
    class UpgradeService : IUpgradeService
    {
        public event Action Updated;

        public MainProperties GetUpgradesPropertiesData()
        {
            return new MainProperties();
        }
    }
}