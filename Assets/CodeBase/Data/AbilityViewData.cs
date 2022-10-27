using System;
using UnityEngine;

namespace CodeBase.MVVM.Views
{
    [Serializable]
    public class AbilityViewData
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
    }
}