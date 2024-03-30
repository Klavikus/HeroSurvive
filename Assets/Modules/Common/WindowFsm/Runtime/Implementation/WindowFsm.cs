using System;
using System.Collections.Generic;
using Modules.Common.WindowFsm.Runtime.Abstract;

namespace Modules.Common.WindowFsm.Runtime.Implementation
{
    public class WindowFsm<T> : IWindowFsm
        where T : IWindow
    {
        private readonly Dictionary<Type, IWindow> _windowsByType;
        private readonly Stack<IWindow> _stack;

        private IWindow _currentWindow;

        public WindowFsm(Dictionary<Type, IWindow> windowsByType)
        {
            _windowsByType = windowsByType;
            _stack = new Stack<IWindow>();

            OpenWindow<T>();
        }

        public event Action<IWindow> Opened;
        public event Action<IWindow> Closed;

        public IWindow CurrentWindow => _currentWindow;

        public void OpenWindow<TWindow>() where TWindow : IWindow
        {
            if (_currentWindow == _windowsByType[typeof(TWindow)])
                return;

            _stack.Push(_windowsByType[typeof(TWindow)]);

            if (_currentWindow != null)
                Closed?.Invoke(_currentWindow);

            _currentWindow = _stack.Peek();
            Opened?.Invoke(_currentWindow);
        }

        public void Close<TWindow>() where TWindow : IWindow
        {
            if (_currentWindow != _windowsByType[typeof(TWindow)])
                return;

            CloseCurrentWindow();
        }

        public void CloseCurrentWindow()
        {
            if (_currentWindow == null)
                return;

            _stack.Pop();
            Closed?.Invoke(_currentWindow);
            _stack.TryPeek(out _currentWindow);

            if (_currentWindow != null)
                Opened?.Invoke(_currentWindow);
        }

        public void ClearHistory()
        {
            _stack.Clear();

            if (_currentWindow != null)
                _stack.Push(_currentWindow);
        }
    }
}