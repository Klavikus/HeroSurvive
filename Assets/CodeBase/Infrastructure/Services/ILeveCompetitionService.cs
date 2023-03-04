using System;
using CodeBase.Domain.Enemies;

namespace CodeBase.Infrastructure.Services
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