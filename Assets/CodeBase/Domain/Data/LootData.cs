using System;
using UnityEngine;

namespace CodeBase.Domain
{
    [Serializable]
    public struct LootData
    {
        [field: SerializeField] public int Experience { get; private set; }
        [field: SerializeField] public int Currency { get; private set; }

        public void UpdateProgression(float stageProgressionModifier)
        {
            Experience = (int) (Experience * stageProgressionModifier);
            Currency = (int) (Currency * stageProgressionModifier);
        }
    }
}