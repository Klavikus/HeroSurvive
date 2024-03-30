using System.Collections.Generic;
using GameCore.Source.Domain.Configs;

namespace GameCore.Source.Domain.Services
{
    public interface IConfigurationProvider
    {
        VfxConfig VfxConfig { get; }
        List<AbilityConfigSO> AbilityConfigs { get; }
    }
}