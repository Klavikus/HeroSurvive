using GameCore.Source.Domain.Configs;
using UnityEngine;

namespace GameCore.Source.Controllers.Api.Services
{
    public interface IAbilityFactory
    {
        IAbilityController Create(AbilityConfigSO initialAbilityConfig, Transform transform);
    }
}