using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Abilities;
using GameCore.Source.Domain.EntityComponents;
using GameCore.Source.Domain.Models;
using UnityEngine;

namespace GameCore.Source.Controllers.Core
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Damageable _damageable;
        [SerializeField] private MoveController _moveController;
        [SerializeField] private AbilityHandler _abilityHandler;

        private IPropertyProvider _propertyProvider;
        private MainProperties _currentProperties;
        private IAudioPlayerService _audioPlayerService;

        public bool IsFreeSlotAvailable => _abilityHandler.IsFreeSlotAvailable;
        public AbilityHandler AbilityHandler => _abilityHandler;
        public AbilityContainer AbilityContainer { get; set; }
    }
}