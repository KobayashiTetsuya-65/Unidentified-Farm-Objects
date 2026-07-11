using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private UFOAnimation _ufo;
    [SerializeField] private Beam _beam;
    [SerializeField] private ParticleSystem _particle;

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

            if(other.TryGetComponent<Bomb>(out var bomb))
            {
                _particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                _particle.Play();
                AudioManager.Instance.PlaySE(SEType.Bomb);
            }
        }
    }
}
