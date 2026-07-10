using UnityEngine;
using UnityEngine.UI;

public class GameStartButton : MonoBehaviour
{
    [SerializeField] private TitleUFO _ufo;
    [SerializeField] private Button _button;
    [SerializeField] private SceneName _sceneName;

    private bool _isPush = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySE(SEType.Button);
            if (_isPush) return;
            _isPush = true;
            _ufo.StartAnimation(() => GameManager.Instance.SceneChange(_sceneName));
        });
    }
}
