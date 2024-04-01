using GameCore.Source.Presentation.Api;
using Modules.MVPPassiveView.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Core
{
    public class HealthView : ViewBase, IHealthView
    {
        [field: SerializeField] public GameObject Target { get; private set; }
        [field: SerializeField] public Image FillImage { get; private set; }
        [field: SerializeField] public float TransitionMaxDelta { get; private set; }
    }
}