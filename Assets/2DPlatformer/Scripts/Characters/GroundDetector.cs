using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    private int _enterExitDifference = 0;

    public bool IsGrounded => _enterExitDifference > 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerList.Ground && IsGrounded == false)
            _enterExitDifference++;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerList.Ground && IsGrounded)
            _enterExitDifference--;
    }
}