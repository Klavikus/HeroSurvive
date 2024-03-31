using GameCore.Source.Domain.Abilities;
using GameCore.Source.Domain.EntityComponents;
using UnityEngine;

namespace GameCore.Source.Domain.Models
{
    public class PlayerModel
    {
        private Vector3 _lastPosition;
        private Camera _playerCamera;
        public bool IsFreeSlotAvailable { get; set; }
        public AbilityContainer AbilityContainer { get; set; }
        public Transform Transform { get; set; }
        public MoveController MoveController { get; set; }
        public Camera Camera { get; set; }

        public Vector3 GetPlayerPosition()
        {
            if (Transform)
                _lastPosition = Transform.position;

            return _lastPosition;
        }

        public Vector3 GetPlayerDirection() =>
            MoveController.LastMoveVector;

        public Camera GetPlayerCamera() => Camera;
    }
}