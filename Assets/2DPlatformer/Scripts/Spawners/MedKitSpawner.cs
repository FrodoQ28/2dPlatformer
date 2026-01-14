using UnityEngine;

public class MedKitSpawner : LootSpawner<MedKit>
{

#if UNITY_EDITOR
    [ContextMenu("Refresh Point List")]
    private void RefreshPointList()
    {
        int pointCoinCount = _listSpawnPoints.transform.childCount;
        _pointsArray = new Transform[pointCoinCount];

        for (int i = 0; i < pointCoinCount; i++)
            _pointsArray[i] = _listSpawnPoints.transform.GetChild(i).transform;
    }
#endif
}
