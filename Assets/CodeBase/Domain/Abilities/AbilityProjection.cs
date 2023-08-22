using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Infrastructure;
using FMODUnity;
using UnityEngine;

namespace CodeBase.Domain
{
    public class AbilityProjection : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private View _view;

        private AbilityData _abilityBaseData;
        private IAttackBehaviour _attackBehaviour;
        private IMovementBehaviour _movementBehaviour;
        private ITargetService _targetFinderService;

        private List<Coroutine> _coroutineHandlers;
        private WaitForSeconds _returnToPoolDelay;
        private ISizeBehaviour _sizeBehaviour;
        private IAudioPlayerService _audioPlayerService;

        public event Action<AbilityProjection> Destroed;

        public void Initialize(IAudioPlayerService audioPlayerService, ITargetService targetFinderService,
            AbilityData abilityBaseData,
            IAttackBehaviour attackBehaviour,
            IMovementBehaviour movementBehaviour,
            ISizeBehaviour sizeBehaviour,
            SpawnData spawnData)
        {
            gameObject.SetActive(true);

            //TODO: Refactor this
            if (abilityBaseData.AudioData.IsPlayable)
                AllServices.Container.AsSingle<IAudioPlayerService>().PlayOneShot(abilityBaseData.AudioData.FMOD, spawnData.StartPosition);
            
            _audioPlayerService = audioPlayerService;
            _abilityBaseData = abilityBaseData;
            _attackBehaviour = attackBehaviour;
            _movementBehaviour = movementBehaviour;
            _sizeBehaviour = sizeBehaviour;

            _targetFinderService = targetFinderService;

            // transform.localScale = Vector3.one * abilityBaseData.Size;

            _sizeBehaviour.Initialize(transform, abilityBaseData.SizeBehaviourData);
            _view.Initialize(_spriteRenderer);
            _attackBehaviour.Initialize(_rigidbody2D);
            _movementBehaviour.Initialize(transform, spawnData, abilityBaseData, targetFinderService);


            _returnToPoolDelay = new WaitForSeconds(_abilityBaseData.Duration);
            _coroutineHandlers = new List<Coroutine>
            {
                StartCoroutine(_sizeBehaviour.Run()),
                StartCoroutine(_view.Run()),
                StartCoroutine(_attackBehaviour.Run()),
                StartCoroutine(_movementBehaviour.Run()),
            };

            // _audioPlayerService.PlayVFXAudio(_abilityBaseData.AudioData.StartAFX);
            _attackBehaviour.PenetrationLimit += OnAttackExpired;
            _attackBehaviour.EnemyHitted += OnEnemyHitted;
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