using UnityEngine;

public class DamageEnemy : DamageDealer
{
    public void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.gameObject.GetComponentInParent<EnemyHealth>();
        if (enemy)
        {
            DealDamage(enemy, damage);
        }
    }
}
