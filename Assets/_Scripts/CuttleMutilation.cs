using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class CuttleMutilation : MonoBehaviour
{
    CapsuleCollider _collider;
    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
