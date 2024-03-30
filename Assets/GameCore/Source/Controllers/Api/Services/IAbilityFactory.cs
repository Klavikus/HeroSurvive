using GameCore.Source.Domain.Configs;
using UnityEngine;

namespace GameCore.Source.Controllers.Api.Services
{
    public interface IAbilityFactory
    {
        void BindGameLoopService(IGameLoopService gameLoopService);
        IAbilityController Create(AbilityConfigSO initialAbilityConfig, Transform transform);
    }
}