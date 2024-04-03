using GameCore.Source.Common;

namespace GameCore.Source.Presentation.Api.GameLoop
{
    public interface ILocalizationSystemView
    {
        public ILocalizable[] Localizables { get; }
    }
}