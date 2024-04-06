using GameCore.Source.Presentation.Api.GameLoop;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Core.MainMenu.Upgrades
{
    public class UpgradeLevelView : MonoBehaviour, IUpgradeLevelView
    {
        [SerializeField] private Image _selectionIcon;
        [SerializeField] private Color _goodColor;
        [SerializeField] private Color _badColor;

        public GameObject GameObject => gameObject;
        public Transform Transform => transform;

        public void SetSelectedStatus(bool isSelected)
        {
            _selectionIcon.color = isSelected ? _goodColor : _badColor;
        }
    }
}