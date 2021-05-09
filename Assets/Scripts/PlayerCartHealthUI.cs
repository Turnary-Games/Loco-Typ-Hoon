using UnityEngine;
using UnityEngine.UI;

public class PlayerCartHealthUI : MonoBehaviour, HealthScript.IOnDamagedEvent, HealthScript.IOnHealedEvent
{
    public Image radialImage;

    public void OnDamaged(HealthScript.DamagedEvent data)
    {
        UpdateRadialImage(data.currentHealth, data.maxHealth);
    }

    public void OnHealed(HealthScript.HealedEvent data)
    {
        UpdateRadialImage(data.currentHealth, data.maxHealth);
    }

    void UpdateRadialImage(int currentHealth, int maxHealth)
    {
        if (!radialImage)
        {
            Debug.LogWarning($"The '{nameof(radialImage)}' field must be set.", this);
            enabled = false;
            return;
        }

        radialImage.fillAmount = currentHealth / (float)maxHealth;
    }
}
