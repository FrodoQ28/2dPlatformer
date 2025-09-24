using UnityEngine;

public class Rotator : MonoBehaviour
{
    private Vector2 _turnLeft = new Vector2(0f, 180f);
    private Vector2 _turnRight = new Vector2(0f, 0f);

    public void Turn(bool isRight)
    {
        if (isRight == false)
        {
            transform.eulerAngles = _turnLeft;
        }
        else
        {
            transform.eulerAngles = _turnRight;
        }
    }
}