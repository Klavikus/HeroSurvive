using System.Collections;
using GameCore.Source.Domain.Data;
using Modules.Common.Utils;
using UnityEngine;

namespace GameCore.Source.Domain.EntityComponents
{
    public class MoveController : MonoBehaviour
    {
        [SerializeField] private InputHandler _inputController;
        [SerializeField] private float _baseMoveSpeed;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private LayerMask _whatIsWall;

        private readonly WaitForSeconds _directionTrackingDelay = new(GameConstants.DirectionTrackingDelay);
        private readonly float _minimumStopDistance = GameConstants.MinimumStopDistance;
        private readonly RaycastHit2D[] _wallCheckResults = new RaycastHit2D[1];

        private bool _facedRight = true;
        private Vector3 _previousPosition;
        private float _resultMoveSpeed;
        private bool _stopped;

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
            _resultMoveSpeed = _baseMoveSpeed * moveSpeedModifier.AsPercentFactor();
            _inputController.Enable();
        }

        //TODO: Refactor this
        private IEnumerator TrackDirection()
        {
            _previousPosition = transform.position;

            yield return _directionTrackingDelay;
        }

        private void OnInputUpdated(InputData inputData) =>
            HandlePhysicsMove(inputData.Horizontal, inputData.Vertical);

        private void HandlePhysicsMove(float inputDataHorizontal, float inputDataVertical)
        {
            if (_stopped)
                return;

            Vector3 direction = new Vector3(inputDataHorizontal, inputDataVertical, 0).normalized;

            if (CheckFlipNeeding(inputDataHorizontal))
                Flip();

            if (CheckWall(direction))
            {
                _rigidbody2D.velocity = Vector2.zero;

                return;
            }

            _rigidbody2D.velocity = direction * _resultMoveSpeed;

            if ((transform.position - _previousPosition).magnitude > _minimumStopDistance)
            {
                LastMoveVector = (transform.position - _previousPosition).normalized;
                _previousPosition = transform.position;
            }
        }

        private bool CheckWall(Vector3 direction) =>
            Physics2D.RaycastNonAlloc(transform.position, direction, _wallCheckResults, 0.5f, _whatIsWall) > 0;

        private bool CheckFlipNeeding(float inputDataHorizontal) =>
            (inputDataHorizontal < 0 && _facedRight) || (inputDataHorizontal > 0 && !_facedRight);

        private void Flip()
        {
            _spriteRenderer.flipX = _facedRight;
            _facedRight = !_facedRight;
        }

        public void Enable()
        {
            _inputController.Enable();
            _stopped = false;
        }

        public void Disable()
        {
            _inputController.Disable();
            _rigidbody2D.velocity = Vector2.zero;
            _stopped = true;
        }
    }
}