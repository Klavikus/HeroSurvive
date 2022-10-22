using System.Collections.Generic;
using CodeBase.Stats;
using UnityEngine;

namespace CodeBase
{
    public interface IAbilityHandler
    {
        void Initialize();
        void AddAbility(Ability newAbility);
        void UpdateAbilityData(IReadOnlyDictionary<PlayerStat, float> stats);
    }

    public class AbilityHandler : MonoBehaviour, IAbilityHandler
    {
        private List<Ability> _abilities;

        public void Initialize()
        {
            _abilities = new List<Ability>();
        }

        public void AddAbility(Ability newAbility)
        {
            _abilities.Add(newAbility);
            newAbility.Initialize(transform);
        }

        public void UpdateAbilityData(IReadOnlyDictionary<PlayerStat, float> stats)
        {
            foreach (Ability ability in _abilities)
            {
                ability.Update(stats);
            }
        }

        private void LateUpdate()
        {
            foreach (Ability ability in _abilities)
            {
                ability.Execute();
            }
        }
    }
}