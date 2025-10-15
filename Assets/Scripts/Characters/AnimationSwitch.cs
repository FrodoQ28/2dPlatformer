using UnityEngine;
[RequireComponent(typeof(Animator))]
public class AnimationSwitch : MonoBehaviour
{

    private const string IsMoving = "IsMoving";
    private const string IsJumping = "IsJumping";

    private Animator _animator;

    private readonly int _isMovingHash = Animator.StringToHash(nameof(IsMoving));
    private readonly int _isJumpingHash = Animator.StringToHash(nameof(IsJumping));

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnMove() =>
        _animator.SetBool(_isMovingHash, true);

    public void OffMove() =>
        _animator.SetBool(_isMovingHash, false);

    public void OnJump() =>
        _animator.SetBool(_isJumpingHash, true);

    public void OffJump() =>
        _animator.SetBool(_isJumpingHash, false);
}