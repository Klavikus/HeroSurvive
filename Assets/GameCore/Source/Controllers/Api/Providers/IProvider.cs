namespace GameCore.Source.Controllers.Api.Providers
{
    public interface IProvider
    {
        public void Bind<TModel>(TModel model) where TModel : class;
        TModel Get<TModel>() where TModel : class;
    }
}