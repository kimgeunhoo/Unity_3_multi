using System;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private int MaxHealth = 10;
    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();

    private bool isDead = false;
    public Action<Health> OnDie;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }

        CurrentHealth.Value = MaxHealth;
    }

    public override void OnNetworkDespawn()
    {

    }

    public void TakeDamage(int value)
    { 
        // 현재 체력에서 value 빼주는것
        ModifyHealth(-value);
    }

    public void RestoreHealth(int value)
    {
        // 현재 체력에서 value 더해주는것
        ModifyHealth(+value);
    }

    private void ModifyHealth(int value)
    {
        if (isDead) { return; }        // 죽었으면? -> 실행 x
        int newHealth = CurrentHealth.Value + value;
        //CurrentHealth.Value = newHealth;            // newHealth 버그로 인해 무한대, 최대체력 넘어간다
        CurrentHealth.Value = Mathf.Clamp(newHealth, 0, MaxHealth); // if else if 비교문 가독성 개선

        if(CurrentHealth.Value == 0)
        {
            OnDie?.Invoke(this); // 이 객체가 사망했습니다. 이벤트 발생
            isDead = true;// isDead = true;
        }
    }

}
