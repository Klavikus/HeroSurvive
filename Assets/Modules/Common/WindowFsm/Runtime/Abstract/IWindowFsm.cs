using System;

namespace Modules.Common.WindowFsm.Runtime.Abstract
{
    public interface IWindowFsm
    {
        event Action<IWindow> Opened;
        event Action<IWindow> Closed;

        IWindow CurrentWindow { get; }

        void OpenWindow<TWindow>() where TWindow : IWindow;
        void Close<TWindow>() where TWindow : IWindow;
        void CloseCurrentWindow();
        void ClearHistory();
    }
}