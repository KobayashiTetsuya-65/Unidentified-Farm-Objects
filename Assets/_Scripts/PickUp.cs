using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private UFOAnimation _ufo;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<ISuckable>(out var component))
        {
            component.PickUped();
            _ufo.GetAnimation();
        }
    }
}
