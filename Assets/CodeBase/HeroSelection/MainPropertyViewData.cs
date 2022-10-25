using System;
using CodeBase.Enums;
using UnityEngine;

[Serializable]
public class MainPropertyViewData
{
    [field: SerializeField] public float Value;
    [field: SerializeField] public BaseProperty BaseProperty { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public string Prefix { get; private set; }
    [field: SerializeField] public string Suffix { get; private set; }
    [field: SerializeField] public string ShortName { get; private set; }
    [field: SerializeField] public string FullName { get; private set; }
}