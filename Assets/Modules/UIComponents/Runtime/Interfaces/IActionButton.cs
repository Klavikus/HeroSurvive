using System;

namespace Modules.UIComponents.Runtime.Interfaces
{
    public interface IActionButton
    {
        event Action Clicked;
        void Initialize();
        void SetInteractionLock(bool isLock);
        void Focus();
        void Unfocus();
        void OnButtonClicked();
    }
}