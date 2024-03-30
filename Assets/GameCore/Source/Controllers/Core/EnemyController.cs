using System;
using System.Collections.Generic;
using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.EnemyStateMachine.States;
using GameCore.Source.Controllers.Core.EnemyStateMachine.Transitions;
using GameCore.Source.Controllers.Core.StateMachines;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.EntityComponents;
using GameCore.Source.Domain.Enums;
using GameCore.Source.Domain.Services;
using UnityEngine;

namespace GameCore.Source.Controllers.Core
{
    public class EnemyController : MonoBehaviour, IEnemyController
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Damageable _damageable;
        [SerializeField] private EnemyAI _enemyAI;

        private AnimationSynchronizer _animationSynchronizer;
        private EntityStateMachine _stateMachine;
        private List<Transition> _transitions;
        private DamageSource _damageSource;
        private EnemyData _enemyData;
        private LootData _lootData;

        public event Action<IEnemyController> Died;
        public event Action<IEnemyController> Destroyed;
        public event Action<IEnemyController> OutOfViewTimeout;
        public event Action<IEnemyController> InvokedBackToPool;

        public bool CanReceiveDamage => _damageable.CanReceiveDamage;
        public int KillExperience => _lootData.Experience;
        public int KillCurrency => _lootData.Currency;
        public EnemyType Type => _enemyData.Type;
        public Transform Transform => transform;

        public void Destroy() =>
            GameObject.Destroy(gameObject);

        private void OnDestroy()
        {
            _stateMachine.Dispose();

            Destroyed?.Invoke(this);
        }

        public void Initialize(
            ITargetService targetService,
            EnemyData enemyData,
            IVfxService vfxService,
            IAudioPlayerService audioPlayerService)
        {
            gameObject.SetActive(true);
            _enemyData = enemyData;
            _lootData = enemyData.LootData;
            _enemyAI.Initialize(enemyData.AIData, targetService);
            _damageSource = new DamageSource(enemyData.DamageSourceData, transform);
            _animationSynchronizer = new AnimationSynchronizer(_animator);

            _damageable.Initialize(enemyData.DamageableData);
            //TODO: Move _damageSource.HandleAttack into enemyStateMachine
            _damageable.Died += OnDied;
            _enemyAI.AttackDistanceReached += _damageSource.HandleAttack;

            _enemyAI.enabled = true;

            InitializeStateMachine(vfxService, audioPlayerService);
        }

        public void InvokeBackToPool()
        {
            _damageable.Died -= OnDied;
            _enemyAI.AttackDistanceReached -= _damageSource.HandleAttack;
            InvokedBackToPool?.Invoke(this);
            gameObject.SetActive(false);
        }

        public void UpdateProgression(float completeProgress)
        {
            _enemyAI.UpdateProgression(_enemyData.ProgressionData.EnemyAI.Evaluate(completeProgress));
            _damageable.UpdateProgression(_enemyData.ProgressionData.Damageable.Evaluate(completeProgress));
            _damageSource.UpdateProgression(_enemyData.ProgressionData.DamageSource.Evaluate(completeProgress));
            _lootData.UpdateProgression(_enemyData.ProgressionData.Loot.Evaluate(completeProgress));
        }

        private void OnDied() =>
            Died?.Invoke(this);

        private void InitializeStateMachine(IVfxService vfxService, IAudioPlayerService audioPlayerService)
        {
            IdleEntityState idleEntityState = new(_animationSynchronizer, _enemyAI);
            RunEntityState runEntityState = new(_animationSynchronizer, _enemyAI);
            DieEntityState dieEntityState = new(_animationSynchronizer, _enemyAI, this, vfxService);
            HitEntityState hitEntityState = new(
                _animationSynchronizer,
                _enemyAI,
                _damageable,
                vfxService,
                audioPlayerService);

            IdleToRunTransition idleToRunTransition = new IdleToRunTransition(runEntityState, _enemyAI);
            RunToIdleTransition runToIdleTransition = new RunToIdleTransition(idleEntityState, _enemyAI);
            AnyToDieTransition anyToDieTransition = new AnyToDieTransition(dieEntityState, _damageable);
            AnyToHitTransition anyToHitTransition = new AnyToHitTransition(hitEntityState, _damageable);
            HitToIdleTransition hitToIdleTransition = new HitToIdleTransition(idleEntityState, _enemyAI);
            HitToRunTransition hitToRunTransition = new HitToRunTransition(runEntityState, _enemyAI);

            idleEntityState.SetTransitions(idleToRunTransition, anyToDieTransition, anyToHitTransition);
            runEntityState.SetTransitions(runToIdleTransition, anyToDieTransition, anyToHitTransition);
            hitEntityState.SetTransitions(hitToIdleTransition, hitToRunTransition, anyToDieTransition);

            _transitions = new List<Transition>
            {
                idleToRunTransition,
                runToIdleTransition,
                anyToDieTransition,
                anyToHitTransition,
                hitToIdleTransition,
                hitToRunTransition,
            };

            List<EntityState> states = new List<EntityState>()
            {
                idleEntityState,
                runEntityState,
                dieEntityState,
                hitEntityState
            };

            _stateMachine = new EntityStateMachine(idleEntityState, states);

            foreach (Transition transition in _transitions)
                transition.BindContext(_stateMachine);

            _stateMachine.Reset();
        }

        private void Update()
        {
            _stateMachine?.Update();
        }
    }
}