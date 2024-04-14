using Cysharp.Threading.Tasks;

namespace Modules.UIComponents.Runtime.Interfaces
{
    public interface ITweenAction
    {
        void Initialize();
        void Cancel();
        UniTask PlayForward();
        UniTask PlayBackward();
        void SetForwardState();
        void SetBackwardState();
    }
}