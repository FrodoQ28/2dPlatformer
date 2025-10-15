using System;
using UnityEngine;
using UnityEngine.Pool;
using System.Collections;
using System.Collections.Generic;


public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private GameObject _listSpawnPoints;
    [SerializeField] private Transform[] _pointsArray;

    private Quaternion _defaultRotate;
    private ObjectPool<Coin> _pool;
    private List<Coin> _coinSubscriptionsList;

    private void Awake()
    {
        if (_pointsArray == null)
            throw new NullReferenceException("Список точек спавна монет пуст");

        _defaultRotate = new Quaternion(0f, 0f, 0f, 0f);

        _pool = new ObjectPool<Coin>(
            createFunc: () => default,
            actionOnGet: (coin) => coin.gameObject.SetActive(true),
            actionOnRelease: (coin) => coin.gameObject.SetActive(false),
            actionOnDestroy: (coin) => Destroy(coin),
            collectionCheck: true,
            defaultCapacity: _pointsArray.Length,
            maxSize: _pointsArray.Length);

        _coinSubscriptionsList = new List<Coin>();

        for (int i = 0; i < _pointsArray.Length; i++)
        {
            Coin coin = Instantiate(_coinPrefab, _pointsArray[i].position, _defaultRotate);

            coin.MoneyTaked += CoinDisable;
            _coinSubscriptionsList.Add(coin);
        }
    }

    private void OnDisable()
    {
        UnsubscribeAll();
    }

    public void CoinDisable(Coin coin)
    {
        _pool.Release(coin);

        StartCoroutine(WaitingToRespawn());
    }

    private void UnsubscribeAll()
    {
        foreach (Coin coin in _coinSubscriptionsList)
            coin.MoneyTaked -= CoinDisable;
    }

    private IEnumerator WaitingToRespawn(int delay = 10)
    {
        yield return new WaitForSeconds(delay);

        _pool.Get();
    }

#if UNITY_EDITOR
    [ContextMenu("Refresh Point List")]
    private void RefreshPointList()
    {
        int pointCount = _listSpawnPoints.transform.childCount;
        _pointsArray = new Transform[pointCount];

        for (int i = 0;i < pointCount;i++)
        {
            _pointsArray[i] = _listSpawnPoints.transform.GetChild(i).transform;
        }
    }
#endif
}