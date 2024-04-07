using GameCore.Source.Presentation.Api.GameLoop;

namespace GameCore.Source.Presentation.Api
{
    public interface IPersistentUpgradeViewFactory
    {
        IPersistentUpgradeView[] Create();
    }
}