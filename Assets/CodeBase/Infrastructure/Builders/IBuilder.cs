namespace CodeBase.Infrastructure
{
    public interface IBuilder
    {
        T Build<T>() where T : class;
    }
}