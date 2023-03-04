﻿using CodeBase.Domain.Data;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.Models;

namespace CodeBase.Infrastructure.StateMachine
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IMainMenuFactory _mainMenuFactory;
        private readonly IModelProvider _modelProvider;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _mainMenuFactory = AllServices.Container.AsSingle<IMainMenuFactory>();
            _modelProvider = AllServices.Container.AsSingle<IModelProvider>();
        }

        public void Enter(string sceneName)
        {
            _modelProvider.Get<GameLoopModel>().StartLevelInvoked += OnLevelInvoked;
            _sceneLoader.Load(sceneName, onLoaded: OnLoaded);
        }

        public void Exit()
        {
            _modelProvider.Get<GameLoopModel>().StartLevelInvoked -= OnLevelInvoked;
        }

        private void OnLoaded()
        {
            _mainMenuFactory.Initialization();
        }

        private void OnLevelInvoked(HeroData heroData) => _gameStateMachine.Enter<GameLoopState, HeroData>(heroData);
    }
}