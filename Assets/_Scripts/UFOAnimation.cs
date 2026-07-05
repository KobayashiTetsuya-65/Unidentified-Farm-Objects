using DG.Tweening;
using UnityEngine;

public class UFOAnimation : MonoBehaviour
{
    [SerializeField] private float _duration = 0.1f;
    private Transform _tr;
    private void Awake()
    {
        _tr = transform;
    }

    public void ChangeDirection(Vector3 moveDir)
    {
        if (moveDir.sqrMagnitude < 0.001f) return;

        Quaternion target = Quaternion.LookRotation(moveDir);
        _tr.rotation = Quaternion.Slerp(_tr.rotation, target, _duration);
    }
}
