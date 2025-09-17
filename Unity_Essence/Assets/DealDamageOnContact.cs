using UnityEngine;

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    private ulong ownerClientId; // NetworkBehavior���� �ڱ��ڽ��� ��Ʈ��ũ ID�� �����Ѵ�.

    public void SetOwner(ulong id)
    {
        ownerClientId = id;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody == null) { return; }     // collision.GetCoponent<Rigidbody>
        
        if (collision.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(damage);
        }
    }
}
