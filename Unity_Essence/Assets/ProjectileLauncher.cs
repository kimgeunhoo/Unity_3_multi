using System;
using Unity.Netcode;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    private PlayerMove player;
    private bool shouldFire = false;

    [SerializeField] Transform ProjectileLauncherTransform;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] GameObject clientProjectilePrefab;
    [SerializeField] GameObject serverProjectilePrefab;
    [SerializeField] float fireRate = 2f;

    private float previousFireTime;

    private void Awake()
    {
        player = GetComponentInParent<PlayerMove>();
    }

    private void OnEnable()
    {
        player.FireAction += HandleFire;
    }
    private void OnDisable()
    {
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

        PrimaryFire(ProjectileLauncherTransform.position, ProjectileLauncherTransform.up);

        previousFireTime = Time.time;
    }

    private void PrimaryFire(Vector3 position, Vector3 up)
    {
        GameObject projectileInstance = 
            Instantiate(gameObject,transform.position, Quaternion.identity);

        if (projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.linearVelocity = transform.up * projectileSpeed;
        }
    }
}
