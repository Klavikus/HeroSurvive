using System.Collections.Generic;
using CodeBase.GameCore.Configs;
using CodeBase.GameCore.Domain.Enums;
using CodeBase.GameCore.Infrastructure.Factories;
using CodeBase.GameCore.Infrastructure.Services;

namespace CodeBase.GameCore.Domain.Abilities
{
    public interface IAbilityHandler
    {
        void Initialize(AbilityFactory abilityFactory, IAudioPlayerService audioPlayerService);
        void AddAbility(AbilityConfigSO newAbility);
        void UpdatePlayerModifiers(IReadOnlyDictionary<BaseProperty, float> stats);
    }
}