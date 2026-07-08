using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("ŽQÆ")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private TextMeshProUGUI _resultScore;

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
                _duration);
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
    }

    public void AddScore(int delta)
    {
        CurrentScore = Mathf.Max(0, CurrentScore + delta);
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
            _resultDuration);
    }
}
