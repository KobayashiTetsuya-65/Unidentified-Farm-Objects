using System;
using UnityEngine;

[Serializable]
public class CharacterData
{
    public CharacterType Type => _type;
    public CharacterBase Prefab => _prefab;
    [SerializeField] private CharacterType _type;
    [SerializeField] private CharacterBase _prefab;
}
