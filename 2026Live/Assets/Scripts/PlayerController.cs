using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float accelerationForce = 18f;
    [SerializeField] private float maxSpeed = 8f;

    [Header("Drag")]
    [SerializeField] private float linearDragWithInput = 0.5f;
    [SerializeField] private float linearDragNoInput = 4f;

    [Header("Input")]
    [SerializeField] private string moveActionName = "Move";
    [SerializeField] private bool enableKeyboardFallback = true;

    private Rigidbody _playerRigidbody;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private Vector2 _moveInput;

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        ResolveMoveAction();
    }

    private void OnEnable()
    {
        ResolveMoveAction();
    }

    private void Update()
    {
        _moveInput = ReadMoveInput();
    }

    private void FixedUpdate()
    {
        ApplyDragByInputState();
        ApplyMovementForce();
        ClampHorizontalSpeed();
    }

    private void ResolveMoveAction()
    {
        _moveAction = null;

        if (_playerInput == null)
        {
            return;
        }

        if (_playerInput.actions != null)
        {
            _moveAction = _playerInput.actions.FindAction(moveActionName);
        }

        if (_moveAction == null && _playerInput.currentActionMap != null)
        {
            _moveAction = _playerInput.currentActionMap.FindAction(moveActionName);
        }
    }

    private Vector2 ReadMoveInput()
    {
        if (_moveAction != null)
        {
            return _moveAction.ReadValue<Vector2>();
        }

        return enableKeyboardFallback ? ReadKeyboardFallbackInput() : Vector2.zero;
    }

    private static Vector2 ReadKeyboardFallbackInput()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return Vector2.zero;
        }

        float x = 0f;
        float y = 0f;

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            x -= 1f;
        }

        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            x += 1f;
        }

        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
        {
            y -= 1f;
        }

        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
        {
            y += 1f;
        }

        return Vector2.ClampMagnitude(new Vector2(x, y), 1f);
    }

    private void ApplyDragByInputState()
    {
        bool hasInput = _moveInput.sqrMagnitude > 0.0001f;
        _playerRigidbody.linearDamping = hasInput ? linearDragWithInput : linearDragNoInput;
    }

    private void ApplyMovementForce()
    {
        Vector3 worldMoveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y);
        if (worldMoveDirection.sqrMagnitude > 1f)
        {
            worldMoveDirection.Normalize();
        }

        _playerRigidbody.AddForce(worldMoveDirection * accelerationForce, ForceMode.Acceleration);
    }

    private void ClampHorizontalSpeed()
    {
        Vector3 velocity = _playerRigidbody.linearVelocity;
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);

        if (horizontalVelocity.sqrMagnitude <= maxSpeed * maxSpeed)
        {
            return;
        }

        horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
        _playerRigidbody.linearVelocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.z);
    }
}
