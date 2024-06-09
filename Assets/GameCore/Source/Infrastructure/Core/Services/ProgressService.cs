using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCore.Source.Domain.Data.Dto;
using GameCore.Source.Infrastructure.Api.Services;
using Modules.DAL.Abstract.Repositories;
using Modules.DAL.Abstract.Services;
using Modules.DAL.Implementation.Data.Entities;

namespace GameCore.Source.Infrastructure.Core.Services
{
    public class ProgressService : IProgressService
    {
        private readonly ICompositeRepository _compositeRepository;
        private readonly ILocalCloudContextService _localCloudContextService;

        public ProgressService(
            ICompositeRepository compositeRepository,
            ILocalCloudContextService localCloudContextService)
        {
            _compositeRepository = compositeRepository ?? throw new ArgumentNullException(nameof(compositeRepository));
            _localCloudContextService = localCloudContextService ??
                                        throw new ArgumentNullException(nameof(localCloudContextService));
        }

        public UniTask Load() =>
            _compositeRepository.Load();

        public async UniTask Save()
        {
            SyncData localSyncData = _compositeRepository.GetAll<SyncData>().FirstOrDefault();

            if (localSyncData == null)
            {
                localSyncData = new SyncData();
                _compositeRepository.Add<SyncData>(localSyncData);
            }

            localSyncData.SyncCount += 1;

            await _compositeRepository.Save();
            await SyncWithCloud();
        }

        public UniTask SyncWithCloud() =>
            _localCloudContextService.Synchronize();

        public int GetGold()
        {
            CurrencyData currencyData = _compositeRepository.GetById<CurrencyData>(nameof(CurrencyData));

            if (currencyData == null)
            {
                currencyData = new CurrencyData()
                {
                    Gold = 0
                };
                _compositeRepository.Add<CurrencyData>(currencyData);
            }

            return currencyData.Gold;
        }

        public void SetGold(int amount)
        {
            CurrencyData currencyData = _compositeRepository.GetById<CurrencyData>(nameof(CurrencyData));

            if (currencyData == null)
            {
                currencyData = new CurrencyData()
                {
                    Gold = 0
                };
                _compositeRepository.Add<CurrencyData>(currencyData);
            }

            currencyData.Gold = amount;
        }
    }
}