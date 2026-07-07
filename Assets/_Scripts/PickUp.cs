using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private UFOAnimation _ufo;
    [SerializeField] private Beam _beam;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<ISuckable>(out var component))
        {
            _beam.RemoveSuckableObject(component);
            component.Solve();
            component.PickUped();
            _ufo.GetAnimation();
        }
    }
}
