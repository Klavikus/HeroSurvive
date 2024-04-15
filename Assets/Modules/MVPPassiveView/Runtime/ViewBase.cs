using System;
using UnityEngine;

namespace Modules.MVPPassiveView.Runtime
{
    public abstract class ViewBase : MonoBehaviour
    {
        private IPresenter _presenter;

        public void Construct(IPresenter presenter)
        {
            if (presenter == null)
                throw new ArgumentNullException(nameof(presenter));

            gameObject.SetActive(false);
           
            OnBeforeConstruct();
            
            _presenter = presenter;
            gameObject.SetActive(true);
            _presenter.Enable();

            OnAfterConstruct();
        }

        protected virtual void OnBeforeConstruct()
        {
        }

        protected virtual void OnAfterConstruct()
        {
        }

        protected virtual void OnBeforeDestroy()
        {
        }

        private void OnDestroy()
        {
            OnBeforeDestroy();
            _presenter?.Disable();
        }
    }
}