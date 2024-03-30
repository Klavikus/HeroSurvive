using Source.Application.Builders;
using UnityEngine;

namespace Source.Application.Bootstrap
{
    public class Bootstrap : MonoBehaviour
    {
        private Game _game;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            
            _game = new GameBuilder().Build();
        }

        private void Start() => 
            _game.Run();

        private void Update() => 
            _game.Update();

        private void OnDestroy() => 
            _game.Finish();
    }
}