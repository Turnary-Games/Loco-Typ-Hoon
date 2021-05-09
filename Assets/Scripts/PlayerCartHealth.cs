public class PlayerCartHealth : HealthScript
{
    public PlayerCartHealth damageOtherFirst;
    public PlayerCartHealth healOtherFirst;
    public PlayerCartHealth healOtherAfter;

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

    public override void ApplyHealing(int heal)
    {
        if (healOtherFirst && healOtherFirst.currentHealth < healOtherFirst.maxHealth)
        {
            healOtherFirst.ApplyHealing(heal);
        }
        else
        {
            var old_health = currentHealth;
            base.ApplyHealing(heal);
            if (healOtherAfter && old_health + heal > maxHealth)
            {
                healOtherAfter.ApplyHealing(old_health + heal - maxHealth);
            }
        }
    }
}
