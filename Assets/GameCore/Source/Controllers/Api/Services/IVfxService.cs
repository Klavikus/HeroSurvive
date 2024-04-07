using UnityEngine;

namespace GameCore.Source.Controllers.Api.Services
{
    public interface IVfxService
    {
        void HandleKill(Vector3 transformPosition);
        void Clear();
        void Reset();
    }
}