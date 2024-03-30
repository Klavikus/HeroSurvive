namespace Source.Infrastructure.Api.Services
{
    public interface ISaveService
    {
        void Save(string key, string value);
        string Get(string key);
        void Clear();
    }
}