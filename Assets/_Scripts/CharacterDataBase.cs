using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DataBase")]
public class CharacterDataBase : ScriptableObject
{
    [SerializeField] private List<CharacterData> _characterDatas;
    private Dictionary<CharacterType, CharacterBase> _characterDic;

    public CharacterBase GetCharacterPrefab(CharacterType type)
    {
        if (_characterDic == null)
        {
            _characterDic = new();
            foreach (CharacterData data in _characterDatas)
            {
                _characterDic.Add(data.Type, data.Prefab);
            }
        }

        return _characterDic.TryGetValue(type, out var prefab) ? prefab : null;
    }
}
