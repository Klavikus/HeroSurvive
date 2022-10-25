using System;
using System.Collections.Generic;
using CodeBase.Enums;
using UnityEngine;

[Serializable]
public class HeroData
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public List<AdditionalHeroProperty> AdditionalProperties { get; private set; }
}

public struct AdditionalHeroProperty
{
    public BaseProperty BaseProperty;
    public float Value;
}
