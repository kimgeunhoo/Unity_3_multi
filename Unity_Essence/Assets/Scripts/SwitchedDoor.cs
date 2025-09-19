using System;
using Unity.Netcode;
using UnityEngine;

public class SwitchedDoor : NetworkBehaviour
{
    [SerializeField] Switch[] switchThatOpenThisDoor;
    public NetworkVariable<bool> IsOpen { get; } = new NetworkVariable<bool>(false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
        );

    public GameObject physicsObject;
    Animator animator;
    const string DoorOpenName = "IsOpen";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            animator.SetBool(DoorOpenName, IsOpen.Value); // NetCode NetworkVariable ������ ���������� �ٷ�����
        }
        IsOpen.OnValueChanged += OnDoorChange;
    }

    public override void OnNetworkDespawn()
    {
        IsOpen.OnValueChanged -= OnDoorChange;
    }

    private void OnDoorChange(bool wasDoorOpen, bool IsDoorOpen)
    {
        if(IsServer)
        {
            animator.SetBool(DoorOpenName, IsDoorOpen);
        }

        if(IsClient)
        {
            physicsObject.SetActive(!IsDoorOpen); // newValue ("True" ���� ������ �� -> false)
        }
    }

    private void Update()
    {
        if(IsServer && IsSpawned)
        {
            bool isAnySwitchOn = false;

            foreach(var sw in switchThatOpenThisDoor)
            {
                if (sw && sw.isSwitchOn.Value) // sw �����ϰ� �� ���� true�� ��
                {
                    isAnySwitchOn = true;
                    break;
                }
            }

            IsOpen.Value = isAnySwitchOn;
        }
    }
}
