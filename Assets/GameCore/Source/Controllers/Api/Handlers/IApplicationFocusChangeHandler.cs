using System;

namespace GameCore.Source.Controllers.Api.Handlers
{
    public interface IApplicationFocusChangeHandler
    {
        event Action<bool> FocusDropped;
    }
}