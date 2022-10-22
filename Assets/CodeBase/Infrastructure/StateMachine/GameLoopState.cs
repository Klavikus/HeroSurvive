using CodeBase.HeroSelection;
using CodeBase.Infrastructure.StateMachine;

namespace CodeBase.Infrastructure.States
{
    public class GameLoopState: IPayloadedState<Hero>
    {
        private const string GameLoopScene = "GameLoop";
        
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;

        public GameLoopState(GameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter(Hero hero)
        {
            _sceneLoader.Load(GameLoopScene);
        }

        public void Exit()
        {
            
        }
    }
}