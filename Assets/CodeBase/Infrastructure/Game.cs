﻿using CodeBase.Domain.Additional;
using CodeBase.Infrastructure.StateMachine;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public readonly GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, ConfigurationContainer configurationContainer,
            AudioPlayer audioPlayer)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), configurationContainer,
                coroutineRunner, audioPlayer);
        }
    }
}