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
        private readonly HeroModel _heroModel;
        private readonly PlayerBuilder _playerBuilder;
        private readonly GameLoopModel _gameLoopModel;
        private readonly PlayerEventHandler _playerEventHandler;
        private LeveCompetitionService _levelCompetitionService;

        public GameLoopService(LevelMapFactory levelMapFactory, GameLoopViewBuilder gameLoopViewBuilder,
            AbilityBuilder abilityBuilder, HeroModel heroModel, PlayerBuilder playerBuilder,
            GameLoopModel gameLoopModel, LeveCompetitionService levelCompetitionService,
            PlayerEventHandler playerEventHandler)
        {
            _levelMapFactory = levelMapFactory;
            _gameLoopViewBuilder = gameLoopViewBuilder;
            _abilityBuilder = abilityBuilder;
            _heroModel = heroModel;
            _playerBuilder = playerBuilder;
            _gameLoopModel = gameLoopModel;
            _levelCompetitionService = levelCompetitionService;
            _playerEventHandler = playerEventHandler;
            _gameLoopModel.PlayerResurrected += OnPlayerResurrected ;
        }

        public event Action<HeroData> LevelInvoked;
        public event Action LevelCloseInvoked;

        private void OnPlayerResurrected() => _playerBuilder.RespawnPlayer();
        public void InvokeLevelStart(HeroData heroData) => LevelInvoked?.Invoke(heroData);
        public void InvokeLevelClose() => _gameLoopModel.InvokeLevelClose();

        public void Start()
        {
            _levelMapFactory.Create();
            _gameLoopViewBuilder.Build();
            _abilityBuilder.Build(_heroModel);
            _playerBuilder.BindCameraToPlayer();
            _playerBuilder.BindEventsHandler(_playerEventHandler);

            _levelCompetitionService.StartCompetition();
        }

        public void Stop() => _levelCompetitionService.Stop();
    }
}