using System;

namespace CodeBase.Presentation.ViewComponents
{
    public interface ITweenTrigger
    {
        event Action<ITweenTrigger> Showed;
        event Action<ITweenTrigger> Hided;

        public void InvokeShow();
        public void InvokeHide();
    }
}