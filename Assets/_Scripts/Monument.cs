using UnityEngine;

public class Monument : CharacterBase
{
    [SerializeField] private float _needPower = 0f;
    public override void PickUped()
    {
        base.PickUped();
        gameObject.SetActive(false);
    }

    public override void Suction(Vector3 beamCenter, float power)
    {
        if (power <= _needPower) return;

        base.Suction(beamCenter, power);
    }
}
