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
    // 보스 소환 조건 : 소환된 모든 일반 몬스터가 없어지면 소환
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

            // random에서 같은 값이 나올 확률 존재.
            // 지금 소환된 위치에 이미 소환된 객체가 존재한다면 소환 x
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

        // 만약 spawnedEnemy.count 갯수가 0보다 작거나 같으면 보스 소환

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

    /// 몬스터 소환을 실행하는 함수
    /// 한번만 실행이 될 것인가?          -> Start
    /// 여러번에 걸쳐 실행될 것인가?      -> Update, Event

    

    /// 일반 몬스터를 소환한다.
    /// 몬스터 소환하고 소환된 몬스터를 SpawnedMonster 저장
    /// 몬스터 모두 처치 시 보스 몬스터 소환 이벤트 실행
    /// Spawn(1)

    /// Vector3.zero 대신에 Spawn(int index, Vector3 spawnPos 위치 값을 전달받는 인자)
    /// 몬스터가 소환될 위치를 선택해서 만들기
    /// RandomPos를 Random 코드 사용하여 구현
}
