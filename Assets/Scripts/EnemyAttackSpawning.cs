using UnityEngine;

public class EnemyAttackSpawning : MonoBehaviour
{
    public PlayerController player;
    public GameObject tentaclePrefab;

    [Header("Distances")]
    public float radiusNearest = 10f;
    public float radiusBortastest = 100f;

    [Header("Spawning interval")]
    public float delaySecondsMin = 4;
    public float delaySecondsMax = 6;
    public float randomAdditionalSpawnTime = 2;
    public AnimationCurve delayMinToMaxPerNearestToBortastest = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public float delaySinceOutOfBounds = 10f;
    public float nextSpawnInSeconds = 10;

    [Header("Spawning location")]
    public float estPosSecondsAheadMin = 2.5f;
    public float estPosSecondsAheadMax = 5f;

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

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireSphere(transform.position, radiusNearest);
        Gizmos.color = new Color(1, 0.27f, 0, 0.5f);
        Gizmos.DrawLine(transform.position + Vector3.forward * radiusNearest, transform.position + Vector3.forward * radiusBortastest);
        Gizmos.DrawLine(transform.position + Vector3.back * radiusNearest, transform.position + Vector3.back * radiusBortastest);
        Gizmos.DrawLine(transform.position + Vector3.right * radiusNearest, transform.position + Vector3.right * radiusBortastest);
        Gizmos.DrawLine(transform.position + Vector3.left * radiusNearest, transform.position + Vector3.left * radiusBortastest);
        Gizmos.DrawWireSphere(transform.position, radiusBortastest);
    }

    public void Update()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, player.engine.transform.position);
        if (distanceToPlayer > radiusBortastest)
        {
            nextSpawnInSeconds = delaySinceOutOfBounds;
            return;
        }

        if (nextSpawnInSeconds <= 0)
        {
            float invLerpNearToBort = Mathf.InverseLerp(radiusNearest, radiusBortastest, distanceToPlayer);
            nextSpawnInSeconds = Mathf.Lerp(delaySecondsMin, delaySecondsMax, delayMinToMaxPerNearestToBortastest.Evaluate(invLerpNearToBort));

            nextSpawnInSeconds += Random.value * randomAdditionalSpawnTime;

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
