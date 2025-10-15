using UnityEngine;

public class Rotator2D : MonoBehaviour
{
    private Quaternion _turnLeft = Quaternion.Euler(0f, 180f, 0f);
    private Quaternion _turnRight = Quaternion.Euler(0f, 0f, 0f);

    public void Turn(bool isRight)
    {
        if (isRight == false)
        {
            transform.rotation = _turnLeft;
        }
        else
        {
            transform.rotation = _turnRight;
        }
    }
}