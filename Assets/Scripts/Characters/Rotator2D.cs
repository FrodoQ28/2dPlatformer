using UnityEngine;

public class Rotator2D : MonoBehaviour
{
    private Quaternion _turnLeft = Quaternion.Euler(0f, 180f, 0f);
    private Quaternion _turnRight = Quaternion.Euler(0f, 0f, 0f);

    private bool _isRight = true;

    public void Turn()
    {
        if (_isRight == false)
        {
            transform.rotation = _turnLeft;
        }
        else
        {
            transform.rotation = _turnRight;
        }
    }

    public Vector2 DefineTurn(Vector2 direction)
    {
        if (direction.x > Vector2.zero.x)
        {
            _isRight = true;
        }
        else if (direction.x < Vector2.zero.x)
        {
            _isRight = false;
            direction.x = -direction.x;
        }

        return direction;
    }
}