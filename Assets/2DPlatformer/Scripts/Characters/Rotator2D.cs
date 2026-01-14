using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Rotator2D : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public bool TurnedLeft => _spriteRenderer.flipX;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Turn(Vector2 direction)
    {
        if (direction.x > Vector2.zero.x)
        {
            _spriteRenderer.flipX = false;
        }
        else if (direction.x < Vector2.zero.x)
        {
            _spriteRenderer.flipX = true;
        }
    }
}