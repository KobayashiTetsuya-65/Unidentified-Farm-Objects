using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class CharacterBase : MonoBehaviour,ISuckable
{
    [Header("パラメータ")]
    [SerializeField] private int _score = 100;
    [SerializeField] private float _energy = 180f;
    [SerializeField] private float _weight = 5f;
    [SerializeField] private float _pendulumMag = 1f;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _behaveTimerMin = 1f;
    [SerializeField] private float _behaveTimeMax = 5f;
    [SerializeField] private float _behaveProbability = 0.5f;
    [SerializeField] private bool _freezeRotateXZ = true;
    [SerializeField] private float _rotateSpeed = 8f;
    [SerializeField] private float _horizontalDamp = 1f;
    [SerializeField] private bool _alignHeadToBeam = false;
    [SerializeField] private bool _isMoveObject = false;

    [Header("参照")]
    [SerializeField] protected Transform _tr;
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] private Animator _animator;

    bool ISuckable.IsSuction { get => _isSuction; }
    protected bool _isSuction = false;
    protected GameManager _gameManager;
    protected AudioManager _audioManager;
    protected ScoreManager _scoreManager;
    private float _behaveTimer = 0;
    protected Vector3 _moveDir;
    private float _fixedDeltaTime;
    protected Action<CharacterBase> _onDespawn;

    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        _gameManager = GameManager.Instance;
        _scoreManager = ScoreManager.Instance;
        _audioManager = AudioManager.Instance;
    }
    protected virtual void Update()
    {
        if (!_isMoveObject) return;
        if (_animator == null) return;

        _animator.SetBool("IsSuction", _isSuction);
        if (!_isSuction)
        {
            _animator.SetFloat("Speed", _moveDir == Vector3.zero ? 0f : _speed);
        }

        
    }
    protected virtual void FixedUpdate()
    {
        if (!_isSuction)
        {
            _fixedDeltaTime = Time.fixedDeltaTime;
            if (!_isMoveObject) return;
            Behavior();
        }
    }
    public virtual void Init(Action<CharacterBase> onDespown)
    {
        _onDespawn = onDespown;
    }

    protected virtual void Behavior()
    {
        _behaveTimer -= _fixedDeltaTime;
        if(_behaveTimer <= 0)
        {
            //行動分岐
            _moveDir = (UnityEngine.Random.value < _behaveProbability)? Vector3.zero
                : new Vector3(UnityEngine.Random.Range(-1f,1f),0f,UnityEngine.Random.Range(-1f,1f)).normalized;

            //タイマー再設定
            _behaveTimer = UnityEngine.Random.Range(_behaveTimerMin, _behaveTimeMax);
        }

        _rb.MovePosition(_rb.position + _moveDir * _speed * _fixedDeltaTime);
        ChangeDirection(_moveDir);
    }
    protected virtual void ChangeDirection(Vector3 moveDir)
    {
        if (moveDir.sqrMagnitude < 0.001f) return;

        if (_freezeRotateXZ)
        {
            Vector3 flat = new Vector3(moveDir.x, 0f, moveDir.z);
            if (flat.sqrMagnitude < 0.001f) return;

            Quaternion target = Quaternion.LookRotation(flat);
            _tr.rotation = Quaternion.Slerp(_tr.rotation, target, _rotateSpeed * _fixedDeltaTime);
        }
        else
        {
            Quaternion target = Quaternion.LookRotation(moveDir);
            _tr.rotation = Quaternion.Slerp(_tr.rotation, target, _rotateSpeed * _fixedDeltaTime);
        }
    }
    public void Suction(Vector3 beamCenter, float power)
    {
        if (!_isSuction)
        {
            _isSuction = true;
            if (!_alignHeadToBeam)
            {
                _rb.constraints = RigidbodyConstraints.None;
                _rb.AddTorque(UnityEngine.Random.onUnitSphere * 1f, ForceMode.VelocityChange);
            }
        }

        _rb.useGravity = false;
        Vector3 toCenter = beamCenter - _rb.position;
        Vector3 horizontal = new Vector3(toCenter.x, 0f, toCenter.z);
        Vector3 hVel = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

        Vector3 force = Vector3.up * power + horizontal * _pendulumMag  - hVel * _horizontalDamp;

        _rb.AddForce(force / _weight, ForceMode.Acceleration);

        if (_alignHeadToBeam)
        {
            Quaternion target = Quaternion.FromToRotation(Vector3.up, toCenter.normalized)
                              * Quaternion.Euler(0f, _tr.eulerAngles.y, 0f);
            _tr.rotation = Quaternion.Slerp(_tr.rotation, target, _rotateSpeed * _fixedDeltaTime);
        }

    }
    public void Solve()
    {
        _isSuction = false;
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _tr.rotation = Quaternion.Euler(0f, _tr.eulerAngles.y, 0f);
        _rb.useGravity = true;
    }

    public void PickUped()
    {
        _audioManager.PlaySE(SEType.PickUp);
        _scoreManager.AddScore(_score);
        _scoreManager.Grow(_score);
        _gameManager.ChangeEnergy(_energy,true);
        _onDespawn?.Invoke(this);
    }

    public void ResetState()
    {
        _behaveTimer = 0;
        _moveDir = Vector3.zero;
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _rb.useGravity = true;
    }
}
