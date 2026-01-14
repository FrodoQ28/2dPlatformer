using UnityEngine;
using System;
public class Wallet : MonoBehaviour
{
    public event Action MoneyChanged;

    public int Money { get; private set; }

    private void Awake() =>
        Money = 0;

    public void AddMoney()
    {
        Money++;
        MoneyChanged?.Invoke();
    }
}