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

    public void OnTriggerEnter(Collider collider)
    {
        var tentacle = collider.GetComponentInParent<EnemyTentacleController>();

        if (tentacle)
        {
            tentacle.GoDownAndSelfDestroy();
        }
    }
}
