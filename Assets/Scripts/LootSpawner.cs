using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class LootSpawner : MonoBehaviour
{
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private MedKit _medKitPrefab;
    [SerializeField] private GameObject _listCoinSpawnPoints;
    [SerializeField] private GameObject _listMedKitSpawnPoints;
    [SerializeField] private Transform[] _pointsCoinArray;
    [SerializeField] private Transform[] _pointsMedKitArray;

    private Quaternion _defaultRotate;
    private int _maxCoin = 2;
    private int _maxMedKit = 1;
    private ObjectPool<Coin> _coinPool;
    private ObjectPool<MedKit> _medKitPool;
    private List<ILootable> _subscriptionsList;

    private void Awake()
    {
        if (_pointsCoinArray == null)
            throw new NullReferenceException("Список точек спавна монет пуст");

        _defaultRotate = new Quaternion(0f, 0f, 0f, 0f);

        _coinPool = new ObjectPool<Coin>(
            createFunc: () => default,
            actionOnGet: (coin) => coin.gameObject.SetActive(true),
            actionOnRelease: (coin) => coin.gameObject.SetActive(false),
            actionOnDestroy: (coin) => Destroy(coin),
            collectionCheck: true,
            defaultCapacity: _maxCoin,
            maxSize: _maxCoin);

        _medKitPool = new ObjectPool<MedKit>(
            createFunc: () => default,
            actionOnGet: (medKit) => medKit.gameObject.SetActive(true),
            actionOnRelease: (medKit) => medKit.gameObject.SetActive(false),
            actionOnDestroy: (medKit) => Destroy(medKit),
            collectionCheck: true,
            defaultCapacity: _maxMedKit,
            maxSize: _maxMedKit);

        _subscriptionsList = new List<ILootable>();

        for (int i = 0; i < _maxCoin; i++)
        {
            Coin coin = Instantiate(_coinPrefab, _pointsCoinArray[i].position, _defaultRotate);

            coin.LootTaked += Disable;
            _subscriptionsList.Add(coin);
        }

        for (int i = 0; i < _maxMedKit; i++)
        {
            MedKit medKit = Instantiate(_medKitPrefab, _pointsMedKitArray[i].position, _defaultRotate);

            medKit.LootTaked += Disable;
            _subscriptionsList.Add(medKit);
        }
    }

    private void OnDisable()
    {
        UnsubscribeAll();
    }

        private void Disable(ILootable loot)
    {
        if (loot is Coin coin)
        {
            _coinPool.Release(coin);

            StartCoroutine(WaitingToRespawn(coin));
        }
        else if (loot is MedKit medKit)
        {
            _medKitPool.Release(medKit);

            StartCoroutine(WaitingToRespawn(medKit));
        }
    }


    private void UnsubscribeAll()
    {
        foreach (ILootable loot in _subscriptionsList)
            loot.LootTaked -= Disable;
    }

    private IEnumerator WaitingToRespawn(Coin coin, int delay = 10)
    {
        yield return new WaitForSeconds(delay);

        _coinPool.Get();
    }

    private IEnumerator WaitingToRespawn(MedKit medKit, int delay = 10)
    {
        yield return new WaitForSeconds(delay);

        _medKitPool.Get();
    }

#if UNITY_EDITOR
    [ContextMenu("Refresh Point List")]
    private void RefreshPointList()
    {
        int pointCoinCount = _listCoinSpawnPoints.transform.childCount;
        int pointMedKitCount = _listMedKitSpawnPoints.transform.childCount;
        _pointsCoinArray = new Transform[pointCoinCount];
        _pointsMedKitArray = new Transform[pointMedKitCount];

        for (int i = 0;i < pointCoinCount;i++)
        {
            _pointsCoinArray[i] = _listCoinSpawnPoints.transform.GetChild(i).transform;
        }

        for (int i = 0; i < pointMedKitCount; i++)
        {
            _pointsMedKitArray[i] = _listMedKitSpawnPoints.transform.GetChild(i).transform;
        }
    }
#endif
}