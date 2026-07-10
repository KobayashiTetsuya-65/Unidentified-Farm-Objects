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
    [SerializeField] private Image _finishImg;
    [SerializeField] private Transform _ufo;
    [SerializeField] private Beam _beam;

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
    private int _growCount = 0;
    private AudioManager _audioManager;
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
        _finishImg.gameObject.SetActive(false);
    }
    private void Start()
    {
        _audioManager = AudioManager.Instance;
    }
    public void AddScore(int delta)
    {
        CurrentScore = Mathf.Max(0, CurrentScore + delta);
        _audioManager.PlaySE(delta >= 0? SEType.Up : SEType.Down);
    }

    public void StartAnimation()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.AppendCallback(()  => _readyImg.gameObject.SetActive(true));
        seq.Append(_readyImg.DOFade(1f, 0.2f));
        seq.Join(_readyImg.rectTransform.DOScale(1f, 0.2f));
        seq.Append(_readyImg.rectTransform.DOScale(1.5f, 2.0f));
        seq.Append(_readyImg.rectTransform.DOScale(0.1f, 0.1f));
        seq.AppendCallback(() =>
        {
            _readyImg.gameObject.SetActive(false);
            _goImg.gameObject.SetActive(true);
            _audioManager.PlaySE(SEType.Start);
        });
        seq.Append(_goImg.rectTransform.DOScale(1.3f, 0.3f).SetEase(Ease.OutBack));
        seq.Append(_goImg.DOFade(0f, 0.1f))
            .OnComplete(() =>
            {
                _goImg.gameObject.SetActive(false);
                GameManager.Instance.Pouse(false);
            });
    }

    public void FinishAnimation(System.Action onComplete = null)
    {
        _audioManager.PlaySE(SEType.Finish);
        _finishImg.gameObject.SetActive(true);
        _finishImg.color = new Color(1f, 1f, 1f, 0f);
        _finishImg.rectTransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        Sequence seq = DOTween.Sequence();
        seq.Append(_finishImg.DOFade(1f, 0.1f));
        seq.Join(_finishImg.rectTransform.DOScale(1f, 0.45f).SetEase(Ease.OutBack));
        seq.AppendInterval(1.0f);
        seq.Append(_finishImg.DOFade(0f, 0.2f))
            .OnComplete(() =>
            {
                _finishImg.gameObject.SetActive(false);
                onComplete?.Invoke();
            });
    }

    public void DisplayResult(System.Action onComplete = null)
    {
        _audioManager.PlayBGM(_audioManager.GetBGM(SceneName.Result));
        _resultPanel.SetActive(true);
        onComplete?.Invoke();
    }

    public void ResultScore()
    {
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

    public void Grow(int delta)
    {
        _growCount += delta >= 0 ? 1 : -1;
        float scale = Mathf.Min(1f + _growCount * 0.05f, 3f);
        _ufo.localScale = Vector3.one * scale;
        _beam.PowerUp(scale);
        _beam.transform.localScale = new Vector3(1.25f, 1.25f / scale, 1.25f);
    }
}
