using GameCore.Source.Presentation.Api.GameLoop;

namespace GameCore.Source.Presentation.Api.Factories
{
    public interface IPersistentUpgradeViewFactory
    {
        IPersistentUpgradeView[] Create();
    }
}