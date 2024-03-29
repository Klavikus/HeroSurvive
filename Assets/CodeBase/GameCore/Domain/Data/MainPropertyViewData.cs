using System;
using CodeBase.GameCore.Domain.Enums;
using UnityEngine;

namespace CodeBase.GameCore.Domain.Data
{
    [Serializable]
    public class MainPropertyViewData
    {
        //TODO: Refactor this
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