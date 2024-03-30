using GameCore.Source.Presentation.Api;

namespace GameCore.Source.Infrastructure.Api
{
    public interface IProjectionPool
    {
        IAbilityProjection[] GetProjections(int count);
        void Clear();
    }
}