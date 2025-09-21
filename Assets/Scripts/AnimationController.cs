using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator _animator;
    private Mover _mover;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _mover = GetComponent<Mover>();
    }

    private void OnEnable()
    {
        _mover.MoveEnabled += OnMove;
        _mover.MoveDisabled += OffMove;
        _mover.JumpEnabled += OnJump;
    }

    private void OnDisable()
    {
        _mover.MoveEnabled -= OnMove;
        _mover.MoveDisabled -= OffMove;
        _mover.JumpEnabled -= OnJump;
    }

    private void OnMove() =>
        _animator.SetBool("IsMoving", true);

    private void OffMove() =>
     _animator.SetBool("IsMoving", false);

    private void OnJump() =>
        StartCoroutine(Jumping(_mover.IsGrounded));

    private IEnumerator Jumping(bool isJumping)
    {
        WaitUntil wait = new WaitUntil(() => isJumping);

        _animator.SetBool("IsJumping", true);

        yield return wait;

        _animator.SetBool("IsJumping", false);
    }
}