using System;
using UnityEngine;

public class AnimationSynchronizer : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private EnemyAI _type;
    private static readonly int Died = Animator.StringToHash("Died");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Attack = Animator.StringToHash("Attack");

    private void Start()
    {
        _type.StateChanged += OnStateChanged;
    }

    private void OnStateChanged(EntityState newState)
    {
        ResetAnimatroBools();
        switch (newState)
        {
            case EntityState.Died:
                _animator.SetBool(Died, true);
                break;
            case EntityState.Idle:
                _animator.SetBool(Idle, true);
                break;
            case EntityState.Walk:
                _animator.SetBool(Walk, true);
                break;
            case EntityState.Attack:
                _animator.SetBool(Attack, true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    private void ResetAnimatroBools()
    {
        _animator.SetBool(Died, false);
        _animator.SetBool(Idle, false);
        _animator.SetBool(Walk, false);
        _animator.SetBool(Attack, false);
    }
}