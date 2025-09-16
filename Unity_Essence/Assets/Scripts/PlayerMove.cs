using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : NetworkBehaviour
{
    Rigidbody2D rigidbody2D;
    PlayerAction playerAction;

    Animator animator;
    public const string ChangeHasName = "IsChange";

    [SerializeField] private float moveSpeed = 5f;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAction = new PlayerAction();
        playerAction.Player.Enable();
    }

    void Update()
    {
        // 내가 소유권을 가지고 있을때만 움직여라
        if (!IsOwner) { return; }

        Move();
        HandleAnimation();
    }

    private void HandleAnimation()
    {
        if (Keyboard.current.rKey.isPressed)
        {
            animator.SetBool(ChangeHasName, true);
        } else if (Keyboard.current.tKey.isPressed)
        {
            animator.SetBool(ChangeHasName, false);
        }        
    }

    private bool Move()
    {
        

        Vector2 moveDir = playerAction.Player.Move.ReadValue<Vector2>();
        rigidbody2D.linearVelocity = moveDir * moveSpeed;
        return true;
    }
}
