using UnityEngine;

namespace GameCore.Source.Domain.Services
{
    public interface ITargetService
    {
        Vector3 GetPlayerPosition();
        Vector3 GetClosestEnemyToPlayer(float abilityDataAttackRadius, LayerMask layerMask);
        Vector3 GetPlayerDirection();
        Vector3 GetRandomEnemyPosition();
    }
}