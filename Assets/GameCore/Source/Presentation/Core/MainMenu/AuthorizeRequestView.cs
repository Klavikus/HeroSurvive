using Modules.MVPPassiveView.Runtime;
using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;

namespace GameCore.Source.Presentation.Core.MainMenu
{
    public class AuthorizeRequestView : ViewBase
    {
        [field: SerializeField] public Canvas MainCanvas { get; private set; }
        [field: SerializeField] public ActionButton AcceptButton { get; private set; }
        [field: SerializeField] public ActionButton DeclineButton { get; private set; }
    }
}