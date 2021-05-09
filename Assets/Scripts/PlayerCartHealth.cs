public class PlayerCartHealth : HealthScript
{
    public PlayerCartHealth damageOtherFirst;

    public override void DealDamage(int damage)
    {
        if (damageOtherFirst && damageOtherFirst.currentHealth > 0)
        {
            damageOtherFirst.DealDamage(damage);
        }
        else
        {
            base.DealDamage(damage);
        }
    }
}
