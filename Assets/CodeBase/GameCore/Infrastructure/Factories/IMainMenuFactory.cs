using CodeBase.GameCore.Infrastructure.Services;

namespace CodeBase.GameCore.Infrastructure.Factories
{
    public interface IMainMenuFactory : IService
    {
        void Initialization();
    }
}