using System.Collections.Generic;
using CodeBase.Configs;
using CodeBase.Domain.Enums;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.StateMachine;

namespace CodeBase.Domain.Abilities
{
    public interface IAbilityHandler
    {
        void Initialize(AbilityFactory abilityFactory, AudioPlayerService audioPlayerService);
        void AddAbility(AbilityConfigSO newAbility);
        void UpdatePlayerModifiers(IReadOnlyDictionary<BaseProperty, float> stats);
    }
}