using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCore.Source.Domain.Data.Dto;
using GameCore.Source.Domain.Models;
using GameCore.Source.Infrastructure.Api.Services;
using Modules.DAL.Abstract.Data;
using Modules.DAL.Abstract.Repositories;
using Modules.DAL.Abstract.Services;
using Modules.DAL.Implementation.Data.Entities;
using UnityEngine;

namespace GameCore.Source.Infrastructure.Core.Services
{
    public class ProgressService : IProgressService
    {
        private readonly ICompositeRepository _compositeRepository;
        private readonly ILocalCloudContextService _localCloudContextService;
        private readonly Dictionary<Type, Func<string, IEntity>> _getFuncByType;

        public ProgressService(
            ICompositeRepository compositeRepository,
            ILocalCloudContextService localCloudContextService)
        {
            _compositeRepository = compositeRepository ?? throw new ArgumentNullException(nameof(compositeRepository));
            _localCloudContextService = localCloudContextService ??
                                        throw new ArgumentNullException(nameof(localCloudContextService));

            _getFuncByType = new Dictionary<Type, Func<string, IEntity>>()
            {
                [typeof(CurrencyDto)] = (id) => _compositeRepository.GetById<CurrencyDto>(id)
                                                ?? AddNewEntity<CurrencyDto>(new CurrencyDto()),
                [typeof(UpgradeDto)] = (id) => _compositeRepository.GetById<UpgradeDto>(id)
                                               ?? AddNewEntity<UpgradeDto>(new UpgradeDto(id)),
                [typeof(AccountDto)] = (id) => _compositeRepository.GetById<AccountDto>(id)
                                               ?? AddNewEntity<AccountDto>(new AccountDto()),
                [typeof(SettingsDto)] = (id) => _compositeRepository.GetById<SettingsDto>(id)
                                                ?? AddNewEntity<SettingsDto>(new SettingsDto()),
            };
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
            CurrencyDto currencyDto = _compositeRepository.GetById<CurrencyDto>(nameof(CurrencyDto));

            if (currencyDto == null)
            {
                currencyDto = new CurrencyDto()
                {
                    Gold = 0
                };
                _compositeRepository.Add<CurrencyDto>(currencyDto);
            }

            return currencyDto.Gold;
        }

        public void SetGold(int amount)
        {
            CurrencyDto currencyDto = _compositeRepository.GetById<CurrencyDto>(nameof(CurrencyDto));

            if (currencyDto == null)
            {
                currencyDto = new CurrencyDto()
                {
                    Gold = 0
                };
                _compositeRepository.Add<CurrencyDto>(currencyDto);
            }

            currencyDto.Gold = amount;
        }

        public T Get<T>(string id)
            where T : class, IEntity =>
            _getFuncByType[typeof(T)](id) as T;

        public void UpdateUpgradeData(string id, int level)
        {
            Get<UpgradeDto>(id).Level = level;
        }

        public void UpdateAccountData(AccountModel accountModel)
        {
            AccountDto accountDto = Get<AccountDto>(AccountDto.DefaultId);
            accountDto.TotalWavesCleared = accountModel.TotalWavesCleared;
            Debug.Log($"Total progress updated: {accountDto.TotalWavesCleared}");
        }

        public async void UpdateMasterVolume(SettingsDto newData)
        {
            SettingsDto settingsDto = Get<SettingsDto>(SettingsDto.DefaultId);
            settingsDto.IsMuted = newData.IsMuted;
            settingsDto.MasterVolume = newData.MasterVolume;
            settingsDto.MusicVolume = newData.MusicVolume;
            settingsDto.VfxVolume = newData.VfxVolume;

            await _compositeRepository.Save();
        }

        private T AddNewEntity<T>(IEntity entity)
            where T : class, IEntity =>
            _compositeRepository.Add<T>(entity) as T;
    }
}