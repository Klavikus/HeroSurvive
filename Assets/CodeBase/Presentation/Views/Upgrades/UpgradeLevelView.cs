using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Presentation.Views.Upgrades
{
    public class UpgradeLevelView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _baseColor;

        public void SetSelectedStatus(bool isSelected) => _icon.color = isSelected ? _selectedColor : _baseColor;
    }
}