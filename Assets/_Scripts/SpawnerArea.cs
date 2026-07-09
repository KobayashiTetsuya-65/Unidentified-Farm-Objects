using UnityEngine;

public class SpawnerArea : MonoBehaviour
{
    public int[] SpawnIndexs => _spawnIndexs;
    [SerializeField] private BoxCollider _areaCol;
    [SerializeField] private SpawnTable[] _spawnData;
    [SerializeField] private float _spawnInterval = 3f;

    private GameManager _gameManager;
    private CharacterSpawner _spawner;
    private int[] _spawnIndexs;
    private float _timer = 0f;
    private void Start()
    {
        _spawner = CharacterSpawner.Instance;
        _gameManager = GameManager.Instance;
        _spawnIndexs = new int[_spawnData.Length];
        for(int i = 0; i < _spawnIndexs.Length; i++)
        {
            _spawnIndexs[i] = 0;
        }
    }

    private void Update()
    {
        if (_gameManager.IsStop) return;

        _timer -= Time.deltaTime;
        if (_timer > 0f) return;

        _timer = _spawnInterval;
        SpawnCharacter();
    }

    public void SpawnCharacter()
    {
        for(int i = 0; i < _spawnData.Length; i++)
        {
            if (_spawnIndexs[i] >= _spawnData[i].Max) continue;

            int index = i;
            CharacterBase cb = _spawner.Spawn(_spawnData[i].CharacterType, SpawnPos());
            if (cb == null) continue;
            _spawnIndexs[index]++;
            cb.Init(chara =>
            {
                _spawnIndexs[index]--;
                _spawner.Despawn(_spawnData[index].CharacterType, chara);
            });
        }
    }

    private Vector3 SpawnPos()
    {
        Vector3 c = _areaCol.transform.TransformPoint(_areaCol.center);
        Vector3 half = Vector3.Scale(_areaCol.size, _areaCol.transform.lossyScale) * 0.5f;
        float x = Random.Range(c.x - half.x, c.x + half.x);
        float z = Random.Range(c.z - half.z, c.z + half.z);

        return new Vector3(x, 0f, z);
    }
}
