using System;
using UnityEngine;

[Serializable]
public class SpawnTable
{
    public int Max => _max;
    public CharacterType CharacterType => _characterType;
    [SerializeField] private int _max;
    [SerializeField] private CharacterType _characterType;
}
