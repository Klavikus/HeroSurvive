using System;
using CodeBase;
using UnityEngine;

public class EnemyAI : InputHandler, IEntity
{
    [SerializeField] private MoveStrategy _moveStrategy;
    [SerializeField] private float _speed;
    [SerializeField] private float _distance;
    [SerializeField] private bool _isFacedRight;
    [SerializeField] private float _attackRange;
    [SerializeField] private AbilityHandler _abilityHandler;

    private EntityState _currentState;
    private TargetService _targetService;
    public event Action<EntityState> StateChanged;
    public event Action<EnemyAI> Die;

    private void OnDestroy()
    {
        Die?.Invoke(this);
    }

    public void Initialize(TargetService targetService)
    {
        _targetService = targetService;
        enabled = true;
    }

    private void Update()
    {
        Vector3 moveVector = _moveStrategy.GetMoveVector(transform, _targetService.GetPlayerPosition(), _distance);
        Move(moveVector, _speed);
    }

    private void Move(Vector3 direction, float speed)
    {
        Vector3 directionToTarget = _targetService.GetPlayerPosition()- transform.position;

        if (directionToTarget.magnitude > _attackRange)
        {
            transform.Translate(direction * (speed * Time.deltaTime), Space.World);
        }
        else
        {
            // _abilityHandler.HandleAttack();
        }

        if ((directionToTarget.x < 0 && _isFacedRight) || (directionToTarget.x > 0 && !_isFacedRight))
            Flip();

        if (directionToTarget.magnitude > _attackRange && _currentState != EntityState.Walk)
        {
            _currentState = EntityState.Walk;
            StateChanged?.Invoke(_currentState);
        }
    }

    private void Flip()
    {
        _isFacedRight = !_isFacedRight;
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
}