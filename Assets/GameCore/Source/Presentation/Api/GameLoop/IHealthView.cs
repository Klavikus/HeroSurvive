using Modules.MVPPassiveView.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Api.GameLoop
{
    public interface IHealthView
    {
        GameObject Target { get; }
        Image FillImage { get; }
        float TransitionMaxDelta { get; }
        void Construct(IPresenter p);
    }
}