using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Beam : MonoBehaviour
{
    public bool IsUnfoldong { get; private set; } = false;

    public HashSet<ISuckable> Suckables { get; private set; } = new();

    [Header("参照")]
    [SerializeField] private Light _light;
    [SerializeField] private GameObject _collider;
    [SerializeField] private Transform _center;

    [Header("パラメータ")]
    [SerializeField] private int _sides = 28;
    [SerializeField] private float _topRadius = 0.4f;
    [SerializeField] private float _maxLength = 8f;
    [SerializeField] private float _maxBottomRadius = 3f;
    [SerializeField] private float _maxIntensity = 120f;
    [SerializeField] private float _power = 5f;

    private Mesh _mesh;
    private Tween _tween;

    void Awake()
    {
        _mesh = new Mesh();
        _mesh.MarkDynamic();
        GetComponent<MeshFilter>().mesh = _mesh;
        _collider.SetActive(false);
    }
    private void FixedUpdate()
    {
        foreach (var obj in Suckables)
        {
            obj.Suction(_center.position,_power);
        }
    }
    /// <summary>
    /// 光線の表示処理
    /// </summary>
    /// <param name="duration">展開時間</param>
    /// <param name="isExpand">展開する</param>
    public void BeamAnimation(float duration, bool isExpand)
    {
        if (_tween != null)
        {
            _tween.Kill();
        }

        if(!isExpand)
        {
            _collider.SetActive(false);
            RemoveAllSuckable();
        }

        IsUnfoldong = true;
        float start = isExpand ? 0f : 1f;
        float end = isExpand ? 1f : 0f;
        _tween = DOTween.To(() => start,
            x =>
            {
                start = x;
                _light.intensity = Mathf.Lerp(0f, _maxIntensity, start);
                RebuilBeam(start);
            },
            end,
            duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                IsUnfoldong = false;
                if (isExpand)
                    _collider.SetActive(true);
            });
    }

    private void RebuilBeam(float deploy)
    {
        float len = _maxLength * deploy;
        float botR = Mathf.Lerp(_topRadius, _maxBottomRadius, deploy);

        var verts = new Vector3[_sides * 2];
        var uvs = new Vector2[_sides * 2];

        for (int i = 0; i < _sides; i++)
        {
            float a = i / (float)_sides * Mathf.PI * 2f;
            var dir = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));
            verts[i] = dir * _topRadius;
            verts[i + _sides] = dir * botR - Vector3.up * len;
            uvs[i] = new Vector2(i / (float)_sides, 0);
            uvs[i + _sides] = new Vector2(i / (float)_sides, 1);
        }

        var tris = new int[_sides * 6];
        for (int i = 0; i < _sides; i++)
        {
            int next = (i + 1) % _sides;
            int t = i * 6;
            tris[t] = i; tris[t + 1] = next; tris[t + 2] = i + _sides;
            tris[t + 3] = next; tris[t + 4] = next + _sides; tris[t + 5] = i + _sides;
        }

        _mesh.Clear();
        _mesh.vertices = verts;
        _mesh.uv = uvs;
        _mesh.triangles = tris;
        _mesh.RecalculateNormals();
    }

    /// <summary>
    /// 吸引対象オブジェクトの登録
    /// </summary>
    /// <param name="obj">登録したいオブジェクト</param>
    public void RegisterSuckableObject(ISuckable obj)
    {
        Suckables.Add(obj);
        Debug.Log("登録！");
    }

    /// <summary>
    /// 吸引対象オブジェクトの登録解除
    /// </summary>
    /// <param name="obj">解除したいオブジェクト</param>
    public void RemoveSuckableObject(ISuckable obj)
    {
        Suckables.Remove(obj);
        Debug.Log("解除！");
    }

    public void RemoveAllSuckable()
    {
        Suckables.RemoveWhere(x => x.IsSuction);
    }
}
