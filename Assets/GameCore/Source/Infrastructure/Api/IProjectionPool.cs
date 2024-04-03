using GameCore.Source.Presentation.Api.GameLoop.Abilities;

namespace GameCore.Source.Infrastructure.Api
{
    public interface IProjectionPool
    {
        IAbilityProjection[] GetProjections(int count);
        void Clear();
    }
}