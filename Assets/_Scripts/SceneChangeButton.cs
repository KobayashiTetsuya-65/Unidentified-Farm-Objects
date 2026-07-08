using UnityEngine;
using UnityEngine.UI;

public class SceneChangeButton : MonoBehaviour
{
    [SerializeField] private TitleUFO _ufo;
    [SerializeField] private Button _button;
    [SerializeField] private SceneName _sceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button.onClick.AddListener(() =>
        {
            _ufo.StartAnimation(() => GameManager.Instance.SceneChange(_sceneName));
        });
    }
}
