using GameCore.Source.Domain.Configs;

namespace GameCore.Source.Infrastructure.Api.Services.Providers
{
    public interface IConfigurationProvider
    {
        VfxConfig VfxConfig { get; }
    }
}