using UnityEngine;

namespace CodeBase.Domain
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _zOffset;

        private void Update()
        {
            transform.position = new Vector3(_target.position.x, _target.position.y, _zOffset);
        }
    }
}