using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")] [SerializeField] private float accelerationForce = 18f;
    [SerializeField] private float maxSpeed = 8f;

    [Header("Drag")] [SerializeField] private float linearDragWithInput = 0.5f;
    [SerializeField] private float linearDragNoInput = 4f;

    [Header("Input")] [SerializeField] private string moveActionName = "Move";
    [SerializeField] private bool enableKeyboardFallback = true;

    private Rigidbody _playerRigidbody;
    private int _count = 0;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private Vector2 _moveInput;

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        ResolveMoveAction();
    }

    private void Start()
    {
        _count = 0;

        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null && GameManager.Instance != null)
            GameManager.Instance.AlocarInput(playerInput);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp") )
        {
            other.gameObject.SetActive(false);
            _count++;

            // Notifica o canal Observer com o total atualizado
            PlayerOM.NotifyCoinCollected(_count);

            Debug.Log($"[PlayerController] Moeda coletada! Total: {_count}");
        }
    }

    private void ResolveMoveAction()
    {
        _moveAction = null;
        if (_playerInput == null) return;

        if (_playerInput.actions != null)
            _moveAction = _playerInput.actions.FindAction(moveActionName);

        if (_moveAction == null && _playerInput.currentActionMap != null)
            _moveAction = _playerInput.currentActionMap.FindAction(moveActionName);
    }

    private Vector2 ReadMoveInput()
    {
        if (_moveAction != null)
            return _moveAction.ReadValue<Vector2>();

        return enableKeyboardFallback ? ReadKeyboardFallbackInput() : Vector2.zero;
    }

    private static Vector2 ReadKeyboardFallbackInput()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return Vector2.zero;

        float x = 0f, y = 0f;
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) x -= 1f;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) x += 1f;
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) y -= 1f;
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) y += 1f;
        return Vector2.ClampMagnitude(new Vector2(x, y), 1f);
    }

    private void ApplyDragByInputState()
    {
        bool hasInput = _moveInput.sqrMagnitude > 0.0001f;
        _playerRigidbody.linearDamping = hasInput ? linearDragWithInput : linearDragNoInput;
    }

    private void ApplyMovementForce()
    {
        Vector3 dir = new Vector3(_moveInput.x, 0f, _moveInput.y);
        if (dir.sqrMagnitude > 1f) dir.Normalize();
        _playerRigidbody.AddForce(dir * accelerationForce, ForceMode.Acceleration);
    }

    private void ClampHorizontalSpeed()
    {
        Vector3 vel = _playerRigidbody.linearVelocity;
        Vector3 hVel = new Vector3(vel.x, 0f, vel.z);
        if (hVel.sqrMagnitude <= maxSpeed * maxSpeed) return;
        hVel = hVel.normalized * maxSpeed;
        _playerRigidbody.linearVelocity = new Vector3(hVel.x, vel.y, hVel.z);
    }
}
