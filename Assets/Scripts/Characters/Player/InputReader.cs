using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public event Action JumpEnabled;

    public Vector2 MoveDirection { get; private set; }

    public void OnMove(InputAction.CallbackContext context) =>
        MoveDirection = context.ReadValue<Vector2>();

    public void OnJump(InputAction.CallbackContext context) =>
        JumpEnabled?.Invoke();
}