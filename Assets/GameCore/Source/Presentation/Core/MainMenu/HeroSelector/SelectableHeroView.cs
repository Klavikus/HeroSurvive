using System;
using GameCore.Source.Presentation.Api.MainMenu.HeroSelector;
using Modules.MVPPassiveView.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Core.MainMenu.HeroSelector
{
    public class SelectableHeroView : ViewBase, ISelectableHeroView, IPointerClickHandler
    {
        public event Action Clicked;

        [field: SerializeField] public Image SelectionBorder { get; private set; }
        [field: SerializeField] public Image Image { get; private set; }
        public Transform Transform => transform;

        public void OnPointerClick(PointerEventData eventData) =>
            Clicked?.Invoke();
    }
}