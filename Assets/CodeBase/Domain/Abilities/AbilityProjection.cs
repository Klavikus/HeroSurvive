using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Domain.Abilities.Attack;
using CodeBase.Domain.Abilities.Movement;
using CodeBase.Domain.Abilities.Size;
using CodeBase.Domain.Abilities.Views;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Domain.Abilities
{
    public class AbilityProjection : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private bool _useVfxView;
        [SerializeField] private View _view;

        private AbilityData _abilityBaseData;
        private IAttackBehaviour _attackBehaviour;
        private IMovementBehaviour _movementBehaviour;

        private List<Coroutine> _coroutineHandlers;
        private WaitForSeconds _returnToPoolDelay;
        private ISizeBehaviour _sizeBehaviour;
        private IAudioPlayerService _audioPlayerService;

        public event Action<AbilityProjection> Destroed;

        public void Initialize(
            IAudioPlayerService audioPlayerService,
            ITargetService targetFinderService,
            AbilityData abilityBaseData,
            IAttackBehaviour attackBehaviour,
            IMovementBehaviour movementBehaviour,
            ISizeBehaviour sizeBehaviour,
            SpawnData spawnData)
        {
            gameObject.SetActive(true);

            _audioPlayerService = audioPlayerService;
            _abilityBaseData = abilityBaseData;
            _attackBehaviour = attackBehaviour;
            _movementBehaviour = movementBehaviour;
            _sizeBehaviour = sizeBehaviour;

            if (abilityBaseData.AudioData.IsPlayable)
                _audioPlayerService.PlayOneShot(abilityBaseData.AudioData.FMOD, spawnData.StartPosition);

            _sizeBehaviour.Initialize(transform, abilityBaseData.SizeBehaviourData);
            _attackBehaviour.Initialize(_rigidbody2D);
            _movementBehaviour.Initialize(transform, spawnData, abilityBaseData, targetFinderService);

            _attackBehaviour.PenetrationLimit += OnAttackExpired;
            _attackBehaviour.EnemyHitted += OnEnemyHitted;

            _returnToPoolDelay = new WaitForSeconds(_abilityBaseData.Duration);
            _coroutineHandlers = new List<Coroutine>
            {
                StartCoroutine(_sizeBehaviour.Run()),
                StartCoroutine(_attackBehaviour.Run()),
                StartCoroutine(_movementBehaviour.Run()),
            };

            if (_useVfxView == false)
            {
                _view.Initialize(_spriteRenderer);
                _coroutineHandlers.Add(StartCoroutine(_view.Run()));
            }

            StartCoroutine(ReturnToPool());
        }

        private void OnDestroy() => Destroed?.Invoke(this);

        private void OnEnemyHitted(Transform enemy)
        {
            _audioPlayerService.PlayHit(enemy.position);
        }

        private void OnAttackExpired()
        {
            _attackBehaviour.PenetrationLimit -= OnAttackExpired;

            Disable();
        }

        private IEnumerator ReturnToPool()
        {
            yield return _returnToPoolDelay;

            Disable();
        }

        private void Disable()
        {
            foreach (Coroutine coroutineHandler in _coroutineHandlers)
                if (coroutineHandler != null)
                    StopCoroutine(coroutineHandler);

            gameObject.SetActive(false);
        }
    }
}