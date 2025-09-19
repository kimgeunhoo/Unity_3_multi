using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Switch : NetworkBehaviour
{
    // NetworkObject 부착한 오브젝트는 NetworkBehaviour 상속시키도록 수정

    // NetworkVariable 선언하기, 유저가 충돌 이벤트를 했을때 작동하는지 안하는지 체크하는 오브젝트
    // bool IsSwitchOn

    public NetworkVariable<bool> isSwitchOn = new NetworkVariable<bool>(false, 
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
        );
    
    List<Collider2D> triggerColliders = new(); // 서버에서 충돌이벤트가 발생했을 때 그 충돌체를 저장하는 변수

    // OnNetworkSpawn, OnNetworkDespawn 오브젝트가 누구의 제어를 받아야 하는지 생각. (Server, <Host, Client>)

    public override void OnNetworkSpawn()
    {
        if (!IsServer) 
        {
            enabled = false;    
        }

        isSwitchOn.OnValueChanged += OnSwitchChanged;
    }

    private void OnSwitchChanged(bool previousValue, bool newValue)
    {
        // 스위치가 On이 되어 animator 실행
    }

    public override void OnNetworkDespawn()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 여러 컴퓨터에서 서버 컴퓨터에 각자의 플레이어를 움직이는 정보를 전달
        // 생성된 객체가 서버 컴퓨터 안에서 충돌했을 때 발생하는 이벤트.

        triggerColliders. Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        triggerColliders.Remove(collision);
    }

    private void FixedUpdate() // 물리적 계산을 처리할 때 호출하면 좋다. TriggerEvent 발생 후에 실행 보장
    {
        if(!IsSpawned) { return; }
        triggerColliders.RemoveAll(col => col == null); // 예외처리, null인 상태로 저장이되면 그 null 삭제

        isSwitchOn.Value = triggerColliders.Count > 0; // List 갯수가 1보다 크면 true, 아니면 false

    }

}
