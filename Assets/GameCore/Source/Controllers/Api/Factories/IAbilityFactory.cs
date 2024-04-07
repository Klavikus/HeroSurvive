using GameCore.Source.Domain;
using GameCore.Source.Domain.Configs;
using UnityEngine;

namespace GameCore.Source.Controllers.Api.Factories
{
    public interface IAbilityFactory
    {
        IAbilityController Create(AbilityConfigSO initialAbilityConfig, Transform transform);
    }
}