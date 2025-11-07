using System;
using UnityEngine;

public class Coin : MonoBehaviour,ILootable
{
    public event Action<ILootable> LootTaked;

    public void Taked() =>
            LootTaked?.Invoke(this);
}