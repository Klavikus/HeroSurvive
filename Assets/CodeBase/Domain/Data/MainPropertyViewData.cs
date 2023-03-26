using System;
using UnityEngine;

namespace CodeBase.Domain
{
    [Serializable]
    public class MainPropertyViewData
    {
        [field: SerializeField] public float Value;
        [field: SerializeField] public BaseProperty BaseProperty { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Prefix { get; private set; }
        [field: SerializeField] public string Postfix { get; private set; }
        [field: SerializeField] public TranslatableString[] TranslatableShortName { get; private set; }
        [field: SerializeField] public TranslatableString[] TranslatableFullName { get; private set; }
        [field: SerializeField] public bool IsIncreaseGood { get; private set; }
        [field: SerializeField] public bool IsSigned { get; private set; }
    }
}