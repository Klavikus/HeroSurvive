using System;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshMover : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _transform;

    private float _radius = 0.2f;
    private RaycastHit2D[] _results = new RaycastHit2D[3];

    private void Start()
    {
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
    }

    private void Update()
    {
        _navMeshAgent.SetDestination(CheckOverlap() == false ? _transform.position : transform.position);
    }

    private bool CheckOverlap()
    {
        var count = Physics2D.RaycastNonAlloc(transform.position, _navMeshAgent.velocity.normalized, _results, 1);
        Debug.Log(_navMeshAgent.velocity.normalized);
        Debug.Log(count);
        return count > 1;
    }
}