using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyZombie zombie = other.GetComponentInParent<EnemyZombie>();

        if (zombie != null)
        {
            Vector2 dir = ((Vector2)zombie.transform.position - (Vector2)transform.position).normalized;
            zombie.TakeDamage(damage, dir);
            Destroy(gameObject);
        }
    }
}
