using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class UIFlyCoins : UIElement
{
    public static UIFlyCoins Instance;

    private class FlyCoin
    {
        public FlyCoin(GameObject coin, float speed, Vector3 customPos)
        {
            _coin = coin;
            _speed = speed;
            _customPos = customPos;
            _goToCustomPos = true;
        }

        public GameObject _coin;
        public float _speed;
        public Vector3 _customPos;
        public bool _goToCustomPos;
    }

    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private Transform _needPos;
    [SerializeField] private Vector2 _coinsSpeeds;
    [SerializeField] private float _acc;

    private List<FlyCoin> _listCoins;

    private bool _isCoinsFly;
    private int _cnt;

    public bool IsCoinsFly() { return _isCoinsFly; }

    private void Awake()
    {
        Instance = this;
        _listCoins = new List<FlyCoin>();
    }

    public void SpawnCoins(Vector3 spawnPos, int cnt, float time)
    {
        _isCoinsFly = true;
        _cnt = cnt;

        StartCoroutine(DelaySpawnCoins(spawnPos, time));
    }

    private IEnumerator DelaySpawnCoins(Vector3 spawnPos, float time)
    {
        yield return new WaitForSeconds(time);

        if(_cnt != 1)
        {
            _cnt = Random.Range(10, 15);

        }

        for (int i = 0; i < _cnt; ++i)
        {
            GameObject coin = Instantiate(_coinPrefab, _canvas.transform);
            coin.transform.position = spawnPos;

            float randX = spawnPos.x + Random.Range(-100, 100);
            float randY = spawnPos.y + Random.Range(-100, 100);

            Vector3 customPos = new Vector3(randX, randY, 0);

            float speed = Random.Range(_coinsSpeeds.x, _coinsSpeeds.y);

            _listCoins.Add(new FlyCoin(coin, speed, customPos));
        }
    }

    private void FixedUpdate()
    {
        if (_isCoinsFly)
        {
            for (int i = 0; i < _listCoins.Count; ++i)
            {
                _listCoins[i]._speed += _acc * Time.fixedDeltaTime;

                float speed = _listCoins[i]._speed;
                Vector3 needPos = _needPos.position;
                if (_listCoins[i]._goToCustomPos)
                {
                    needPos = _listCoins[i]._customPos;
                    speed *= 0.35f;
                }

                Vector3 dir = needPos - _listCoins[i]._coin.transform.position;
                float dist = dir.magnitude;

                float speedInDt = speed * Time.fixedDeltaTime;
                if (dist < speedInDt)
                {
                    if (_listCoins[i]._goToCustomPos)
                    {
                        _listCoins[i]._goToCustomPos = false;
                    }
                    else
                    {
                        GameManager.Instance.AddCoins(1);
                        UICoins.Instance.ShowCoins(1);
                        Destroy(_listCoins[i]._coin);
                        _listCoins.RemoveAt(i);
                    }
                }
                else
                {
                    _listCoins[i]._coin.transform.position += dir.normalized * speedInDt;
                }
            }
        }
    }
}
