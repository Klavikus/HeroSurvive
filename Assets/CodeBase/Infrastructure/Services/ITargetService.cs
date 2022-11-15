using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public interface ITargetService : IService
    {
        Vector3 GetPlayerPosition();
        Vector3 GetPlayerDirection();
        Vector3 GetClosestEnemyToPlayer();
    }
}