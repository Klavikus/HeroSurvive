using System;
using Modules.Common.WindowFsm.Runtime.Abstract;
using Modules.MVPPassiveView.Runtime;
using UnityEngine;

namespace GameCore.Source.Controllers.Core
{
    public abstract class BaseWindowPresenter<T> : IPresenter
        where T : IWindow
    {
        private readonly Canvas _canvas;

        protected BaseWindowPresenter(IWindowFsm windowFsm, Canvas canvas)
        {
            WindowFsm = windowFsm ?? throw new ArgumentNullException(nameof(windowFsm));
            _canvas = canvas ? canvas : throw new ArgumentNullException(nameof(canvas));
        }

        public IWindowFsm WindowFsm { get; }

        public void Enable()
        {
            WindowFsm.Opened += OnOpened;
            WindowFsm.Closed += OnClosed;

            InitialCheck();

            OnAfterEnable();
        }

        public void Disable()
        {
            WindowFsm.Opened -= OnOpened;
            WindowFsm.Opened -= OnClosed;

            OnAfterDisable();
        }

        protected virtual void OnAfterEnable()
        {
        }

        protected virtual void OnAfterDisable()
        {
        }

        protected virtual void OnAfterOpened()
        {
        }

        protected virtual void OnAfterClosed()
        {
        }

        private void OnOpened(IWindow window)
        {
            if (window is not T)
                return;

            _canvas.enabled = true;

            OnAfterOpened();
        }

        private void OnClosed(IWindow window)
        {
            if (window is not T)
                return;

            _canvas.enabled = false;

            OnAfterClosed();
        }

        private void InitialCheck() =>
            _canvas.enabled = WindowFsm.CurrentWindow is T;
    }
}