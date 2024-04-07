using GameCore.Source.Common;
using GameCore.Source.Common.Localization;

namespace GameCore.Source.Presentation.Api.GameLoop
{
    public interface ILocalizationSystemView
    {
        public ILocalizable[] Localizables { get; }
    }
}