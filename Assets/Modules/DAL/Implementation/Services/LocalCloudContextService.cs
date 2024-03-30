using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Modules.DAL.Abstract.Repositories;
using Modules.DAL.Abstract.Services;
using Modules.DAL.Implementation.Data.Entities;
using UnityEngine;

namespace Modules.DAL.Implementation.Services
{
    public class LocalCloudContextService : ILocalCloudContextService
    {
        private readonly ICompositeRepository _localCompositeRepository;
        private readonly ICompositeRepository _cloudCompositeRepository;

        public LocalCloudContextService(
            ICompositeRepository localCompositeRepository,
            ICompositeRepository cloudCompositeRepository
        )
        {
            _localCompositeRepository = localCompositeRepository;
            _cloudCompositeRepository = cloudCompositeRepository;
        }

        public UniTask LoadLocalContext() =>
            _localCompositeRepository.Load();

        public UniTask LoadCloudContext() =>
            _cloudCompositeRepository.Load();

        public async UniTask SaveLocalContext()
        {
            SyncData localSyncData = _localCompositeRepository.GetAll<SyncData>().FirstOrDefault();

            if (localSyncData == null)
            {
                localSyncData = new SyncData();
                _localCompositeRepository.Add<SyncData>(localSyncData);
            }

            localSyncData.SyncCount++;

            await _localCompositeRepository.Save();
        }

        public UniTask SaveCloudContext() =>
            _cloudCompositeRepository.Save();

        public bool CheckUpdateFromCloud()
        {
            List<SyncData> cloudSyncData = _cloudCompositeRepository.GetAll<SyncData>() ?? new List<SyncData>();
            List<SyncData> localSyncData = _localCompositeRepository.GetAll<SyncData>() ?? new List<SyncData>();

            if (cloudSyncData.Count == 0)
                return false;

            if (localSyncData.Count == 0)
                return true;

            return cloudSyncData[0].SyncCount > localSyncData[0].SyncCount && IsLevelProgressDifferent();
        }

        public bool CheckUpdateFromLocal()
        {
            List<SyncData> cloudSyncData = _cloudCompositeRepository.GetAll<SyncData>() ?? new List<SyncData>();
            List<SyncData> localSyncData = _localCompositeRepository.GetAll<SyncData>() ?? new List<SyncData>();

            if (localSyncData.Count == 0)
                return false;

            if (cloudSyncData.Count == 0)
                return true;

            return localSyncData[0].SyncCount > cloudSyncData[0].SyncCount && IsLevelProgressDifferent();
        }

        public void TransferCloudToLocal() =>
            _localCompositeRepository.CopyFrom(_cloudCompositeRepository);

        public void TransferLocalToCloud() =>
            _cloudCompositeRepository.CopyFrom(_localCompositeRepository);

        public async UniTask ClearLocalContext()
        {
            _localCompositeRepository.Clear();
            await _localCompositeRepository.Save();
        }

        public async UniTask ClearCloudContext()
        {
            _cloudCompositeRepository.Clear();
            await _cloudCompositeRepository.Save();
        }

        private bool IsLevelProgressDifferent()
        {
            List<LevelProgress> cloudProgressData =
                _cloudCompositeRepository.GetAll<LevelProgress>() ?? new List<LevelProgress>();
            List<LevelProgress> localProgressData =
                _localCompositeRepository.GetAll<LevelProgress>() ?? new List<LevelProgress>();

            int cloudProgressTotal = cloudProgressData.Sum(data => data.LevelCurrency);
            int localProgressTotal = localProgressData.Sum(data => data.LevelCurrency);

            Debug.Log($"Cloud total progress {cloudProgressTotal}");
            Debug.Log($"Local total progress {localProgressTotal}");

            return cloudProgressTotal != localProgressTotal;
        }
    }
}