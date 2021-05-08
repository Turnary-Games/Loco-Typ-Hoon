using UnityEngine;

public class EnemyAttackSpawning : MonoBehaviour
{
    public PlayerController player;
    public GameObject tentaclePrefab;

    public float intervalMin = 4;
    public float intervalMax = 6;
    public float estPosSecondsAheadMin = 2.5f;
    public float estPosSecondsAheadMax = 5f;

    public float nextSpawnInSeconds = 10;

    public void OnEnable()
    {
        if (!player)
        {
            Debug.LogWarning($"The '{nameof(player)}' field must be set.", this);
            enabled = false;
            return;
        }
        else if (!tentaclePrefab)
        {
            Debug.LogWarning($"The '{nameof(tentaclePrefab)}' field must be set.", this);
            enabled = false;
            return;
        }
    }

    public void Update()
    {
        if (nextSpawnInSeconds <= 0)
        {
            nextSpawnInSeconds = Random.Range(intervalMin, intervalMax);
            var aheadSeconds = Random.Range(estPosSecondsAheadMin, estPosSecondsAheadMax);
            var pos = player.EstimatePositionAfterSeconds(aheadSeconds);
            Instantiate(tentaclePrefab, pos, Quaternion.Euler(0, Random.Range(0, 360), 0));
        }
        else
        {
            nextSpawnInSeconds -= Time.deltaTime;
        }
    }
}
