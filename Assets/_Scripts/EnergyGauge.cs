using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGauge : MonoBehaviour
{
    public float CurrentTime => _currenTime;

    [Header("参照")]
    [SerializeField] private RectTransform _rt;
    [SerializeField] private Image _gaugeImg;

    [Header("パラメータ")]
    [SerializeField] private float _maxTime = 3600;

    private float _currenTime;
    private Tween _gaugeTween;
    private Sequence _seq;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.RegisterGauge(this);
        _currenTime = _maxTime;
    }
    /// <summary>
    /// エネルギーゲージの変化
    /// </summary>
    /// <param name="delta">減少量</param>
    public void ChangeGauge(float delta)
    {
        float start = _currenTime;
        float goal = Mathf.Clamp(_currenTime - delta, 0f, _maxTime);
        _currenTime = goal;

        if (_gaugeTween != null)
            _gaugeTween.Kill();

        //増加演出
        if (delta < 0f)
        {
            if (_seq != null)
                _seq.Kill();

            _seq = DOTween.Sequence();
            _seq.Append(_rt.DOScale(1.1f, 0.2f));
            _seq.Append(_rt.DOScale(1.0f, 0.1f)).SetEase(Ease.InOutQuad);
        }

        //ゲージアニメーション
        _gaugeTween = DOTween.To(() => start,
            x =>
            {
                start = x;
                _gaugeImg.fillAmount = start / _maxTime;
            },
            goal,
            0.1f)
            .SetEase(Ease.OutQuad);
    }
}
