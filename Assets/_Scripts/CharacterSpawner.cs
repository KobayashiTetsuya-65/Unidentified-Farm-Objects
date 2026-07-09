using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public static CharacterSpawner Instance;
    public CharacterDataBase CharacterDataBase => _characterDataBase;
    [SerializeField] private CharacterDataBase _characterDataBase;
    [SerializeField] private CharacterType[] _poolCharacters;
    [SerializeField] private int _poolSize = 20;
    [SerializeField] private SpawnerArea[] _spawnAreas;

    private Dictionary<CharacterType, Queue<CharacterBase>> _spawnerDic = new();

    private void Awake()
    {
        Instance = this;

        foreach (var type in _poolCharacters)
        {
            CharacterBase character = _characterDataBase.GetCharacterPrefab(type);
            Queue<CharacterBase> pool = new();
            for (int i = 0; i < _poolSize; i++)
            {
                CharacterBase newCharacter = Instantiate(character);
                newCharacter.gameObject.SetActive(false);
                pool.Enqueue(newCharacter);
            }
            _spawnerDic.Add(type, pool);
        }
    }

    public CharacterBase Spawn(CharacterType type,Vector3 pos)
    {
        if(_spawnerDic.TryGetValue(type,out var pool))
        {
            if(pool.TryDequeue(out var character))
            {
                character.transform.position = pos;
                character.gameObject.SetActive(true);
                character.ResetState();
                return character;
            }
            else
            {
                CharacterBase newCharactor = Instantiate(_characterDataBase.GetCharacterPrefab(type));
                newCharactor.transform.position = pos;
                newCharactor.gameObject.SetActive(true);
                newCharactor.ResetState();
                return newCharactor;
            }
        }
        else
        {
            CharacterBase newChara = Instantiate(_characterDataBase.GetCharacterPrefab(type));
            newChara.transform.position = pos;
            newChara.gameObject.SetActive(true);
            newChara.ResetState();
            return newChara;
        }
    }

    public void Despawn(CharacterType type, CharacterBase chara)
    {
        if (!chara.gameObject.activeSelf) return;

        chara.gameObject.SetActive(false);
        if (_spawnerDic.TryGetValue(type, out var pool))
            pool.Enqueue(chara);
    }
}
