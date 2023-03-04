using System;
using CodeBase.Domain.Data;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.MVVM.Builders;
using CodeBase.MVVM.Models;

namespace CodeBase.Infrastructure.Services
{
    public class GameLoopService : IGameLoopService
    {
        private readonly LevelMapFactory _levelMapFactory;
        private readonly GameLoopViewBuilder _gameLoopViewBuilder;
        private readonly AbilityBuilder _abilityBuilder;
        private readonly IModelProvider _modelProvider;
        private readonly PlayerBuilder _playerBuilder;
        private readonly PlayerEventHandler _playerEventHandler;
        private readonly ILeveCompetitionService _levelCompetitionService;

        public GameLoopService(
            LevelMapFactory levelMapFactory,
            GameLoopViewBuilder gameLoopViewBuilder,
            AbilityBuilder abilityBuilder,
            IModelProvider modelProvider,
            PlayerBuilder playerBuilder,
            ILeveCompetitionService levelCompetitionService,
            PlayerEventHandler playerEventHandler)
        {
            _levelMapFactory = levelMapFactory;
            _gameLoopViewBuilder = gameLoopViewBuilder;
            _abilityBuilder = abilityBuilder;
            _modelProvider = modelProvider;
            _playerBuilder = playerBuilder;
            _levelCompetitionService = levelCompetitionService;
            _playerEventHandler = playerEventHandler;
        }

        public void Initialize() => 
            _modelProvider.Get<GameLoopModel>().PlayerResurrected += OnPlayerResurrected;

        public event Action<HeroData> LevelInvoked;

        public event Action LevelCloseInvoked;

        private void OnPlayerResurrected()
        {
            _playerBuilder.RespawnPlayer();
        }

        public void InvokeLevelStart(HeroData heroData) => LevelInvoked?.Invoke(heroData);

        public void InvokeLevelClose() => _modelProvider.Get<GameLoopModel>().InvokeLevelClose();

        public void Start()
        {
            _levelMapFactory.Create();
            _gameLoopViewBuilder.Build();
            _abilityBuilder.Build( _modelProvider.Get<HeroModel>());
            _playerBuilder.BindCameraToPlayer();
            _playerBuilder.BindEventsHandler(_playerEventHandler);

            _levelCompetitionService.StartCompetition();
        }

        public void Stop() => _levelCompetitionService.Stop();
    }
}