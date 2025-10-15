using UnityEngine;
public class Mover2D : MonoBehaviour
{
    [SerializeField, Min(1f)] private float _speed;
    [SerializeField, Min(1f)] private float _jumpHeight;
    public void Move(Vector2 direction) =>
        transform.Translate(direction * _speed * Time.deltaTime);

    public void Jump(Rigidbody2D rigidbody2D) =>
        rigidbody2D.AddForce(Vector2.up * _jumpHeight, ForceMode2D.Impulse);

}