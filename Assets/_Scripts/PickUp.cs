using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private UFOAnimation _ufo;
    [SerializeField] private Beam _beam;

    private GameManager _gamaManager;
    private void OnTriggerEnter(Collider other)
    {
        if (_gamaManager == null)
            _gamaManager = GameManager.Instance;

        if (_gamaManager.IsStop) return;
            
        if (other.TryGetComponent<ISuckable>(out var component))
        {
            _beam.RemoveSuckableObject(component);
            component.Solve();
            component.PickUped();
            _ufo.GetAnimation();
        }
    }
}
