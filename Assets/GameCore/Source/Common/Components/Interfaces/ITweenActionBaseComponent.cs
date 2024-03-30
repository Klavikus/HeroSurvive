using Cysharp.Threading.Tasks;

namespace Source.Common.Components.Interfaces
{
    public interface ITweenActionBaseComponent
    {
        void Initialize();
        void Cancel();
        UniTask PlayForward();
        UniTask PlayBackward();
        void SetForwardState();
        void SetBackwardState();
    }
}