using System;

namespace GameCore.Source.Controllers.Api.Services
{
    public interface ILeveCompetitionService
    {
        event Action<int> WaveCompleted;
        event Action AllWavesCompleted;
        event Action<IEnemyController> EnemyKilled;
        void StartCompetition();
        void Stop();
        int GetAllWavesCount();
    }
}