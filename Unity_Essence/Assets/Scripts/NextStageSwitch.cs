using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NextStageSwitch : NetworkBehaviour
{
    public NetworkVariable<bool> isSwitchOn = new NetworkVariable<bool>(false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    List<Collider2D> triggerColliders = new();

    public override void OnNetworkSpawn()
    {
        if(!IsServer)
        {
            enabled = false;
        }

        isSwitchOn.OnValueChanged += NextStageChanged;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer)
        {
            enabled = false;
        }

        isSwitchOn.OnValueChanged -= NextStageChanged;
    }

    private void NextStageChanged(bool previousValue, bool newValue)
    {
        if(newValue == true)
        {
            // NextStage ȣ��
            Debug.Log("�������� Ŭ���� �̺�Ʈ ȣ��");
            Bus<IStageClearEvent>.Raise(new IStageClearEvent());

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ��ǻ�Ϳ��� ���� ��ǻ�Ϳ� ������ �÷��̾ �����̴� ������ ����
        // ������ ��ü�� ���� ��ǻ�� �ȿ��� �浹���� �� �߻��ϴ� �̺�Ʈ.

        triggerColliders.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        triggerColliders.Remove(collision);
    }

    private void FixedUpdate() // ������ ����� ó���� �� ȣ���ϸ� ����. TriggerEvent �߻� �Ŀ� ���� ����
    {
        if(!IsServer) { return; }

        if (!IsSpawned) { return; }
        triggerColliders.RemoveAll(col => col == null); // ����ó��, null�� ���·� �����̵Ǹ� �� null ����

        isSwitchOn.Value = triggerColliders.Count > 0; // List ������ 1���� ũ�� true, �ƴϸ� false

    }

}
