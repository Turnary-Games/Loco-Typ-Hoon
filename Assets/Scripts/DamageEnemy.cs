using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    [Range(0, 50)]
    public int damage = 5;

    public void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.gameObject.GetComponentInParent<EnemyHealth>();
        if (enemy)
        {
            enemy.DealDamage(damage);
        }
    }
}
