using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.ForSort
{
    public class AnimationSynchronizer
    {
        private static readonly int Attack = Animator.StringToHash("HandleAttack");
        private static readonly int Died = Animator.StringToHash("Died");
        private static readonly int Hitted = Animator.StringToHash("Hitted");
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Walk = Animator.StringToHash("Walk");

        private readonly Animator _animator;

        private readonly Dictionary<EntityState, int> _stateHashes;

        public AnimationSynchronizer(Animator animator)
        {
            _animator = animator;
            _stateHashes = new Dictionary<EntityState, int>
            {
                // {EntityState.Attack, Attack},
                {EntityState.Died, Died},
                {EntityState.Hitted, Hitted},
                {EntityState.Idle, Idle},
                {EntityState.Walk, Walk},
            };
        }

        public void ChangeState(EntityState newState)
        {
            ResetAnimatorParams();
            _animator.SetBool(_stateHashes[newState], true);
        }

        private void ResetAnimatorParams()
        {
            _animator.SetBool(Died, false);
            _animator.SetBool(Hitted, false);
            _animator.SetBool(Idle, false);
            _animator.SetBool(Walk, false);
        }
    }
}