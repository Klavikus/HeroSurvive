using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : InputHandler
{
    [SerializeField] private Player _player;
    [SerializeField] private float MinError;
    [SerializeField] private float _distance;

    private RaycastHit2D[] _hits = new RaycastHit2D[2];

    private void Update()
    {
        Vector3 direction = _player.transform.position - transform.position;

        if (direction.magnitude > MinError)
        {
            direction.Normalize();

            InvokeInputUpdated(new InputData(direction.x, direction.y));
        }
    }

    private bool CheckPath(Vector3 direction)
    {
        Debug.DrawLine(transform.position, transform.position + direction.normalized * _distance, Color.green, 1f);
        return Physics2D.RaycastNonAlloc(transform.position, direction.normalized * _distance, _hits, _distance) <= 1;
    }
}