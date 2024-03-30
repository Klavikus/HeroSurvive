
using GameCore.Source.Domain.Data;

namespace GameCore.Source.Controllers.Api.Services
{
    public interface IEnemySpawnService
    {
        IEnemyController[] SpawnWave(EnemySpawnData[] enemiesSpawnData);
        void ClearEnemies();
    }
}