using UnityEngine;

public class DelaySpawningWhenDamaged : MonoBehaviour, PlayerCartHealth.IOnDamagedEvent
{
    public EnemyAttackSpawning spawning;

    public float delaySecondsRandomMin = 4f;
    public float delaySecondsRandomMax = 5f;
    public float waitAtLeastSeconds = 8f;
    public float sumUpToAtMostSeconds = 20f;

    public void OnEnabled()
    {
        if (!spawning)
        {
            Debug.LogWarning($"The '{nameof(spawning)}' field must be set.", this);
            enabled = false;
            return;
        }
    }

    public void Start()
    {
        // kept just to force the "enabled" checkmark in the inspector
    }

    public void OnDamaged(PlayerCartHealth.DamagedEvent data)
    {
        Debug.Log("Wao " + enabled);
        if (!enabled)
        {
            return;
        }

        var newNextSpawnInSeconds = Mathf.Max(
            spawning.nextSpawnInSeconds + Random.Range(delaySecondsRandomMin, delaySecondsRandomMax),
            waitAtLeastSeconds);

        if (newNextSpawnInSeconds > sumUpToAtMostSeconds && spawning.nextSpawnInSeconds < sumUpToAtMostSeconds)
        {
            newNextSpawnInSeconds = sumUpToAtMostSeconds;
        }

        spawning.nextSpawnInSeconds = newNextSpawnInSeconds;
    }
}
