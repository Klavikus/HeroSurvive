using GameCore.Source.Presentation.Api.GameLoop;

namespace GameCore.Source.Presentation.Core.Factories
{
    public interface IPersistentUpgradeViewFactory
    {
        IPersistentUpgradeView[] Create();
    }
}