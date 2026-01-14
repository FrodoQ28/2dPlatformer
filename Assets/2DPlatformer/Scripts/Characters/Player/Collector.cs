using UnityEngine;

[RequireComponent (typeof(Health))]
public class Collector : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Coin>(out Coin coin))
        {
            coin.Taked();
            _wallet.AddMoney();
        }
        else if (collision.gameObject.TryGetComponent<MedKit>(out MedKit medKit))
        {
            medKit.Taked();
            _health.Heal(medKit.HealValue);
        }
    }
}