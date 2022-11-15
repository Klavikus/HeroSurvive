using System.Collections.Generic;
using CodeBase.Domain.Enums;
using UnityEngine;

namespace CodeBase.Domain.Abilities
{
    public class AbilityHandler : MonoBehaviour, IAbilityHandler
    {
        private List<Ability> _abilities = new List<Ability>();
        private bool _initialized;

        public void Initialize() => _initialized = true;

        public void AddAbility(Ability newAbility)
        {
            _abilities.Add(newAbility);
            newAbility.Initialize(transform);
        }

        public void UpdateAbilityData(IReadOnlyDictionary<BaseProperty, float> stats)
        {
            foreach (Ability ability in _abilities) 
                ability.Update(stats);
        }

        private void LateUpdate()
        {
            if (_initialized == false)
                return;

            foreach (Ability ability in _abilities) 
                ability.Execute();
        }
    }
}