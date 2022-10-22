using System.Collections;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] private InputHandler _inputController;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private MoveStrategy _moveStrategy;

    private bool FacedRight = true;
    private Vector3 previousPosition;

    public Vector3 LastMoveVector { get; private set; }

    private void OnEnable()
    {
        _inputController.InputUpdated += OnInputUpdated;
        StartCoroutine(TrackDirection());
    }

    private IEnumerator TrackDirection()
    {
        previousPosition = transform.position;
        yield return new WaitForSeconds(0.35f);
    }

    private void OnDisable()
    {
        _inputController.InputUpdated -= OnInputUpdated;
    }

    private void OnInputUpdated(InputData inputData)
    {
        PhysicsMove(inputData.Horizontal, inputData.Vertical);
    }

    private void PhysicsMove(float inputDataHorizontal, float inputDataVertical)
    {
        Vector3 direction = new Vector3(inputDataHorizontal, inputDataVertical, 0).normalized;

        if (CheckFlipNeeding(inputDataHorizontal))
            Flip();

        _rigidbody2D.velocity = direction * _moveSpeed;

        if ((transform.position - previousPosition).magnitude > 0.2f)
        {
            LastMoveVector = (transform.position - previousPosition).normalized;
            previousPosition = transform.position;
        }
    }

    private bool CheckFlipNeeding(float inputDataHorizontal) =>
        (inputDataHorizontal < 0 && FacedRight) || (inputDataHorizontal > 0 && !FacedRight);

    private void Flip()
    {
        FacedRight = !FacedRight;
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
}