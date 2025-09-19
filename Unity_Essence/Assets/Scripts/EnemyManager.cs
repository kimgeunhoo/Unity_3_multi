using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyManager : NetworkBehaviour
{
    [SerializeField] Enemy enemyPrefab;

    [SerializeField] List<Enemy> enemyList;

    [Header("Common Monster Spawn Setting")]
    [SerializeField] private int count = 10;
    [SerializeField] Transform SpawnPosition;

    [Header("Boss Monster Spawn Setting")]
    [SerializeField] Transform BossSpaenPos;
    // ���� ��ȯ ���� : ��ȯ�� ��� �Ϲ� ���Ͱ� �������� ��ȯ
    public List<Enemy> spawnedEnemy = new();

    public void Spawn()
    {
        Enemy instance = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);

        instance.GetComponent<NetworkObject>().Spawn();
    }

    public void Spawn(int index)
    {
        Enemy instance = Instantiate(enemyList[index], Vector3.zero, Quaternion.identity);
        instance.GetComponent<NetworkObject>().Spawn();
    }

    public void Spawn(int index, Vector3 targetPos)
    {
        Enemy instance = Instantiate(enemyList[index], targetPos, Quaternion.identity);
        instance.GetComponent<NetworkObject>().Spawn();
    }

    private void Start()
    {
        
        for (int i = 0; i < count; i++)
        {
            int rand = UnityEngine.Random.Range(-5, 6);

            Vector3 randomPosition = new Vector3(
                SpawnPosition.position.x + rand, SpawnPosition.position.y + rand, 0);

            Spawn(0, randomPosition);

            // random���� ���� ���� ���� Ȯ�� ����.
            // ���� ��ȯ�� ��ġ�� �̹� ��ȯ�� ��ü�� �����Ѵٸ� ��ȯ x
        }

    }

    public override void OnNetworkSpawn()
    {
        if(!IsServer) { return; }

        Bus<IEnemyDeathEvent>.OnEvent += HandleMonsterDeath;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer) { return; }

        Bus<IEnemyDeathEvent>.OnEvent -= HandleMonsterDeath;
    }

    private void HandleMonsterDeath(IEnemyDeathEvent evt)
    {
        spawnedEnemy.Remove(evt.enemy);

        // ���� spawnedEnemy.count ������ 0���� �۰ų� ������ ���� ��ȯ

        if (spawnedEnemy.Count <= 0)
        {
            Spawn(1, BossSpaenPos.position);
        }

    }

    public void Update()
    {
        if(!IsServer) { return; }
        if(!IsSpawned) { return; }

        if(Keyboard.current.oKey.isPressed)
        {
            Spawn(0);
        }
        if(Keyboard.current.pKey.isPressed)
        {
            Spawn(1);
        }
    }

    /// ���� ��ȯ�� �����ϴ� �Լ�
    /// �ѹ��� ������ �� ���ΰ�?          -> Start
    /// �������� ���� ����� ���ΰ�?      -> Update, Event

    

    /// �Ϲ� ���͸� ��ȯ�Ѵ�.
    /// ���� ��ȯ�ϰ� ��ȯ�� ���͸� SpawnedMonster ����
    /// ���� ��� óġ �� ���� ���� ��ȯ �̺�Ʈ ����
    /// Spawn(1)

    /// Vector3.zero ��ſ� Spawn(int index, Vector3 spawnPos ��ġ ���� ���޹޴� ����)
    /// ���Ͱ� ��ȯ�� ��ġ�� �����ؼ� �����
    /// RandomPos�� Random �ڵ� ����Ͽ� ����
}
