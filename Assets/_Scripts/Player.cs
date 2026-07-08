using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private UFOAnimation _ufoView;
    [SerializeField] private Beam _beam;
    [SerializeField] private Transform _tr;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private PlayerInput _playerInput;

    [Header("パラメーター調整")]
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _beamExpandSpeed = 0.5f;

    private InputAction _moveAction,_catchAction;
    private Vector2 _move;
    private bool _isMoving = true,_isCatch = false;
    private Coroutine _unfoldCorourine;
    private GameManager _gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameManager = GameManager.Instance;
        _moveAction = _playerInput.actions["Move"];
        _catchAction = _playerInput.actions["Jump"];
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.IsStop) return;

        PlayerInput();
    }
    private void FixedUpdate()
    {
        if (_gameManager.IsStop) return;

        Move();
    }

    private void PlayerInput()
    {
        if (_isMoving)
        {
            _move = _moveAction.ReadValue<Vector2>();

            if (_catchAction.WasPressedThisFrame() && !_beam.IsUnfoldong && !_isCatch)
            {
                _isCatch = true;
                _beam.BeamAnimation(_beamExpandSpeed,_isCatch);
            }
            else if(_catchAction.WasReleasedThisFrame() && _isCatch)
            {
                if (_unfoldCorourine != null) StopCoroutine(_unfoldCorourine);

                _unfoldCorourine = StartCoroutine(WaitUnfolding());
            }
        }
    }

    private void Move()
    {
        Vector3 move = new Vector3(_move.x * 0.5f + _move.y * 0.5f, 0f, -_move.x * 0.5f + _move.y * 0.5f);
        _rb.MovePosition(_rb.position + move.normalized * _speed * Time.deltaTime);
        _ufoView.ChangeDirection(move.normalized);
    }


    private IEnumerator WaitUnfolding()
    {
        yield return new WaitUntil(() => !_beam.IsUnfoldong);
        _isCatch = false;
        _beam.BeamAnimation(_beamExpandSpeed, _isCatch);
    }
}
