using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class CharacterBase : MonoBehaviour,ISuckable
{
    [Header("ƒpƒ‰ƒ[ƒ^")]
    [SerializeField] private int _score = 100;
    [SerializeField] private float _energy = 180f;
    [SerializeField] private float _weight = 5f;
    [SerializeField] private float _pendulumMag = 1f;
    [SerializeField] protected Transform _tr;
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] private Animator _animator;

    bool ISuckable.IsSuction { get => _isSuction; }
    protected bool _isSuction = false;
    protected GameManager _gameManager;
    protected ScoreManager _scoreManager;

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
            _animator.SetFloat("Speed", _rb.linearVelocity.magnitude);
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
    }
    public void Solve()
    {
        _isSuction = false;
        _rb.linearVelocity = Vector3.zero;
        _rb.useGravity = true;
    }

    public void PickUped()
    {
        _scoreManager.AddScore(_score);
        _gameManager.ChangeEnergy(_energy,true);
        gameObject.SetActive(false);
    }
}
