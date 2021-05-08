using UnityEngine;
using UnityEngine.UI;

public class PlayerCartHealthUI : MonoBehaviour, PlayerCartHealth.IOnDamagedEvent
{
    public Image radialImage;

    public void OnDamaged(PlayerCartHealth.DamagedEvent data)
    {
        if (!radialImage)
        {
            Debug.LogWarning($"The '{nameof(radialImage)}' field must be set.", this);
            enabled = false;
            return;
        }

        radialImage.fillAmount = data.currentHealth / (float)data.maxHealth;
    }
}
