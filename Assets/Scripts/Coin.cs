using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public event Action<Coin> MoneyTaked;

    public void TakeMoney() =>
            MoneyTaked?.Invoke(this);
}