using System;
using CodeBase.Domain;

namespace CodeBase.Infrastructure
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