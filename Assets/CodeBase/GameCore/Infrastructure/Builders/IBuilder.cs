namespace CodeBase.GameCore.Infrastructure.Builders
{
    public interface IBuilder
    {
        T Build<T>() where T : class;
    }
}