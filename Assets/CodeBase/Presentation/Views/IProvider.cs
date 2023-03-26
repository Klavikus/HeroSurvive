using CodeBase.Infrastructure;

namespace CodeBase.Presentation
{
    public interface IProvider : IService
    {
        public void Bind<TModel>(TModel model) where TModel : class;
        TModel Get<TModel>() where TModel : class;
    }
}