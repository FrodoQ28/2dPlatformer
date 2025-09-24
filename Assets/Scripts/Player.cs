using UnityEngine;
using System;
public class Player : MonoBehaviour
{
    public event Action MoneyTaked;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Coin>(out Coin coin))
        {
            MoneyTaked?.Invoke();
            Destroy(coin.gameObject);
        }
    }
}