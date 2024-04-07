using GameCore.Source.Domain.Data;
using GameCore.Source.Presentation.Api.MainMenu.HeroSelector;
using Modules.MVPPassiveView.Runtime;

namespace GameCore.Source.Controllers.Api
{
    public interface IPropertyPresenterFactory
    {
        IPresenter Create(IPropertyView view, MainPropertyViewData propertyViewData);
    }
}