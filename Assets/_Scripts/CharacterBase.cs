using UnityEngine;

public abstract class CharacterBase : MonoBehaviour,ISuckable
{
    [Header("ƒpƒ‰ƒ[ƒ^")]
    [SerializeField] private float _weight = 5f;

    bool ISuckable.IsSuction { get => _isSuction; }
    protected bool _isSuction = false;
    protected Rigidbody _rb;
    protected Transform _tr;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _tr = transform;
    }
    public void Suction(Vector3 beamCenter, float power)
    {
        if (!_isSuction) _isSuction = true;

        _rb.linearVelocity = (beamCenter - _tr.position).normalized * (power / _weight);
    }
}
