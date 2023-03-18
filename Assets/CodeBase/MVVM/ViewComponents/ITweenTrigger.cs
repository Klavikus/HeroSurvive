using System;

public interface ITweenTrigger
{
    event Action Showed;
    event Action Hided;

    public void InvokeShow();
    public void InvokeHide();
}