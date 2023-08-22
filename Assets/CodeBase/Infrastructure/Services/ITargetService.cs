using UnityEngine;

namespace CodeBase.Infrastructure
{
    public interface ITargetService : IService
    {
        Vector3 GetPlayerPosition();
        Vector3 GetPlayerDirection();
        Vector3 GetClosestEnemyToPlayer(float radius, LayerMask layerMask);
        void BindPlayerBuilder(PlayerBuilder playerBuilder);
        Camera GetCamera();
        Vector3 GetRandomEnemyPosition();
    }
}