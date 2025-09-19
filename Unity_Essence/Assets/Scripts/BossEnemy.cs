using UnityEngine;

public class BossEnemy : Enemy
{

    protected override void HandleEnemyDie(Health health)
    {
        Bus<IStageClearEvent>.Raise(new IStageClearEvent());
        base.HandleEnemyDie(health); // Enemy 작성한 HandleEnemyDie 실행

    }
}
