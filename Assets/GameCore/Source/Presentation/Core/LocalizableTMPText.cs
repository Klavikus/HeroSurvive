using GameCore.Source.Common;
using TMPro;
using UnityEngine;

namespace GameCore.Source.Presentation.Core
{
    public class LocalizableTMPText : MonoBehaviour, ILocalizable
    {
        [SerializeField] private TMP_Text _text;

        [field: SerializeField] public string LocalizationKey { get; private set; }
    
        public string LocalizedValue => _text.text;

        public void SetValue(string localizedValue) =>
            _text.text = localizedValue;
    }
}