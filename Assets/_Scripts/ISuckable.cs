using UnityEngine;

public interface ISuckable
{
    public bool IsSuction {  get; }
    void Suction(Vector3 beamCenter,float power);
    void Solve();
    void PickUped();
}
