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

    [Header("参照")]
    [SerializeField] protected Transform _tr;
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] private Animator _animator;

    bool ISuckable.IsSuction { get => _isSuction; }
    protected bool _isSuction = false;
    protected GameManager _gameManager;
    protected ScoreManager _scoreManager;
    private float _behaveTimer = 0;
    protected Vector3 _moveDir;

    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        _gameManager = GameManager.Instance;
        _scoreManager = ScoreManager.Instance;
    }
    protected virtual void Update()
    {
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
            Behavior();
        }
        else
        {
            ChangeDirection(_rb.linearVelocity.normalized);
        }
    }

    protected virtual void Behavior()
    {
        _behaveTimer -= Time.fixedDeltaTime;
        if(_behaveTimer <= 0)
        {
            //行動分岐
            _moveDir = (Random.value < _behaveProbability)? Vector3.zero
                : new Vector3(Random.Range(-1f,1f),0f,Random.Range(-1f,1f)).normalized;

            //タイマー再設定
            _behaveTimer = Random.Range(_behaveTimerMin, _behaveTimeMax);
        }

        _rb.MovePosition(_rb.position + _moveDir * _speed * Time.fixedDeltaTime);
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
            _tr.rotation = Quaternion.Slerp(_tr.rotation, target, 0.1f);
        }
        else
        {
            Quaternion target = Quaternion.LookRotation(moveDir);
            _tr.rotation = Quaternion.Slerp(_tr.rotation, target, 0.1f);
        }
    }
    public void Suction(Vector3 beamCenter, float power)
    {
        _isSuction = true;

        _rb.useGravity = false;
        Vector3 suck = (beamCenter - _tr.position).normalized;
        Vector3 pendulum = new Vector3(beamCenter.x - _tr.position.x,0f,
            beamCenter.z - _tr.position.z).normalized * _pendulumMag;
        _rb.AddForce((suck + pendulum) * (power / _weight),
            ForceMode.Acceleration);

        if (!_isSuction)
        {
            _isSuction = true;
            _rb.constraints = RigidbodyConstraints.None;
            _rb.AddTorque(Random.onUnitSphere * 1f, ForceMode.VelocityChange);
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
        _scoreManager.AddScore(_score);
        _gameManager.ChangeEnergy(_energy,true);
        gameObject.SetActive(false);
    }
}
