using TMPro;
using UnityEngine;

public class WalletViewer : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private TMP_Text _text;

    private void Awake() =>
        _text.text = _wallet.Money.ToString();

    private void OnEnable() =>
        _wallet.MoneyChanged += ShowMoney;

    private void OnDisable() =>
        _wallet.MoneyChanged -= ShowMoney;

    private void ShowMoney() =>
        _text.text = _wallet.Money.ToString();
}