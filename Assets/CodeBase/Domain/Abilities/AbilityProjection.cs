using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Domain.Abilities.Attack;
using CodeBase.Domain.Abilities.Movement;
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
        [SerializeField] private View _view;

        private AbilityData _abilityBaseData;
        private IAttackBehaviour _attackBehaviour;
        private IMovementBehaviour _movementBehaviour;
        private TargetFinderService _targetFinderService;

        private List<Coroutine> _coroutineHandlers;
        private WaitForSeconds _returnToPoolDelay;

        public void Initialize(TargetFinderService targetFinderService, AbilityData abilityBaseData,
            IAttackBehaviour attackBehaviour,
            IMovementBehaviour movementBehaviour, SpawnData spawnData)
        {
            gameObject.SetActive(true);

            _abilityBaseData = abilityBaseData;
            _attackBehaviour = attackBehaviour;
            _movementBehaviour = movementBehaviour;
            _targetFinderService = targetFinderService;

            transform.localScale = Vector3.one * abilityBaseData.Size;

            _view.Initialize(_spriteRenderer);
            _attackBehaviour.Initialize(_rigidbody2D);
            _movementBehaviour.Initialize(transform, spawnData, abilityBaseData, targetFinderService);


            _returnToPoolDelay = new WaitForSeconds(_abilityBaseData.Duration);
            _coroutineHandlers = new List<Coroutine>
            {
                StartCoroutine(_view.Run()),
                StartCoroutine(_attackBehaviour.Run()),
                StartCoroutine(_movementBehaviour.Run()),
            };
            _attackBehaviour.PenetrationLimit += OnAttackExpired;
            StartCoroutine(ReturnToPool());
        }

        private void OnAttackExpired()
        {
            _attackBehaviour.PenetrationLimit -= OnAttackExpired;
            foreach (Coroutine coroutineHandler in _coroutineHandlers)
                if (coroutineHandler != null)
                    StopCoroutine(coroutineHandler);

            gameObject.SetActive(false);
        }

        private IEnumerator ReturnToPool()
        {
            yield return _returnToPoolDelay;

            foreach (Coroutine coroutineHandler in _coroutineHandlers)
                if (coroutineHandler != null)
                    StopCoroutine(coroutineHandler);

            gameObject.SetActive(false);
        }
    }
}