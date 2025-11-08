using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class LootSpawner<T> : MonoBehaviour where T : MonoBehaviour, ILootable
{
    [SerializeField] private T _prefab;
    [SerializeField] protected GameObject _listSpawnPoints;
    [SerializeField] protected Transform[] _pointsArray;

    private Quaternion _defaultRotate;
    private ObjectPool<T> _pool;
    private List<T> _subscriptionsList;

    private void Awake()
    {
        if (_pointsArray == null)
            throw new NullReferenceException("Список точек спавна монет пуст");

        _defaultRotate = new Quaternion(0f, 0f, 0f, 0f);

        _pool = new ObjectPool<T>(
            createFunc: () => default,
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: _pointsArray.Length,
            maxSize: _pointsArray.Length);

        _subscriptionsList = new List<T>();

        for (int i = 0; i < _pointsArray.Length; i++)
        {
            T obj = Instantiate(_prefab, _pointsArray[i].position, _defaultRotate);

            obj.LootTaked += Disable;
            _subscriptionsList.Add(obj);
        }
    }

    private void OnDisable()
    {
        UnsubscribeAll();
    }

    private void Disable(ILootable loot)
    {
        if (loot is T obj)
        {
            _pool.Release(obj);

            StartCoroutine(WaitingToRespawn());
        }
    }

    private void UnsubscribeAll()
    {
        foreach (ILootable loot in _subscriptionsList)
            loot.LootTaked -= Disable;
    }

    private IEnumerator WaitingToRespawn(int delay = 10)
    {
        yield return new WaitForSeconds(delay);

        _pool.Get();
    }


}