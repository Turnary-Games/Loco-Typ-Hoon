using UnityEngine;

public class GameOverAfterDeath : MonoBehaviour, HealthScript.IOnDamagedEvent
{
    public bool isWin = true;

    public void OnDamaged(HealthScript.DamagedEvent data)
    {
        if (data.currentHealth <= 0 && data.previousHealth > 0)
        {
            GameOverScript.SetGameOver(isWin);
        }
    }
}
