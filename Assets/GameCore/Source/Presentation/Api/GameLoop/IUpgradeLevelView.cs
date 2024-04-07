using UnityEngine;

namespace GameCore.Source.Presentation.Api.GameLoop
{
    public interface IUpgradeLevelView
    {
        GameObject GameObject { get; }
        Transform Transform { get; }
        void SetSelectedStatus(bool isSelected);
        void Show();
        void Hide();
    }
}