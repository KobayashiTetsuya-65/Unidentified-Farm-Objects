using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class CharacterBase : MonoBehaviour,ISuckable
{
    [Header("ƒpƒ‰ƒ[ƒ^")]
    [SerializeField] private int _score = 100;
    [SerializeField] private float _weight = 5f;
    [SerializeField] private float _pendulumMag = 1f;
    [SerializeField] protected Transform _tr;
    [SerializeField] protected Rigidbody _rb;

    bool ISuckable.IsSuction { get => _isSuction; }
    protected bool _isSuction = false;

    protected virtual void Awake()
    {

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
        ScoreManager.Instance.AddScore(_score);
        gameObject.SetActive(false);
    }
}
