using System.Collections.Generic;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Enums;

namespace GameCore.Source.Controllers.Api
{
    public interface IAbilityController
    {
        void Execute();
        void UpdatePlayerModifiers(IReadOnlyDictionary<BaseProperty, float> stats);
        void Upgrade();
        void ResetUpgrades();
        bool CheckConfig(AbilityConfigSO abilityConfigSO);
        void Dispose();
    }
}