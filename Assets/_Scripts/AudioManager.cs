using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public enum SEType
{
    PickUp, Damage, BeamOn, BeamOff, Button, Finish
}
public enum BGMType
{
    None, Title, InGame,Result
}

[Serializable]
public class SEData
{
    public SEType Type;
    public AudioClip Clip;
}

[Serializable]
public class BGMData
{
    public BGMType Type;
    public AudioClip Clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public const string MasterVolume = "MasterVolume";
    public const string BGMVolume = "BGMVolume";
    public const string SEVolume = "SEVolume";

    [Header("参照")]
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _seSource;

    [Header("データ")]
    [SerializeField] private SEData[] _seDatas;
    [SerializeField] private BGMData[] _bgmDatas;

    [Header("パラメータ")]
    [SerializeField] private float _bgmFadeDuration = 0.5f;

    private Dictionary<SEType, AudioClip> _seDic;
    private Dictionary<BGMType, AudioClip> _bgmDic;
    private BGMType _currentBGM = BGMType.None;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _seDic = new();
        foreach (var d in _seDatas) _seDic.Add(d.Type, d.Clip);
        _bgmDic = new();
        foreach (var d in _bgmDatas) _bgmDic.Add(d.Type, d.Clip);
    }

    public void PlaySE(SEType type)
    {
        if (_seDic.TryGetValue(type, out var clip))
            _seSource.PlayOneShot(clip);
        else
            Debug.LogWarning($"SE未登録: {type}");
    }

    public void PlayBGM(BGMType type)
    {
        if (type == _currentBGM) return; 
        _currentBGM = type;

        if (!_bgmDic.TryGetValue(type, out var clip) || clip == null)
        {
            StopBGM();
            return;
        }

        _bgmSource.DOKill();
        _bgmSource.DOFade(0f, _bgmFadeDuration)
            .OnComplete(() =>
            {
                _bgmSource.clip = clip;
                _bgmSource.Play();
                _bgmSource.DOFade(1f, _bgmFadeDuration);
            })
            .SetLink(gameObject);
    }

    public void StopBGM()
    {
        _currentBGM = BGMType.None;
        _bgmSource.DOKill();
        _bgmSource.DOFade(0f, _bgmFadeDuration)
            .OnComplete(() => _bgmSource.Stop())
            .SetLink(gameObject);
    }

    /// <summary>
    /// 音量設定
    /// </summary>
    public void SetVolume(string exposedName, float volume01)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume01, 0.0001f, 1f)) * 20f;
        _mixer.SetFloat(exposedName, dB);
    }

    public BGMType GetBGM(SceneName scene)
    {
        return scene switch
        {
            SceneName.Title => BGMType.Title,
            SceneName.InGame => BGMType.InGame,
            SceneName.Result => BGMType.Result,
            _ => BGMType.None,
        };
    }
}