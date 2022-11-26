using System;
using CodeBase.Domain.Enums;
using UnityEngine;

namespace CodeBase.Domain.Abilities.Size
{
    [Serializable]
    public class SizeBehaviourData
    {
        [field: SerializeField] public SizeType SizeType { get; private set; }
        [field: SerializeField] public float MainTargetSize { get; private set; }
        [field: SerializeField] public float StartTargetSize { get; private set; }
        [field: SerializeField] public float EndTargetSize { get; private set; }
        [field: SerializeField] public float StartTime { get; private set; }
        [field: SerializeField] public float EndTime { get; private set; }
        [field: SerializeField] public float FullTimePeriod { get; private set; }


        public void UpdateTargetSize(float size)
        {
            MainTargetSize = size;
            CheckFields();
        }

        public void UpdateFullTime(float duration)
        {
            FullTimePeriod = duration;
            CheckFields();
        }

        private void CheckFields()
        {
            if (StartTime + EndTime >= 1 && SizeType == SizeType.OverLifetime)
                throw new ArgumentException(
                    $"{nameof(StartTime)} + {nameof(EndTime)} should be lower than 1");
        }
    }
}