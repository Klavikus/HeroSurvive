using System;
using GameCore.Source.Presentation.Api.GameLoop;
using Modules.MVPPassiveView.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Core.MainMenu.Upgrades
{
    public class PersistentUpgradeView : ViewBase, IPersistentUpgradeView, IPointerClickHandler
    {
        [field: SerializeField] public TMP_Text Name { get; private set; }
        [field: SerializeField] public Image Icon { get; private set; }
        [field: SerializeField] public Image SelectionBorder { get; private set; }
        [field: SerializeField] public RectTransform UpgradeLevelViewsContainer { get; private set; }
        public Transform Transform => transform;

        public event Action Clicked;

        public void OnPointerClick(PointerEventData eventData) =>
            Clicked?.Invoke();
    }
}