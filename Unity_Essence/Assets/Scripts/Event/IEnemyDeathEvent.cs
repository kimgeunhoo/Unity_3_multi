using UnityEngine;

public class IEnemyDeathEvent : IEvent
{
    public Enemy enemy;

    public IEnemyDeathEvent(Enemy enemy)
    {
        this.enemy = enemy;
    }

}
