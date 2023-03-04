using System.Collections.Generic;
using ForSort;
using UnityEngine;

namespace CodeBase.Domain.EntityComponents
{
    public class AnimationSynchronizer
    {
        private static readonly int Attack = Animator.StringToHash("HandleAttack");
        private static readonly int Died = Animator.StringToHash("Died");
        private static readonly int Hitted = Animator.StringToHash("Hitted");
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Walk = Animator.StringToHash("Walk");
        
        private static readonly Dictionary<EntityState, int> _stateHashes = new()
        {
            {EntityState.Died, Died},
            {EntityState.Hitted, Hitted},
            {EntityState.Idle, Idle},
            {EntityState.Walk, Walk},
        };

        private readonly Animator _animator;

        public AnimationSynchronizer(Animator animator) => 
            _animator = animator;

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