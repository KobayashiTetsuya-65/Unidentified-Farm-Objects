using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public EnergyGauge EnergyGauge { get; private set; }

    [SerializeField] private float _decreaseTime = 1.0f;
    private SceneName _currentScene;

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

        //å„Ç≈è¡Ç∑
        _currentScene = SceneName.InGame;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentScene != SceneName.InGame) return;

        ChangeEnergy(_decreaseTime);
    }

    public void RegisterGauge(EnergyGauge gauge)
    {
        EnergyGauge = gauge;
    }

    public void ChangeEnergy(float delta)
    {
        EnergyGauge.ChangeGauge(delta);
    }
}
public enum SceneName
{
    Title,
    InGame
}
