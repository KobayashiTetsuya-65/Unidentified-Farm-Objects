using DG.Tweening;
using UnityEngine;

public class UFOAnimation : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private Transform _tr;

    [Header("パラメータ")]
    [SerializeField] private float _rotateDur = 0.1f;
    [SerializeField] private float _getDur = 0.3f;
    [SerializeField] private float _getScale = 1.2f;

    private Sequence _getSeq;
    private Vector3 _scale;

    private void Awake()
    {
        _scale = _tr.localScale;
    }

    public void ChangeDirection(Vector3 moveDir)
    {
        if (moveDir.sqrMagnitude < 0.001f) return;

        Quaternion target = Quaternion.LookRotation(moveDir);
        _tr.rotation = Quaternion.Slerp(_tr.rotation, target, _rotateDur);
    }

    public void GetAnimation()
    {
        if(_getSeq != null)
        {
            _getSeq.Kill();
        }

        _getSeq = DOTween.Sequence();
        _getSeq.Append(_tr.DOScale(_scale * _getScale, _getDur * 0.7f)).SetEase(Ease.OutQuart);
        _getSeq.Append(_tr.DOScale(_scale,_getDur * 0.3f)).SetEase(Ease.InOutQuart);
        //_getSeq.Join(_tr.DOShakeRotation(_getDur * 0.5f,30,10,50));
    }
}
