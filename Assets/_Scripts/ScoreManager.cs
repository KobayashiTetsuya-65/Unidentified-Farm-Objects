using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("参照")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private TextMeshProUGUI _resultScore;
    [SerializeField] private GameObject _scorePanel;
    [SerializeField] private Image _readyImg;
    [SerializeField] private Image _goImg;

    [Header("パラメータ")]
    [SerializeField] private float _duration = 0.3f;
    [SerializeField] private float _resultDuration = 2f;
    public int CurrentScore
    {
        get => _currentScore;
        private set
        {
            int start = _currentScore;
            int goal = value;
            _currentScore = value;

            if(_scoreTween != null)
                _scoreTween.Kill();

            _scoreTween = DOTween.To(() => start,
                x =>
                {
                    start = x;
                    _scoreText.text = $"{start:D9}";
                },
                goal,
                _duration)
                .SetLink(gameObject);
        }
    }

    private Tween _scoreTween;
    private int _currentScore = 0;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _readyImg.gameObject.SetActive(false);
        _goImg.gameObject.SetActive(false);
        _resultPanel.SetActive(false);
    }

    public void AddScore(int delta)
    {
        CurrentScore = Mathf.Max(0, CurrentScore + delta);
    }

    public void StartAnimation()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.AppendCallback(()  => _readyImg.gameObject.SetActive(true));
        seq.Append(_readyImg.DOFade(1f, 0.2f));
        seq.Join(_readyImg.rectTransform.DOScale(1f, 0.2f));
        seq.Append(_readyImg.rectTransform.DOScale(1.5f, 2.5f));
        seq.Append(_readyImg.rectTransform.DOScale(0.1f, 0.1f));
        seq.AppendCallback(() =>
        {
            _readyImg.gameObject.SetActive(false);
            _goImg.gameObject.SetActive(true);
        });
        seq.Append(_goImg.rectTransform.DOScale(1.3f, 0.3f).SetEase(Ease.OutBack));
        seq.Append(_goImg.DOFade(0f, 0.1f))
            .OnComplete(() =>
            {
                _goImg.gameObject.SetActive(false);
                GameManager.Instance.Pouse(false);
            });
    }

    public void DisplayResult()
    {
        _resultPanel.SetActive(true);
        int score = 0;
        DOTween.To(() => score,
            x =>
            {
                score = x;
                _resultScore.text = $"{score:D9}";
            },
            CurrentScore,
            _resultDuration)
            .SetLink(gameObject);
    }
}
