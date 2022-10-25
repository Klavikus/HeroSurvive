using CodeBase.Infrastructure.Services;

namespace CodeBase.Infrastructure.Factories
{
    public interface IMainMenuFactory : IService
    {
        void Initialization();
        void ShowMenu();
    }
}