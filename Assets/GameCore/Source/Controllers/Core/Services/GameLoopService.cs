using System;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Factories;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Models;
using GameCore.Source.Domain.Services;
using Modules.GamePauseSystem.Runtime;

namespace GameCore.Source.Controllers.Core.Services
{
    public class GameLoopService : IGameLoopService
    {
        private readonly AbilityBuilder _abilityBuilder;
        private readonly PlayerBuilder _playerBuilder;
        private readonly PlayerEventHandler _playerEventHandler;
        private readonly ILeveCompetitionService _levelCompetitionService;
        private readonly IAudioPlayerService _sfxService;
        private readonly IGamePauseService _gamePauseService;
        private readonly GameLoopModel _gameLoopModel;
        private readonly HeroModel _heroModel;
        private readonly ITargetService _targetService;

        public GameLoopService(
            AbilityBuilder abilityBuilder,
            PlayerBuilder playerBuilder,
            ILeveCompetitionService levelCompetitionService,
            PlayerEventHandler playerEventHandler,
            IAudioPlayerService sfxService,
            IGamePauseService gamePauseService,
            GameLoopModel gameLoopModel,
            HeroModel heroModel,
            ITargetService targetService)
        {
            _abilityBuilder = abilityBuilder ?? throw new ArgumentNullException(nameof(abilityBuilder));
            _playerBuilder = playerBuilder ?? throw new ArgumentNullException(nameof(playerBuilder));
            _levelCompetitionService = levelCompetitionService ??
                                       throw new ArgumentNullException(nameof(levelCompetitionService));
            _playerEventHandler = playerEventHandler ?? throw new ArgumentNullException(nameof(playerEventHandler));
            _sfxService = sfxService ?? throw new ArgumentNullException(nameof(sfxService));
            _gamePauseService = gamePauseService ?? throw new ArgumentNullException(nameof(gamePauseService));
            _gameLoopModel = gameLoopModel ?? throw new ArgumentNullException(nameof(gameLoopModel));
            _heroModel = heroModel ?? throw new ArgumentNullException(nameof(heroModel));
            _targetService = targetService ?? throw new ArgumentNullException(nameof(targetService));
        }

        public void Initialize() =>
            _gameLoopModel.PlayerResurrected += OnPlayerResurrected;

        public event Action<HeroData> LevelInvoked;

        public event Action LevelCloseInvoked;

        private void OnPlayerResurrected()
        {
            _playerBuilder.RespawnPlayer();
        }

        public void InvokeLevelStart(HeroData heroData) => LevelInvoked?.Invoke(heroData);

        public void InvokeLevelClose() => _gameLoopModel.InvokeLevelClose();

        public void Start()
        {
            _abilityBuilder.Build(_heroModel, this);

            _playerBuilder.BindCameraToPlayer();
            _playerBuilder.BindEventsHandler(_playerEventHandler);
            _levelCompetitionService.StartCompetition();
            // GameLoopPauseViewModel gameLoopPauseViewModel = new GameLoopPauseViewModel(gamePauseService);
            // viewModelProvider.Bind(gameLoopPauseViewModel);

            // GameObject.FindObjectOfType<SceneCompositionRoot>().Initialize(AllServices.Container);

            _sfxService.PlayAmbient();
        }

        public void Stop()
        {
            _levelCompetitionService.Stop();

            _sfxService.StopAmbient();
        }
    }
}