using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public static SpawnPoint Instance;
    public GameObject _spawnTrigger;
    [SerializeField] List<Transform> _spawnPos;
    public List<GameObject> _enemies;
    [SerializeField] GameObject _enemyHolder;
    [SerializeField] GameObject _enemyPref;
    [SerializeField] GameObject _coinPref;
    [SerializeField] GameObject _sectionBlock;
    [SerializeField] Transform _coinSpawnPose;
    [SerializeField] Transform _roadBlockPos;
    [SerializeField] GameObject _baseEnemy;
    [SerializeField] GameObject _fastEnemy;
    [SerializeField] GameObject _rangeEnemy;
    [SerializeField] GameObject _giantEnemy;

    bool _isEnemyFilled = false;
    bool _isCoinSpawned = false;
    bool _isMarkerOn = false;
    public bool _isWaveSpawned = false;

    private void Awake()
    {
        Instance = this;
        if (!_isWaveSpawned)
        {
            FillSpawnPoints();
        }
    }

    private void Start()
    {
        if (Level.Instance.GetComponent<LevelType>()._levelType == LevelType.LevelT._base)
        {
            SpawnCoins();
            _isCoinSpawned = true;
        }
    }
    private void Update()
    {
        if (IsWaveCleared() && !_isCoinSpawned)
        {
            SpawnCoins();
            _isCoinSpawned = true;

        }
        if (_isWaveSpawned)
        {
            if(_sectionBlock != null)
            {
                _sectionBlock.transform.position = _roadBlockPos.position;
            }
        }
        if(_enemies.Count < 4  && !_isMarkerOn)
        {
            _isMarkerOn = true;
            foreach(GameObject enemy in _enemies)
            {
                enemy.GetComponent<OffscreenMarker>().enabled = true;
            }
        }
    }

    public void SpawnEnemies()
    {
        foreach (Transform en in _spawnPos)
        {
            GameObject enemy;
            EnemyType et = en.GetComponent<EnemyType>();
            if( et._type == EnemyType.Type.Base)
            {
                enemy = Instantiate(_baseEnemy);
            }
            else if(et._type == EnemyType.Type.Fast)
            {
                enemy = Instantiate(_fastEnemy);
            } 
            else if(et._type == EnemyType.Type.Range)
            {
                enemy = Instantiate(_rangeEnemy);
            }
            else
            {
                enemy = Instantiate(_giantEnemy);
            }
            enemy.transform.parent = gameObject.transform;
            enemy.transform.position = en.position;
            enemy.transform.rotation = Quaternion.Euler(0, 180, 0);
            enemy.GetComponent<OffscreenMarker>().enabled = false;
            _enemies.Add(enemy);
            
        }
        _spawnTrigger.GetComponent<SpawnSectionByTRigger>()._isSpawned = true;
        _isEnemyFilled = true;
        _isWaveSpawned = true;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    foreach(GameObject enemy in _enemies)
    //    {
    //        Gizmos.DrawLine(enemy.transform.position, Player.Instance.transform.position);
    //    }
    //}
    public bool IsWaveCleared()
    {
        bool cleared = false;
        if (_isWaveSpawned)
        {
            cleared = _enemies.Count == 0;
        }
        return cleared;
    }
   public bool PrevWaveStatus()
    {
        return IsWaveCleared();
    }
   public void  FillSpawnPoints()
    {
        EnemyType[] positions = _enemyHolder.GetComponentsInChildren<EnemyType>();
        foreach(EnemyType pos in positions)
        {
            
            _spawnPos.Add(pos.gameObject.transform);
        }

    }

    void SpawnCoins() 
    {
        //Transform[] coinPos = _coinSpawnPose.GetComponentsInChildren<Transform>();
        foreach(Transform c in _coinSpawnPose)
        {
            GameObject coin = Instantiate(_coinPref);
            coin.transform.position = c.transform.position;
            coin.transform.rotation = c.transform.rotation;
            coin.transform.parent = gameObject.transform;
        }
    }
    public void EnemyDie(GameObject enemy)
    {
        _enemies.Remove(enemy);
    }

    public void PreviousWaveDestroy()
    {
        foreach(GameObject enemy in _enemies)
        {
            if(enemy != null)
            {
                Destroy(enemy);
            }
        }
    }
}
