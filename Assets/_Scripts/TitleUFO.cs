using DG.Tweening;
using UnityEngine;

public class TitleUFO : MonoBehaviour
{
    [SerializeField] private Transform _tr;
    [SerializeField] private float _moveSpeed = 1.2f;
    [SerializeField] private float _duration = 1.5f;
    [SerializeField] private Vector3 _backPos = Vector3.zero;
    [SerializeField] private Vector3 _frontPos = Vector3.zero;
    [SerializeField] private Vector3 _earthPos = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tr.localPosition = _backPos;
        MoveAnimation();
    }

    private void MoveAnimation()
    {
        _tr.DOLocalMove(RandomPos(), _moveSpeed)
            .SetSpeedBased()
            .SetEase(Ease.InOutSine)
            .SetLink(gameObject)
            .OnComplete(MoveAnimation);
    }

    private Vector3 RandomPos() => new Vector3(
        Random.Range(_backPos.x, _frontPos.x),
        Random.Range(_backPos.y, _frontPos.y),
        Random.Range(_backPos.z, _frontPos.z));

    public void StartAnimation(System.Action onComplete)
    {
        _tr.DOKill();
        Sequence seq = DOTween.Sequence();
        seq.Append(_tr.DOLocalMove(_backPos, _duration * 0.5f));
        seq.Append(_tr.DOLocalMove(_earthPos, _duration * 0.5f))
            .OnComplete(() => onComplete?.Invoke());
    }
}
