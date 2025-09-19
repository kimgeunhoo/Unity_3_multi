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
            // NextStage 호출
            Debug.Log("스테이지 클리어 이벤트 호출");
            Bus<IStageClearEvent>.Raise(new IStageClearEvent());

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 여러 컴퓨터에서 서버 컴퓨터에 각자의 플레이어를 움직이는 정보를 전달
        // 생성된 객체가 서버 컴퓨터 안에서 충돌했을 때 발생하는 이벤트.

        triggerColliders.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        triggerColliders.Remove(collision);
    }

    private void FixedUpdate() // 물리적 계산을 처리할 때 호출하면 좋다. TriggerEvent 발생 후에 실행 보장
    {
        if(!IsServer) { return; }

        if (!IsSpawned) { return; }
        triggerColliders.RemoveAll(col => col == null); // 예외처리, null인 상태로 저장이되면 그 null 삭제

        isSwitchOn.Value = triggerColliders.Count > 0; // List 갯수가 1보다 크면 true, 아니면 false

    }

}
