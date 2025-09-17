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
    [SerializeField] private Transform projectileTransform;

    public Action<bool> FireAction;// ���� �߻� ������(����, Spacebar) ������ �� ���� ��� �ڵ忡�� �Ѱ��ֱ� ����

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAction = new PlayerAction();
        playerAction.Player.Enable();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { return; }

        playerAction.Player.Fire.performed += OnFirePerformed;
        playerAction.Player.Fire.canceled += OnFireCanceled;
    }
    public override void OnNetworkDespawn()
    {
        if (!IsOwner) { return; }

        playerAction.Player.Fire.performed -= OnFirePerformed;
        playerAction.Player.Fire.canceled -= OnFireCanceled;
    }

    private void OnFirePerformed(InputAction.CallbackContext context)
    {
        FireAction?.Invoke(true);
    }

    private void OnFireCanceled(InputAction.CallbackContext context)
    {
        FireAction?.Invoke(false);
    }

    void Update()
    {
        // ���� �������� ������ �������� ��������
        if (!IsOwner) { return; }


        Move();
        HandleAnimation(); 
    }

    private void LateUpdate()
    {
        if (!IsOwner) { return; }

        Vector2 ScreenPos = playerAction.Player.Aim.ReadValue<Vector2>();
        Vector2 WorldPos = Camera.main.ScreenToWorldPoint(ScreenPos);

        projectileTransform.up = new Vector3(WorldPos.x - projectileTransform.position.x, 
            WorldPos.y - projectileTransform.position.y, 0);
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
