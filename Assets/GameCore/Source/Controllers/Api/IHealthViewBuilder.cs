using UnityEngine;

namespace GameCore.Source.Controllers.Api
{
    public interface IHealthViewBuilder
    {
        void Build(GameObject objectWithHealthView);
    }
}