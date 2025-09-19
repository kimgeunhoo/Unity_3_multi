using System;
using Unity.Netcode;
using UnityEngine;

public class ProjectileLauncher : NetworkBehaviour
{
    private PlayerMove player;
    private bool shouldFire = false;

    [SerializeField] Transform ProjectileLauncherTransform;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] GameObject clientProjectilePrefab;
    [SerializeField] GameObject serverProjectilePrefab;
    [SerializeField] float fireRate = 2f;

    private float previousFireTime;

    private void Awake()
    {
        player = GetComponentInParent<PlayerMove>();
    }

    public override void OnNetworkSpawn()
    {
        if(!IsOwner) { return; }
        player.FireAction += HandleFire;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) { return; }
        player.FireAction -= HandleFire;
    }

    private void HandleFire(bool fire)
    {
        shouldFire = fire;
    }

    private void Update()
    {
        if(!shouldFire) { return; }

        // 다음 발사까지 남았는가? FireRate
        if(Time.time < (1 / fireRate) + previousFireTime) { return; }

        FireServerRpc(ProjectileLauncherTransform.position, ProjectileLauncherTransform.up);
        SpawnDummyProjectile(ProjectileLauncherTransform.position, ProjectileLauncherTransform.up); // 투사체를 생성한다 서버에게 알려준다

        // 내 컴퓨터에서 내가 투사체를 발사했다.

        previousFireTime = Time.time;
    }

    [Rpc(SendTo.Server)]
    private void FireServerRpc(Vector3 position, Vector3 up)
    {
        GameObject projectileInstance =
            Instantiate(serverProjectilePrefab, position, Quaternion.identity);

        if (projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.linearVelocity = up * projectileSpeed;
        }

        if(projectileInstance.TryGetComponent<DealDamageOnContact>(out DealDamageOnContact dealDamage))
        {
            dealDamage.SetOwner(OwnerClientId);
        }

        FireClientRpc(position, up);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void FireClientRpc(Vector3 position, Vector3 up)
    {
        if(IsOwner) { return; }

        SpawnDummyProjectile(position, up);
    }

    private void SpawnDummyProjectile(Vector3 position, Vector3 up)
    {
        GameObject projectileInstance = 
            Instantiate(clientProjectilePrefab, position, Quaternion.identity);

        if (projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.linearVelocity = up * projectileSpeed;
        }
    }
}
