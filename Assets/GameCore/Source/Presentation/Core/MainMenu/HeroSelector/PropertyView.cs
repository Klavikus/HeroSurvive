using GameCore.Source.Presentation.Api.MainMenu.HeroSelector;
using Modules.MVPPassiveView.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Core.MainMenu.HeroSelector
{
    public class PropertyView : ViewBase, IPropertyView
    {
        [field: SerializeField] public TMP_Text Value { get; private set; }
        [field: SerializeField] public TMP_Text ShortName { get; private set; }
        [field: SerializeField] public Image Icon { get; private set; }
        public Transform Transform => transform;
    }
}