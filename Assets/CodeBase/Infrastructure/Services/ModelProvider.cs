using CodeBase.MVVM.Models;

namespace CodeBase.Infrastructure.Services
{
    public class ModelProvider : IModelProvider
    {
        public ModelProvider(GameLoopModel gameLoopModel)
        {
            GameLoopModel = gameLoopModel;
        }

        public GameLoopModel GameLoopModel { get; }
    }
}