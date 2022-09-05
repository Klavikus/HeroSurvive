using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] private InputHandler _inputController;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    private bool FacedRight = true;
    private Vector3 direction;

    private void OnEnable()
    {
        _inputController.InputUpdated += OnInputUpdated;
    }

    private void OnDisable()
    {
        _inputController.InputUpdated -= OnInputUpdated;
    }

    private void OnInputUpdated(InputData inputData)
    {
        PhysicsMove(inputData.Horizontal, inputData.Vertical);
        // Move(inputData.Horizontal, inputData.Vertical);
    }

    private void PhysicsMove(float inputDataHorizontal, float inputDataVertical)
    {
        Vector3 direction = new Vector3(inputDataHorizontal, inputDataVertical, 0).normalized;

        if (CheckFlipNeeding(inputDataHorizontal))
            Flip();

        _rigidbody2D.velocity = direction * _moveSpeed;
    }

    private void Move(float inputDataHorizontal, float inputDataVertical)
    {
        Vector3 direction = new Vector3(inputDataHorizontal, inputDataVertical, 0).normalized;

        if (CheckFlipNeeding(inputDataHorizontal))
            Flip();

        transform.Translate(direction * (_moveSpeed * Time.deltaTime), Space.World);
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