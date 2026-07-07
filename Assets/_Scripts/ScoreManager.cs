using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int CurrentScore
    {
        get => _currentScore;
        set
        {
            _currentScore = value;
            Debug.Log($"スコア：{_currentScore}点");
            //UI周り呼びたい
        }
    }

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void AddScore(int delta)
    {
        CurrentScore = Mathf.Max(0, CurrentScore + delta);
    }
}
