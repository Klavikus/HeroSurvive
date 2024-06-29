using System;
using System.Collections;
using System.Collections.Generic;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Abilities;
using GameCore.Source.Domain.Abilities.Attack;
using GameCore.Source.Domain.Abilities.Movement;
using GameCore.Source.Domain.Abilities.Size;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Services;
using GameCore.Source.Infrastructure.Api;
using GameCore.Source.Infrastructure.Api.Services;
using GameCore.Source.Presentation.Api.GameLoop.Abilities;
using Modules.MVPPassiveView.Runtime;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameCore.Source.Controllers.Core.Presenters.GameLoop
{
    public class AbilityProjectionPresenter : IPresenter
    {
        private readonly IAbilityProjection _view;
        private readonly AbilityData _abilityData;
        private readonly IAttackBehaviour _attackBehaviour;
        private readonly IMovementBehaviour _movementBehaviour;
        private readonly ISizeBehaviour _sizeBehaviour;
        private readonly IAudioPlayerService _audioPlayerService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IProjectionPool _projectionPool;
        private readonly SpawnData _spawnData;
        private readonly ITargetService _targetFinderService;
        private readonly List<Coroutine> _coroutineHandlers;
        
        private WaitForSeconds _returnToPoolDelay;

        public AbilityProjectionPresenter(
            IAbilityProjection view,
            AbilityData abilityData,
            IAudioPlayerService audioPlayerService,
            ICoroutineRunner coroutineRunner,
            IProjectionPool projectionPool,
            IAttackBehaviour attackBehaviour,
            IMovementBehaviour movementBehaviour,
            ISizeBehaviour sizeBehaviour,
            ITargetService targetFinderService,
            SpawnData spawnData)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _abilityData = abilityData ?? throw new ArgumentNullException(nameof(abilityData));
            _audioPlayerService = audioPlayerService ?? throw new ArgumentNullException(nameof(audioPlayerService));
            _coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
            _projectionPool = projectionPool ?? throw new ArgumentNullException(nameof(projectionPool));
            _attackBehaviour = attackBehaviour ?? throw new ArgumentNullException(nameof(attackBehaviour));
            _movementBehaviour = movementBehaviour ?? throw new ArgumentNullException(nameof(movementBehaviour));
            _sizeBehaviour = sizeBehaviour ?? throw new ArgumentNullException(nameof(sizeBehaviour));
            _targetFinderService = targetFinderService ?? throw new ArgumentNullException(nameof(targetFinderService));
            _spawnData = spawnData;
            _coroutineHandlers = new List<Coroutine>();
        }

        public void Enable()
        {
            if (_abilityData.AudioData.IsPlayable)
                _audioPlayerService.PlayOneShot(_abilityData.AudioData.FMOD, _spawnData.StartPosition);

            _attackBehaviour.PenetrationLimit += OnAttackExpired;
            _attackBehaviour.EnemyHitted += OnEnemyHitted;

            _sizeBehaviour.Initialize(_view.GameObject.transform, _abilityData.SizeBehaviourData);
            _attackBehaviour.Initialize(_view.Rigidbody);
            _movementBehaviour.Initialize(_view.GameObject.transform, _spawnData, _abilityData, _targetFinderService);

            _returnToPoolDelay = new WaitForSeconds(_abilityData.Duration);

            _coroutineHandlers.Add(_coroutineRunner.StartCoroutine(_sizeBehaviour.Run()));
            _coroutineHandlers.Add(_coroutineRunner.StartCoroutine(_attackBehaviour.Run()));
            _coroutineHandlers.Add(_coroutineRunner.StartCoroutine(_movementBehaviour.Run()));

            if (_view.UseVfx == false)
            {
                _view.Animator.Initialize(_view.SpriteRenderer);
                _coroutineHandlers.Add(_coroutineRunner.StartCoroutine(_view.Run()));
            }

            _coroutineHandlers.Add(_coroutineRunner.StartCoroutine(ReturnToPool()));
        }

        public void Disable()
        {
            _attackBehaviour.PenetrationLimit -= OnAttackExpired;
            _attackBehaviour.EnemyHitted -= OnEnemyHitted;

            foreach (Coroutine coroutineHandler in _coroutineHandlers)
                if (coroutineHandler != null)
                    _coroutineRunner.StopCoroutine(coroutineHandler);
        }

        private void OnEnemyHitted(Transform enemy)
        {
            // _audioPlayerService.PlayHit(enemy.position);
        }

        private void OnAttackExpired()
        {
            _attackBehaviour.PenetrationLimit -= OnAttackExpired;
            Object.Destroy(_view.Rigidbody.gameObject);
        }

        private IEnumerator ReturnToPool()
        {
            yield return _returnToPoolDelay;
            
            _attackBehaviour.PenetrationLimit -= OnAttackExpired;
            _attackBehaviour.EnemyHitted -= OnEnemyHitted;

            foreach (Coroutine coroutineHandler in _coroutineHandlers)
                if (coroutineHandler != null)
                    _coroutineRunner.StopCoroutine(coroutineHandler);
            
            _view.Rigidbody.gameObject.SetActive(false);
        }
    }
}