using System.Linq;
using GameCore.Source.Controllers.Api.Providers;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Data.Dto;
using GameCore.Source.Domain.Models;
using GameCore.Source.Domain.Services;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Infrastructure.Api.Services;
using GameCore.Source.Infrastructure.Core.Services.DI;

namespace GameCore.Source.Application.GameFSM.States
{
    public class LoadDataState : IState
    {
        private readonly ServiceContainer _serviceContainer;
        private IProgressService _progressService;

        public LoadDataState(ServiceContainer serviceContainer)
        {
            _serviceContainer = serviceContainer;
        }

        public async void Enter()
        {
            _progressService = _serviceContainer.Single<IProgressService>();
            IConfigurationProvider configurationProvider = _serviceContainer.Single<IConfigurationProvider>();
            IModelProvider modelProvider = _serviceContainer.Single<IModelProvider>();

            await _progressService.Load();
            await _progressService.SyncWithCloud();

            PrepareModels(configurationProvider, modelProvider, _progressService);
            
            _serviceContainer.Single<IAudioPlayerService>().Enable();

            _serviceContainer.Single<IGameStateMachine>().Enter<MainMenuState>();
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }

        private void PrepareModels(IConfigurationProvider configurationProvider, IModelProvider modelProvider,
            IProgressService progressService)
        {
            UpgradesConfigSO upgradesConfig = configurationProvider.UpgradesConfig;
            UpgradeModel[] result = new UpgradeModel[upgradesConfig.UpgradeData.Length];

            for (var i = 0; i < result.Length; i++)
            {
                result[i] = new UpgradeModel(upgradesConfig.UpgradeData[i]);
                UpgradeDto upgradeDto = progressService.Get<UpgradeDto>(result[i].Data.KeyName);
                result[i].SetLevel(upgradeDto.Level);
            }

            modelProvider.Bind(result);

            PropertiesModel propertiesModel = new();
            modelProvider.Bind(propertiesModel);

            HeroModel heroModel = new();
            HeroData[] availableHeroesData =
                configurationProvider.HeroConfig.HeroesData.Select(heroData => heroData).ToArray();
            heroModel.SetHeroData(availableHeroesData.First());
            modelProvider.Bind(heroModel);

            CurrencyDto currencyDto = progressService.Get<CurrencyDto>(CurrencyDto.DefaultId);
            CurrencyModel currencyModel = new(currencyDto);
            modelProvider.Bind(currencyModel);

            PlayerModel playerModel = new PlayerModel();
            modelProvider.Bind(playerModel);

            AccountDto accountDto = _progressService.Get<AccountDto>(AccountDto.DefaultId);
            AccountModel accountModel = new AccountModel(accountDto);
            modelProvider.Bind(accountModel);

            //TODO: Refactor this
            accountModel.TotalWavesClearChanged += _ =>
            {
                _progressService.UpdateAccountData(accountModel);
                _progressService.Save();
            };
            
            SettingsDto settingsDto = _progressService.Get<SettingsDto>(SettingsDto.DefaultId);
            SettingsModel settingsModel = new SettingsModel(settingsDto);
            modelProvider.Bind(settingsModel);
        }
    }
}