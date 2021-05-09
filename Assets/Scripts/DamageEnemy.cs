using UnityEngine;

public class DamageEnemy : DamageDealer
{
    public void OnCollisionEnter(Collision collision)
    {
        DealDamageToGameObject(collision.gameObject);
    }

    public void OnTriggerEnter(Collider collider)
    {
        DealDamageToGameObject(collider.gameObject);
    }

    void DealDamageToGameObject(GameObject obj)
    {
        var tentacle = obj.GetComponentInParent<EnemyTentacleController>();
        if (tentacle)
        {
            tentacle.GoDownAndSelfDestroy();
        }

        var enemy = obj.GetComponentInParent<EnemyHealth>();
        if (enemy)
        {
            DealDamage(enemy, damage);
        }
    }
}
