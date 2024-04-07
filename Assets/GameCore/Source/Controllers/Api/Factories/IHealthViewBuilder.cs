using UnityEngine;

namespace GameCore.Source.Controllers.Api.Factories
{
    public interface IHealthViewBuilder
    {
        void Build(GameObject objectWithHealthView);
    }
}