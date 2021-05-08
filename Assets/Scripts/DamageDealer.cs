using UnityEngine;

public abstract class DamageDealer : MonoBehaviour
{
    [Range(0, 50)]
    public int damage = 5;

    protected void DealDamage(HealthScript target, int damage)
    {
        target.DealDamage(damage);

        SendMessageUpwards(nameof(IOnDamageDealt.OnDamageDealt), new DamageDealtEvent
        {
            damage = damage,
            target = target,
        }, SendMessageOptions.DontRequireReceiver);
    }

    public interface IOnDamageDealt
    {
        void OnDamageDealt(DamageDealtEvent data);
    }

    public struct DamageDealtEvent
    {
        public int damage;
        public HealthScript target;
    }
}
