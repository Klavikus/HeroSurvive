using UnityEngine;

namespace CodeBase.SO
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private MoveStrategy.MoveStrategy _moveStrategy;
        [SerializeField] private float _checkDistance;

        private void Update()
        {
            transform.Translate(_moveStrategy.GetMoveVector(transform, _target.position, _checkDistance) *
                                (Time.deltaTime * _moveSpeed));
        }
    }
}