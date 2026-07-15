using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class QuitButton : MonoBehaviour
{
    private Button _button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySE(SEType.Button);
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
        });
    }
}
