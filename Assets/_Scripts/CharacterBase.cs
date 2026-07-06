using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class CharacterBase : MonoBehaviour,ISuckable
{
    [Header("ƒpƒ‰ƒ[ƒ^")]
    [SerializeField] private float _weight = 5f;
    [SerializeField] protected Transform _tr;
    [SerializeField] protected Rigidbody _rb;

    bool ISuckable.IsSuction { get => _isSuction; }
    protected bool _isSuction = false;

    protected virtual void Awake()
    {

    }
    public void Suction(Vector3 beamCenter, float power)
    {
        if (!_isSuction) _isSuction = true;

        _rb.useGravity = false;
        _rb.AddForce((beamCenter - _tr.position).normalized * (power / _weight),
            ForceMode.Acceleration);
    }
    public void Solve()
    {
        _isSuction = false;
        _rb.linearVelocity = Vector3.zero;
        _rb.useGravity = true;
    }
}
