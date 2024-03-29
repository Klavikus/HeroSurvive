using System;

namespace CodeBase.GameCore.Presentation.ViewComponents
{
    public interface ITweenTrigger
    {
        event Action<ITweenTrigger> Showed;
        event Action<ITweenTrigger> Hided;

        public void InvokeShow();
        public void InvokeHide();
    }
}