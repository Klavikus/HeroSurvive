using CodeBase.GameCore.Domain.Data;
using CodeBase.GameCore.Domain.Enemies;
using CodeBase.GameCore.Infrastructure.Services;

namespace CodeBase.GameCore.Infrastructure
{
    public interface IEnemySpawnService : IService
    {
        Enemy[] SpawnWave(EnemySpawnData[] enemiesSpawnData);
        void MoveCloserToPlayer(Enemy enemy);
        void ClearEnemies();
    }
}