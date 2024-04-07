using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCore.Source.Presentation.Api.GameLoop
{
    public interface IAbilityUpgradeView
    {
        event Action<IAbilityUpgradeView> Selected;
        void OnPointerClick(PointerEventData pointerEventData);
        void SetSelected(bool isSelected);
        void Show(Sprite icon, string title, string description);
        void Hide();
    }
}