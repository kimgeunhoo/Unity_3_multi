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


    }

    private void HandleEnemyDie(Health health)
    {
        // 죽엇을 때 이펙트
        // 죽엇을 때 사운드

        Destroy(gameObject);
    }
}
