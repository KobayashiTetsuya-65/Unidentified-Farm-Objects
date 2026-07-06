using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class CuttleMutilation : MonoBehaviour
{
    [SerializeField] private Beam _beam;
    private CapsuleCollider _collider;
    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<ISuckable>(out var component))
        {
            _beam.RegisterSuckableObject(component);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ISuckable>(out var component))
        {
            _beam.RemoveSuckableObject(component);
        }
    }
}
