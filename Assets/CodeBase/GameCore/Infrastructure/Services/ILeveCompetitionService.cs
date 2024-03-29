using System;
using CodeBase.GameCore.Domain.Enemies;

namespace CodeBase.GameCore.Infrastructure.Services
{
    public interface ILeveCompetitionService : IService
    {
        event Action<int> WaveCompleted;
        event Action AllWavesCompleted;
        event Action<Enemy> EnemyKilled;
        void StartCompetition();
        void Stop();
        int GetAllWavesCount();
    }
}