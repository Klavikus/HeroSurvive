using System;
using System.Collections;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Services;
using UnityEngine;

namespace GameCore.Source.Controllers.Core
{
    public class EnemyAI : MonoBehaviour
    {
        private EnemyAIData _enemyAIData;
        private ITargetService _targetService;
        private IGameLoopService _gameLoopService;

        private bool _isFacedRight = true;
        private bool _isWaitingForInitialize = true;

        private bool _isMoving;
        private bool _isPreviousMoving;
        private bool _isAttackOnCooldown;
        private bool _isStaggered;
        private float _currentStaggerPower;
        private float _staggerResist;

        private Coroutine _attackCoroutine;
        private Coroutine _staggerCoroutine;
        private WaitForSeconds _attackCheckDelay;
        private WaitForSeconds _staggerDelay;

        public event Action AttackDistanceReached;
        public event Action StartMoving;
        public event Action StopMoving;
        public event Action StaggerOut;

        public bool IsMoving => _isMoving;
        public bool IsStaggered => _isStaggered;

        public void Update()
        {
            if (_isWaitingForInitialize || _isStaggered)
                return;

            if (_gameLoopService.PlayerIsAlive)
                Move();
            else
                _isMoving = false;

            UpdateMoveStatus();
        }

        public void Initialize(EnemyAIData enemyAIData, ITargetService targetService, IGameLoopService gameLoopService)
        {
            _enemyAIData = enemyAIData;
            _targetService = targetService;
            _gameLoopService = gameLoopService;

            _attackCheckDelay = new WaitForSeconds(enemyAIData.AttackCheckInterval);
            _staggerDelay = new WaitForSeconds(GameConstants.AIMinimumStaggerDelay);
            _isWaitingForInitialize = false;
            _staggerResist = enemyAIData.StaggerResist;
            _isStaggered = false;
            _isPreviousMoving = false;
            _isAttackOnCooldown = false;

            if (_attackCoroutine != null)
                StopCoroutine(_attackCoroutine);
        }

        public void UpdateProgression(float progressionModifier) =>
            _enemyAIData.CalculateWithProgression(progressionModifier);

        public void Stagger(float stagger)
        {
            stagger -= stagger * _staggerResist;
            stagger = Mathf.Max(stagger, 0);

            if (stagger == 0 && _isStaggered == false)
            {
                StaggerOut?.Invoke();

                return;
            }

            if (_isStaggered)
                return;

            if (_currentStaggerPower < stagger)
                _currentStaggerPower = stagger;

            if (_staggerCoroutine != null)
                StopCoroutine(_staggerCoroutine);

            _staggerCoroutine = StartCoroutine(StartStaggerCooldown());
        }

        private IEnumerator StartStaggerCooldown()
        {
            _isStaggered = true;

            while (_currentStaggerPower > 0)
            {
                _currentStaggerPower -= 0.1f;

                yield return _staggerDelay;
            }

            _isStaggered = false;
            StaggerOut?.Invoke();
        }

        private void Move()
        {
            Vector3 moveVector =
                _enemyAIData.MoveStrategy.GetMoveVector(transform, _targetService.GetPlayerPosition(),
                    _enemyAIData.ObstacleCheckDistance);
            Vector3 directionToTarget = _targetService.GetPlayerPosition() - transform.position;

            if (directionToTarget.magnitude > _enemyAIData.AttackRange)
            {
                _isMoving = false;

                if (moveVector.magnitude > 0)
                {
                    _isMoving = true;
                    transform.Translate(moveVector * (_enemyAIData.Speed * Time.deltaTime), Space.World);
                }
            }
            else
            {
                _isMoving = false;
                HandleAttackStatus();
            }

            if ((directionToTarget.x < 0 && _isFacedRight) || (directionToTarget.x > 0 && !_isFacedRight))
                Flip();
        }

        private void HandleAttackStatus()
        {
            if (_isAttackOnCooldown)
                return;

            if (_attackCoroutine != null)
                StopCoroutine(_attackCoroutine);

            AttackDistanceReached?.Invoke();

            _attackCoroutine = StartCoroutine(StartAttackCooldown());
        }

        private IEnumerator StartAttackCooldown()
        {
            _isAttackOnCooldown = true;

            yield return _attackCheckDelay;

            _isAttackOnCooldown = false;
        }

        private void Flip()
        {
            _isFacedRight = !_isFacedRight;
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }

        private void UpdateMoveStatus()
        {
            if (_isMoving && _isPreviousMoving == false)
                StartMoving?.Invoke();

            if (_isMoving == false && _isPreviousMoving)
                StopMoving?.Invoke();

            _isPreviousMoving = _isMoving;
        }
    }
}