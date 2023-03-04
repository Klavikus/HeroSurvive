using CodeBase.Infrastructure.Services;

namespace CodeBase.Infrastructure.StateMachine
{
    public interface IViewModelBuilder : IService
    {
        TViewModel Build<TViewModel>() where TViewModel : class;
    }
}