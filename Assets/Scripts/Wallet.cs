using UnityEngine;
using System;
public class Wallet : MonoBehaviour
{
    [SerializeField] private Player _player;

    public event Action MoneyChanged;

    public int Money { get; private set; }

    private void Awake() =>
        Money = 0;

    private void OnEnable() =>
        _player.MoneyTaked += AddMoney;

    private void OnDisable() =>
        _player.MoneyTaked -= AddMoney;

    private void AddMoney()
    {
        Money++;
        MoneyChanged?.Invoke();
    }
}