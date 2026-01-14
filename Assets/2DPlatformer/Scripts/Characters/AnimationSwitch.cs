using UnityEngine;
[RequireComponent(typeof(Animator))]
public class AnimationSwitch : MonoBehaviour
{

    private const string IsMoving = "IsMoving";
    private const string IsJumping = "IsJumping";
    private const string Attack = "Attack";

    private Animator _animator;

    private readonly int _isMovingHash = Animator.StringToHash(nameof(IsMoving));
    private readonly int _isJumpingHash = Animator.StringToHash(nameof(IsJumping));
    private readonly int _attackHash = Animator.StringToHash(nameof(Attack));

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void TurnOnMove() =>
        _animator.SetBool(_isMovingHash, true);

    public void TurnOffMove() =>
        _animator.SetBool(_isMovingHash, false);

    public void TurnOnJump() =>
        _animator.SetBool(_isJumpingHash, true);

    public void TurnOffJump() =>
        _animator.SetBool(_isJumpingHash, false);

    public void TurnOnAttack() =>
        _animator.SetTrigger(_attackHash);
}