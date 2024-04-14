using System;
using Cysharp.Threading.Tasks;
using Modules.Common.WindowFsm.Runtime.Abstract;
using Modules.MVPPassiveView.Runtime;
using UnityEngine;

namespace GameCore.Source.Controllers.Core.Presenters.Base
{
    public abstract class BaseWindowPresenter<T> : IPresenter
        where T : IWindow
    {
        private readonly Action _onOpenedBaseAction;
        private readonly Action _onClosedBaseAction;

        protected BaseWindowPresenter(
            IWindowFsm windowFsm,
            Action onOpenedBaseAction = null,
            Action onClosedBaseAction = null)
        {
            WindowFsm = windowFsm ?? throw new ArgumentNullException(nameof(windowFsm));
            _onOpenedBaseAction = onOpenedBaseAction;
            _onClosedBaseAction = onClosedBaseAction;
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

            _onOpenedBaseAction?.Invoke();

            OnAfterOpened();
        }

        private void OnClosed(IWindow window)
        {
            if (window is not T)
                return;

            _onClosedBaseAction?.Invoke();

            OnAfterClosed();
        }

        private void InitialCheck()
        {
            if (WindowFsm.CurrentWindow is T)
                _onOpenedBaseAction?.Invoke();
            else
                _onClosedBaseAction?.Invoke();
        }
    }
}