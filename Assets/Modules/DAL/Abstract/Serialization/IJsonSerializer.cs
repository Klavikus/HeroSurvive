namespace Modules.DAL.Abstract.Serialization
{
    public interface IJsonSerializer
    {
        string Serialize(object @object);
        T Deserialize<T>(string jsonString);
    }
}