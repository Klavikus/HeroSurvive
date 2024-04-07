using GameCore.Source.Presentation.Api.GameLoop;
using Modules.MVPPassiveView.Runtime;
using Modules.UIComponents.Runtime.Implementations;
using UnityEngine;

namespace GameCore.Source.Presentation.Core
{
    public class CurrencyView : ViewBase, ICurrencyView
    {
        [field: SerializeField] public ActionCounter Counter { get; private set; }
    }
}