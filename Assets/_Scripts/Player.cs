using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("参照")]
    [Header("パラメーター調整")]
    [SerializeField] private float _speed = 3f;

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private Transform _tr;
    private Vector2 _move;
    private bool _isMoving = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tr = transform;

        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
    }
    private void LateUpdate()
    {
        Move();
    }

    private void PlayerInput()
    {
        if (_isMoving)
        {
            _move = _moveAction.ReadValue<Vector2>();
        }
    }

    private void Move()
    {
        Vector3 move = new Vector3(_move.x * 0.5f + _move.y * 0.5f, 0f, -_move.x * 0.5f + _move.y * 0.5f);
        _tr.position += move.normalized * _speed * Time.deltaTime;
    }
}
