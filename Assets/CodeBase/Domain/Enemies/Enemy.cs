using System;
using System.Collections.Generic;
using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.Domain
{
    public class Enemy : MonoBehaviour
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

        public event Action<Enemy> Died;
        public event Action<Enemy> Destroyed;
        public event Action<Enemy> OutOfViewTimeout;
        public event Action<Enemy> InvokedBackToPool;

        public bool CanReceiveDamage => _damageable.CanReceiveDamage;
        public int KillExperience => _lootData.Experience;
        public int KillCurrency => _lootData.Currency;
        public EnemyType Type => _enemyData.Type;
        private void OnDestroy()
        {
            _stateMachine.Dispose();
            Destroyed?.Invoke(this);
        }

        public void InvokeBackToPool()
        {
            _damageable.Died -= OnDied;
            _enemyAI.AttackDistanceReached -= _damageSource.HandleAttack;
            InvokedBackToPool?.Invoke(this);
            gameObject.SetActive(false);
        }

        public void Initialize(ITargetService targetService, EnemyData enemyData)
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
            InitializeStateMachine();
        }

        private void OnDied() => Died?.Invoke(this);

        public void UpdateProgression(float completeProgress)
        {
            _enemyAI.UpdateProgression(_enemyData.ProgressionData.EnemyAI.Evaluate(completeProgress));
            _damageable.UpdateProgression(_enemyData.ProgressionData.Damageable.Evaluate(completeProgress));
            _damageSource.UpdateProgression(_enemyData.ProgressionData.DamageSource.Evaluate(completeProgress));
            _lootData.UpdateProgression(_enemyData.ProgressionData.Loot.Evaluate(completeProgress));
        }

        private void InitializeStateMachine()
        {
            IdleEntityState idleEntityState = new IdleEntityState(_animationSynchronizer, _enemyAI);
            RunEntityState runEntityState = new RunEntityState(_animationSynchronizer, _enemyAI);
            DieEntityState dieEntityState = new DieEntityState(_animationSynchronizer, _enemyAI, this);
            HitEntityState hitEntityState = new HitEntityState(_animationSynchronizer, _enemyAI, _damageable);

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

            _stateMachine = new EntityStateMachine(idleEntityState, _transitions);
            _stateMachine.Reset();
        }

        private void Update()
        {
            _stateMachine.Update();

            foreach (Transition transition in _transitions)
                transition.Update();
        }
    }
}