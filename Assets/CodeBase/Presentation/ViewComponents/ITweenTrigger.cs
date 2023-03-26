using System;

namespace CodeBase.Presentation
{
    public interface ITweenTrigger
    {
        event Action<ITweenTrigger> Showed;
        event Action<ITweenTrigger> Hided;

        public void InvokeShow();
        public void InvokeHide();
    }
}