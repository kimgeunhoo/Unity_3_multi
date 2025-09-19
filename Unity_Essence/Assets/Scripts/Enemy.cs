using System;
using Unity.Netcode;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
    [SerializeField] Health health;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }

        health.OnDie += HandleEnemyDie;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer) { return; }

        health.OnDie -= HandleEnemyDie;
    }

    protected virtual void HandleEnemyDie(Health health)
    {
        // �׾��� �� ����Ʈ
        // �׾��� �� ����

        Bus<IEnemyDeathEvent>.Raise(new IEnemyDeathEvent(this));
        NetworkManager.Destroy(gameObject); //Scene���� ���ֶ�
    }
}
