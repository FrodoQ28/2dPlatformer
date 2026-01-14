using System;
using UnityEngine;

public class MedKit : MonoBehaviour, ILootable
{
    public event Action<ILootable> LootTaked;

    public int HealValue => 10;

    public void Taked() =>
        LootTaked?.Invoke(this);
}