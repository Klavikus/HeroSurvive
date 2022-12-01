using System.Collections;
using CodeBase.Domain.Data;
using UnityEngine;

namespace CodeBase.Domain.EntityComponents
{
    public class MoveController : MonoBehaviour
    {
        [SerializeField] private InputHandler _inputController;
        [SerializeField] private float _baseMoveSpeed;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private bool _facedRight = true;
        private Vector3 _previousPosition;
        private float _resultMoveSpeed;

        public Vector3 LastMoveVector { get; private set; }

        private void OnEnable()
        {
            LastMoveVector = Vector3.right;
            _inputController.InputUpdated += OnInputUpdated;
            StartCoroutine(TrackDirection());
        }

        private void OnDisable()
        {
            _inputController.InputUpdated -= OnInputUpdated;
        }

        public void Initialize(float moveSpeedModifier)
        {
            _resultMoveSpeed = _baseMoveSpeed * moveSpeedModifier / 100;
        }

        //TODO: Refactor this
        private IEnumerator TrackDirection()
        {
            _previousPosition = transform.position;
            yield return new WaitForSeconds(0.35f);
        }

        private void OnInputUpdated(InputData inputData) => 
            PhysicsMove(inputData.Horizontal, inputData.Vertical);

        private void PhysicsMove(float inputDataHorizontal, float inputDataVertical)
        {
            Vector3 direction = new Vector3(inputDataHorizontal, inputDataVertical, 0).normalized;

            if (CheckFlipNeeding(inputDataHorizontal))
                Flip();

            _rigidbody2D.velocity = direction * _resultMoveSpeed;

            if ((transform.position - _previousPosition).magnitude > 0.2f)
            {
                LastMoveVector = (transform.position - _previousPosition).normalized;
                _previousPosition = transform.position;
            }
        }

        private bool CheckFlipNeeding(float inputDataHorizontal) =>
            (inputDataHorizontal < 0 && _facedRight) || (inputDataHorizontal > 0 && !_facedRight);

        private void Flip()
        {
            _spriteRenderer.flipX = _facedRight;
            _facedRight = !_facedRight;
        }
    }
}