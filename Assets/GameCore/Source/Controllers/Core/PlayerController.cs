using GameCore.Source.Domain.EntityComponents;
using UnityEngine;

namespace GameCore.Source.Controllers.Core
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Damageable _damageable;
        [SerializeField] private MoveController _moveController;
        [SerializeField] private AbilityHandler _abilityHandler;

        public bool IsFreeSlotAvailable => _abilityHandler.IsFreeSlotAvailable;
    }
}