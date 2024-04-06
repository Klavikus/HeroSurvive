using GameCore.Source.Presentation.Api.GameLoop;

namespace GameCore.Source.Infrastructure.Api
{
    public interface IViewFactory
    {
        IUpgradeLevelView[] Create(int count);
    }
}