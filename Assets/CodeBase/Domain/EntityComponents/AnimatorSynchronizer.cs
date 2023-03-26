using UnityEngine;

namespace CodeBase.Domain
{
    public class AnimatorSynchronizer : MonoBehaviour
    {
        [SerializeField] private InputController _inputController;
        [SerializeField] private Animator _animator;

        private static readonly int MovingCash = Animator.StringToHash("Moving");

        private void OnEnable() => _inputController.InputUpdated += OnInputUpdated;

        private void OnDisable() => _inputController.InputUpdated -= OnInputUpdated;

        private void OnInputUpdated(InputData inputData) =>
            _animator.SetBool(MovingCash, inputData.Vertical != 0 || inputData.Horizontal != 0);
    }
}