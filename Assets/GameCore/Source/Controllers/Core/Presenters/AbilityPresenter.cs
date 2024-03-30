using System;
using System.Collections;
using System.Collections.Generic;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Abilities;
using GameCore.Source.Domain.Abilities.Attack;
using GameCore.Source.Domain.Abilities.Movement;
using GameCore.Source.Domain.Abilities.Size;
using GameCore.Source.Domain.Data;
using GameCore.Source.Infrastructure.Api;
using GameCore.Source.Infrastructure.Api.Services;
using GameCore.Source.Presentation.Api;
using JetBrains.Annotations;
using Modules.MVPPassiveView.Runtime;
using UnityEngine;

namespace GameCore.Source.Controllers.Core.Presenters
{
    public class AbilityPresenter : IPresenter
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

        private WaitForSeconds _returnToPoolDelay;
        private List<Coroutine> _coroutineHandlers;

        public AbilityPresenter(
            [NotNull] IAbilityProjection view,
            [NotNull] AbilityData abilityData,
            [NotNull] IAttackBehaviour attackBehaviour,
            [NotNull] IMovementBehaviour movementBehaviour,
            [NotNull] ISizeBehaviour sizeBehaviour,
            [NotNull] IAudioPlayerService audioPlayerService,
            [NotNull] ICoroutineRunner coroutineRunner,
            [NotNull] IProjectionPool projectionPool,
            SpawnData spawnData)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _abilityData = abilityData ?? throw new ArgumentNullException(nameof(abilityData));
            _attackBehaviour = attackBehaviour ?? throw new ArgumentNullException(nameof(attackBehaviour));
            _movementBehaviour = movementBehaviour ?? throw new ArgumentNullException(nameof(movementBehaviour));
            _sizeBehaviour = sizeBehaviour ?? throw new ArgumentNullException(nameof(sizeBehaviour));
            _audioPlayerService = audioPlayerService ?? throw new ArgumentNullException(nameof(audioPlayerService));
            _coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
            _projectionPool = projectionPool ?? throw new ArgumentNullException(nameof(projectionPool));
            _spawnData = spawnData;
        }

        public void Enable()
        {
            if (_abilityData.AudioData.IsPlayable)
                _audioPlayerService.PlayOneShot(_abilityData.AudioData.FMOD, _spawnData.StartPosition);

            _attackBehaviour.PenetrationLimit += OnAttackExpired;
            _attackBehaviour.EnemyHitted += OnEnemyHitted;

            _returnToPoolDelay = new WaitForSeconds(_abilityData.Duration);
            _coroutineHandlers = new List<Coroutine>
            {
                _coroutineRunner.StartCoroutine(_sizeBehaviour.Run()),
                _coroutineRunner.StartCoroutine(_attackBehaviour.Run()),
                _coroutineRunner.StartCoroutine(_movementBehaviour.Run()),
            };

            if (_view.UseVfx == false)
            {
                _view.Animator.Initialize(_view.SpriteRenderer);
                _coroutineHandlers.Add(_coroutineRunner.StartCoroutine(_view.Run()));
            }

            _coroutineRunner.StartCoroutine(ReturnToPool());
        }

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

        public void Disable()
        {
            foreach (Coroutine coroutineHandler in _coroutineHandlers)
                if (coroutineHandler != null)
                    _coroutineRunner.StopCoroutine(coroutineHandler);
        }
    }
}