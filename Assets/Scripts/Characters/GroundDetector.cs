using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public bool IsGrounded {  get; private set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerList.Ground && IsGrounded == false)
            IsGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerList.Ground && IsGrounded)
            IsGrounded = false;
    }
}