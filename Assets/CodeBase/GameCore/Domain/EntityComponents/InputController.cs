using CodeBase.GameCore.Domain.Data;
using UnityEngine;

namespace CodeBase.GameCore.Domain.EntityComponents
{
    public class InputController : InputHandler
    {
        private PlayerInputActions _playerInputActions;

        public void Initialize()
        {
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Enable();
        }

        private void OnEnable()
        {
            _playerInputActions?.Enable();
        }

        private void OnDisable()
        {
            _playerInputActions?.Disable();
        }

        private void Update()
        {
            float horizontal = _playerInputActions.Combat.Move.ReadValue<Vector2>().x;
            float vertical = _playerInputActions.Combat.Move.ReadValue<Vector2>().y;

            InvokeInputUpdated(new InputData(horizontal, vertical));
        }
    }
}