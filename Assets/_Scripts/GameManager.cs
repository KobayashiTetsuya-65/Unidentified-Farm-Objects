using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsStop{ get; private set; } = false;
    public EnergyGauge EnergyGauge { get; private set; }
    public SceneName CurrentScene
    {
        get => _currentScene;
        private set
        {
            _currentScene = value;

            if(_currentScene == SceneName.Result)
            {
                ScoreManager scoreManager = ScoreManager.Instance;
                scoreManager.ResultScore();

                return;
            }
            else if(_currentScene == SceneName.InGame)
            {
                IsStop = true;

                //開始演出
                ScoreManager.Instance.StartAnimation();
                return;
            }

            //初期化
            IsStop = false;
        }
    }

    [SerializeField] private Canvas _canvas;
    [SerializeField] private Image _fadePanel;
    [SerializeField] private SceneName _startScene;
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private float _decreaseTime = 1.0f;
    private SceneName _currentScene;
    private bool _isFade = false;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
        _canvas.sortingOrder = 9999;
        CurrentScene = _startScene;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentScene != SceneName.InGame) return;

        if (IsStop) return;

        ChangeEnergy(-_decreaseTime * Time.deltaTime);
    }

    public void RegisterGauge(EnergyGauge gauge)
    {
        EnergyGauge = gauge;
    }

    public void ChangeEnergy(float delta,bool useAnimation = false)
    {
        EnergyGauge.ChangeGauge(delta,useAnimation);
    }

    public void Pouse(bool isPouse)
    {
        IsStop = isPouse;
    }

    public void SceneChange(SceneName name)
    {
        if (_isFade) return;
        _isFade = true;
        FadePanel(false, async () =>
        {
            await SceneManager.LoadSceneAsync($"{name}");
            CurrentScene = name;
            _isFade = false;
            FadePanel(true);
        });
    }

    public void ChangeCurrentScene(SceneName scene)
    {
        CurrentScene = scene;
    }

    /// <summary>
    /// フェードする
    /// </summary>
    /// <param name="isFadeIN">明るくなる方？</param>
    /// <param name="onComplate">追加処理</param>
    /// <param name="duration">演出時間</param>
    /// <param name="ease">関数</param>
    public void FadePanel(bool isFadeIN, System.Action onComplate = null, float duration = 0f, Ease ease = Ease.Unset)
    {
        if (duration == 0f) duration = _fadeDuration;
        float to = isFadeIN ? 0f : 1f;
        float start = isFadeIN ? 1f : 0f;
        _fadePanel.color = new Color(0f, 0f, 0f, start);
        _fadePanel.raycastTarget = true;
        _fadePanel.DOFade(to, duration)
            .SetEase(ease)
            .OnComplete(() =>
            {
                _fadePanel.raycastTarget = false;
                onComplate?.Invoke();
            }).SetAutoKill(true);
    }
}
public enum SceneName
{
    Title,
    InGame,
    Result
}
