namespace CodeBase.Infrastructure.Builders
{
    public interface IBuilder
    {
        T Build<T>() where T : class;
    }
}