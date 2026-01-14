using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Mover2D), typeof(Attacker))]
public class Pursuer : MonoBehaviour
{
    private Mover2D _mover;
    private Rotator2D _rotator;
    private AnimationSwitch _animation;
    private Attacker _attacker;
    private Health _target;

    private Vector2 _direction;
    private Coroutine _chaseCoroutine;

    private void Awake()
    {
        _mover = GetComponent<Mover2D>();
        _rotator = GetComponent<Rotator2D>();
        _animation = GetComponent<AnimationSwitch>();
        _attacker = GetComponent<Attacker>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Health player) && player == _target)
        {
            _animation.TurnOnAttack();
            _attacker.Attack(player);
        }
    }

    public void SetTarget(Health target) =>
        _target = target;

    public void Enable()
    {
        if (_chaseCoroutine == null)
            _chaseCoroutine = StartCoroutine(Chase());
    }

    public void Disable()
    {
        if (_chaseCoroutine != null)
        {
            StopCoroutine(_chaseCoroutine);

            _chaseCoroutine = default;
        }
    }

    private IEnumerator Chase()
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();


        while (enabled)
        {
            yield return wait;

            if (_target != null)
            {
                _direction = (_target.transform.position - transform.position).normalized;
                _rotator.Turn(_direction);
            }

            if (_direction != null)
            {
                _mover.Move(_direction);
                _animation.TurnOnMove();
            }
        }
    }
}