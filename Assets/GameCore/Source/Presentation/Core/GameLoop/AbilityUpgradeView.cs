using System;
using GameCore.Source.Presentation.Api.GameLoop;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.GameCore.Presentation.Views
{
    public class AbilityUpgradeView : MonoBehaviour, IAbilityUpgradeView, IPointerClickHandler
    {
        public event Action<IAbilityUpgradeView> Selected;

        [field: SerializeField] public Image Icon { get; private set; }
        [field: SerializeField] public Image SelectionBorder { get; private set; }
        [field: SerializeField] public Image SelectionBackground { get; private set; }
        [field: SerializeField] public TMP_Text Title { get; private set; }
        [field: SerializeField] public TMP_Text Description { get; private set; }

        public void Show(Sprite icon, string title, string description)
        {
            gameObject.SetActive(true);
            Icon.sprite = icon;
            Title.text = title;
            Description.text = description;

            SelectionBorder.enabled = false;
            SelectionBackground.enabled = false;
        }

        public void Hide() =>
            gameObject.SetActive(false);

        public void SetSelected(bool isSelected)
        {
            SelectionBorder.enabled = isSelected;
            SelectionBackground.enabled = isSelected;
        }

        public void OnPointerClick(PointerEventData eventData) =>
            Selected?.Invoke(this);
    }
}