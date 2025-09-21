using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    [SerializeField, Min(1f)] private float _moveSpeed;
    [SerializeField, Min(1f)] private float _jumpHeight;

    private Vector2 _moveDirection;
    private Rigidbody2D _rigidbody;
    private LayerMask _groundLayer;
    private bool _isRight = true;

    public event UnityAction MoveEnabled;
    public event UnityAction MoveDisabled;
    public event UnityAction JumpEnabled;

    public bool IsGrounded { get; private set; }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundLayer = LayerMask.NameToLayer("Ground");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();

        if (_moveDirection == Vector2.zero )
        {
            MoveDisabled?.Invoke();
        }
        else
        {
            MoveEnabled?.Invoke();
        }

        if (_moveDirection == Vector2.right)
        {
            _isRight = true;
        }
        else if (_moveDirection == Vector2.left)
        {
            _isRight = false;
            _moveDirection = -_moveDirection;
        }
    }

    public void OnJump(InputAction.CallbackContext context) =>
        Jump();

    private void Update()
    {
        IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, ~_groundLayer);

        Move(_moveDirection * _moveSpeed * Time.deltaTime);
        Rotate();
    }

    private void Rotate()
    {
        quaternion rotation = transform.rotation;
        float rortationRight = 0f;
        float rotationLeft = 180f;

        if (_isRight)
        {
            rotation.value.y = rortationRight;
        }
        else
        {
            rotation.value.y = rotationLeft;
        }

        transform.rotation = rotation;
    }

    private void Move(Vector2 direction) =>
        transform.Translate(direction);

    private void Jump()
    {
        if (IsGrounded)
        {
            _rigidbody.AddForce(Vector2.up * _jumpHeight, ForceMode2D.Impulse);

            JumpEnabled?.Invoke();
        }
    }
}