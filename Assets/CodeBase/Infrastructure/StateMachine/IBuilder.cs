namespace CodeBase.Infrastructure.StateMachine
{
    public interface IBuilder
    {
        T Build<T>() where T : class;
    }
}