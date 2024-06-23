using System;
using GameCore.Source.Controllers.Api.Handlers;
using UnityEngine;

namespace GameCore.Source.Controllers.Core.Handlers
{
    public class ApplicationFocusChangeHandler : MonoBehaviour, IApplicationFocusChangeHandler
    {
        public event Action<bool> FocusDropped;

        private void OnApplicationFocus(bool hasFocus) =>
            FocusDropped?.Invoke(hasFocus == false);
    }
}